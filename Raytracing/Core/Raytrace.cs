using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Raytracing.Core;
using Raytracing.Materials;

namespace Raytracing {

  public static partial class Raytrace {

    #region Public Methods

    public static void Calculate(CoreValues cv) {
      var myCamera = new Camera(cv.Origin, cv.LookAt, cv.VecUp, cv.FovY, cv.AspectRatio, cv.Aperture, cv.DistToFocus);

      var image = new Vec3[cv.Width * cv.Height];
      var scene = GenerateScene();

      var i = 0;
      Parallel.For(0, cv.Height, (y) => {
        var iwas = cv.Height - i++;
        Console.WriteLine(iwas);
        for (var x = 0; x < cv.Width; x++) {
          //Parallel.For(0, width, (x) => {
          var c = new Vec3(0, 0, 0);
          for (var s = 0; s < cv.Samplecount; s++) {
            var u = (x + MathR.Uniform(0, 0.99F)) / (cv.Width - 1);
            var v = (y + MathR.Uniform(0, 0.99F)) / (cv.Height - 1);
            var primary = myCamera.GetRay(u, v);

            c += GetColor(primary, scene, cv.maxDepth) / cv.Samplecount;
          }
          image[(y * cv.Width) + x] = c;
        }
      });
      PPM_Writer.Write(image, cv.Width, cv.Height, "a.ppm");
      Process.Start(@"E:\WinToUSB\Portable Apps\PortableApps\GIMPPortable\GIMPPortable.exe", @"E:\Proggn\Projects\GitRepos\GitRepos\Raytracing\Raytracing\bin\Debug\netcoreapp3.1\a.ppm");
      Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
    }

    public static Vec3 GetColor(Ray ray, HitableList scene, int depth) {
      if (depth == 0) {
        return new Vec3(0, 0, 0);
      }

      var hit = scene.GetNearestHit(ray, 0.0001F);

      if (hit != null) {
        var result = hit.Material.Scatter(ray, hit);
        if (result != default) {
          return GetColor(result.ray, scene, depth - 1) * result.color;
        }
      }
      var t = (MathR.Normalize(ray.Direction).Y + 1) * 0.5F;
      return (new Vec3(1, 1, 1) * (1.0F - t)) + (new Vec3(0.5F, 0.7F, 1) * t);
    }

    public static HitableList GenerateScene() {
      var world = new HitableList {
        //Bottom "plane"
        new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertain(new Vec3(0.5F, 0.5F, 0.5F))),

        // three large spheres
	      new Sphere(new Vec3(0, 1, 0), 1.0F, new Dielectric(1.5F)),
        new Sphere(new Vec3(-4, 1, 0), 1.0F, new Lambertain(new Vec3(0.4F, 0.2F, 0.1F))),
        new Sphere(new Vec3(4, 1, 0), 1.0F, new Metal(new Vec3(0.7F, 0.6F, 0.5F), 0.0F))
      };
      // numerous small sphere
      RandomizeSzene(world);

      return world;
    }

    #endregion Public Methods

    #region Private Methods

    private static void RandomizeSzene(HitableList world) {
      for (var a = -11; a < 11; a++) {
        for (var b = -11; b < 11; b++) {
          var chooseMat = MathR.Uniform(0, 1);
          var center = new Vec3(a + (0.9F * MathR.Uniform(0, 1)), 0.2F, b + (0.9F * MathR.Uniform(0, 1)));
          if ((center - new Vec3(4, 0.2F, 0)).Lenght() > 0.9F) {
            if (chooseMat < 0.8F) {
              // diffuse
              var albedo = Vec3.Random() * Vec3.Random();

              world.Add(new Sphere(center, 0.2F, new Lambertain(albedo)));
            }
            else if (chooseMat < 0.95) {
              // metal
              var albedo = MathR.Uniform(.5F, 1);

              var fuzz = MathR.Uniform(0, .5F);

              world.Add(new Sphere(center, 0.2F, new Metal(new Vec3(albedo, albedo, albedo), fuzz)));
            }
            else {
              // glass
              world.Add(new Sphere(center, 0.2F, new Dielectric(1.5F)));
            }
          }
        }
      }
    }

    #endregion Private Methods
  }
}