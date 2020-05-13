using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Raytracing.Core;
using Raytracing.Materials;

namespace Raytracing {

  public static class Raytrace {

    #region Public Methods

    public static void Calculate() {
      var width = 800;
      var height = 400;

      var image = new Vec3[width * height];
      var origin = new Vec3(0, 0, 0);
      var virtDist = -1F;
      var virtWidth = 4F;
      var virtHeight = 2F;
      var maxDepth = 10;
      var samplecount = 50;

      var lowerLeftCorner = new Vec3(-virtWidth / 2, -virtHeight / 2, virtDist);
      var horizontal = new Vec3(virtWidth, 0, 0);
      var vertical = new Vec3(0, virtHeight, 0);

      var scene = GenerateScene();

      Parallel.For(0, height, (y) => {
        Parallel.For(0, width, (x) => {
          var xCoord = lowerLeftCorner.X + (x * horizontal.X / width);
          var yCoord = lowerLeftCorner.Y + (y * vertical.Y / height);
          var zCoord = virtDist;

          var primary = new Ray(origin, new Vec3(xCoord, yCoord, zCoord));

          for (var s = 0; s < samplecount; s++) {
            var oldColor = image[(y * width) + x] ?? new Vec3();
            var newColor = GetColor(primary, scene, maxDepth);

            var mixColor = new Vec3(oldColor.R + (newColor.R / samplecount), oldColor.G + (newColor.G / samplecount), oldColor.B + (newColor.B / samplecount));
            image[(y * width) + x] = mixColor;
          }
          //image[y * width + x] = new Vec3((float)x / width, (float)y / height, 0);
          //image[(y * width) + x] = GetColor(primary, scene, maxDepth);
        });
      });
      PPM_Writer.Write(image, width, height, "a.ppm");
      Process.Start(@"E:\WinToUSB\Portable Apps\PortableApps\GIMPPortable\GIMPPortable.exe", @"E:\Proggn\Projects\GitRepos\GitRepos\Raytracing\Raytracing\bin\Debug\netcoreapp3.1\a.ppm");
      Application.Current.Shutdown();
    }

    public static Vec3 GetColor(Ray ray, HitableList scene, int depth) {
      //return new Vec3(ray.Direction.X, ray.Direction.Y, ray.Direction.Z);
      //return new Vec3(Clamp(ray.Direction.X), Clamp(ray.Direction.Y), Clamp(ray.Direction.Z));
      var hit = scene.GetNearestHit(ray, 0.0001F);

      if (depth == 0) {
        return new Vec3(0, 0, 0);
      }

      if (hit != null) {
        var result = hit.Material.Scatter(ray, hit);
        if (result != default) {
          var color = GetColor(result.ray, scene, depth - 1);
          return new Vec3(color.R * result.color.R, color.G * result.color.G, color.B * result.color.B);
        }
      }
      return MathR.Lerp(new Vec3(0.5F, 0.7F, 1.0F), new Vec3(1, 1, 1), ray.Direction.Y.Clamp());
      //return new Vec3(Math.Abs(hit.N.X), Math.Abs(hit.N.Y), Math.Abs(hit.N.Z));
      //hit = (hit + new Vec3(1, 1, 1)) / 2;
      //return new Vec3(hit.X, hit.Y, hit.Z);
    }

    public static HitableList GenerateScene() {
      var scene = new HitableList {
        new Sphere(new Vec3(2, -1, -5), 2, new Metal(new Vec3(0.9F, 0.9F, 0.9F), 0.5F)),
        new Sphere(new Vec3(-2, -0.5F, -5), 2, new Dielectric(1.3F)),
        new Sphere(new Vec3(-2, -0.5F, -5), 0.5F, new Lambertain(new Vec3(0,0, 1))),
        new Sphere(new Vec3(8, 0, -10), 2, new Lambertain(new Vec3(1,1, 0))),
        //new Sphere(new Vec3(-2, -1, -5), 2, new Metal(new Vec3(0.9F, 0.9F, 0.9F), 0)),
        new Sphere(new Vec3(0, -1003, -10), 1000, new Lambertain(new Vec3(0.9F, 0.9F, 0.9F))),
      };

      //for (int i = 0; i < 20; i++) {
      //  scene.Add(new Sphere(new Vec3(GetRan(), GetRan(), GetRan()), random.Next(2, 50)));
      //}

      return scene;
    }

    #endregion Public Methods
  }
}