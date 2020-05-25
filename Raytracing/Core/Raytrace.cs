using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Raytracing {

  public static partial class Raytrace {
    public const string gimpPath = @"E:\WinToUSB\Portable Apps\PortableApps\GIMPPortable\GIMPPortable.exe";

    #region Public Methods

    public static void Calculate(CoreValues cv) {
      var image = new float[cv.Width * cv.Height * 4];
      var scene = HitableList.DefaultWorld;
      var stepping = 5;
      var m = Mat4.RotationY(stepping);

      for (var k = 0; k < 360; k += stepping) {
        var watch = new Stopwatch();
        watch.Start();

        cv.Origin = m * cv.Origin;
        var myCamera = new Camera(cv.Origin, cv.LookAt, cv.VecUp, cv.FovY, cv.AspectRatio, cv.Aperture, cv.DistToFocus);

        for (var s = 1; s <= cv.SamplesPerPixel; s++) {
          Parallel.For(0, cv.Height, new ParallelOptions { MaxDegreeOfParallelism = 2 * Environment.ProcessorCount }, (y) => {
            Console.Write(".");
            for (var x = 0; x < cv.Width; x++) {
              var pixelColor = new Vec3(0, 0, 0);
              var u = (x + Rand.Rand01()) / (cv.Width - 1);
              var v = (y + Rand.Rand01()) / (cv.Height - 1);
              var primary = myCamera.GetRay(u, v);

              pixelColor += GetColor(primary, scene, cv.MaxDepth);

              var index = ((y * cv.Width) + x) * 4;
              image[index++] += pixelColor.R;
              image[index++] += pixelColor.G;
              image[index++] += pixelColor.B;
              image[index++] += 255;
            }
          });
          var filename = $"result{k:0000}:{s}.bmp";
          var imgToSave = image.Select(b => FinalizeColor(b, s)).ToArray();
          BMP_Writer.Save(filename, cv.Width, cv.Height, imgToSave);
          Process.Start(gimpPath, Path.Combine(Directory.GetCurrentDirectory(), filename));
          Console.WriteLine($"{watch.ElapsedMilliseconds}");
        }

        watch.Stop();
        Console.WriteLine($"Done with {""} {watch.ElapsedMilliseconds} ms passed");
      }
    }

    public static Vec3 GetColor(Ray ray, HitableList scene, int depth) {
      if (depth <= 0) {
        return new Vec3(0, 0, 0);
      }

      var hit = scene.GetNearestHit(ray, 0.0001F);

      if (hit != null) {
        var result = hit.Material.Scatter(ray, hit);
        if (result != default) {
          return GetColor(result.ray, scene, depth - 1) * result.color;
        }
        return new Vec3(0, 0, 0);
      }
      var t = (MathR.Normalize(ray.Direction).Y + 1) * 0.5F;
      return (new Vec3(1, 1, 1) * (1.0F - t)) + (new Vec3(0.5F, 0.7F, 1) * t);
    }

    public static byte[] FinalizeColor(Vec3 pixelColor, int samplesPerPixel) {
      (var r, var g, var b) = pixelColor;

      // Replace NaN components with zero. See explanation in Ray Tracing: The Rest of Your Life.
      //if (r != r) r = 0.0f;
      //if (g != g) g = 0.0f;
      //if (b != b)  b = 0.0f;

      // Divide the color by the number of samples and gamma-correct for gamma=2.0.
      var scale = 1.0f / samplesPerPixel;
      r = MathF.Sqrt(scale * r);
      g = MathF.Sqrt(scale * g);
      b = MathF.Sqrt(scale * b);

      // return the translated [0,255] value of each color component.
      return new[]{
        Convert.ToByte(255 * MathR.Clamp(r, 0.0f, 0.999f)),
        Convert.ToByte(255 * MathR.Clamp(g, 0.0f, 0.999f)),
        Convert.ToByte(255 * MathR.Clamp(b, 0.0f, 0.999f))};
    }

    public static byte FinalizeColor(float pixelColor, int samplesPerPixel) {
      var c = pixelColor;

      // Replace NaN components with zero. See explanation in Ray Tracing: The Rest of Your Life.
      if (float.IsNaN(c))
        c = 0.0f;

      // Divide the color by the number of samples and gamma-correct for gamma=2.0.
      var scale = 1.0f / samplesPerPixel;
      c = MathF.Sqrt(scale * c);

      // return the translated [0,255] value of each color component.
      return Convert.ToByte(255 * MathR.Clamp(c, 0.0f, 0.999f));
    }

    #endregion Public Methods
  }
}