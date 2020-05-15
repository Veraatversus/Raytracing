using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Raytracing.Core;
using Raytracing.Materials;

namespace Raytracing {

  public static class Raytrace {

    #region Public Methods

    public static void Calculate() {
      var aspectRatio = 16F / 9F;
      var width = 1920;
      var height = (int)(width / aspectRatio);
      var image = new Vec3[width * height];
      var maxDepth = 10;
      var samplecount = 50;
      //var origin = new Vec3(0, 0, 0);
      //var virtDist = -1F;
      //var virtWidth = 4F;
      //var virtHeight = 2F;

      //var lowerLeftCorner = new Vec3(-virtWidth / 2, -virtHeight / 2, virtDist);
      //var horizontal = new Vec3(virtWidth, 0, 0);
      //var vertical = new Vec3(0, virtHeight, 0);

      var scene = GenerateScene();

      var lookFrom = new Vec3(13, 2, 3);
      var lookAt = new Vec3(0, 0, 0);
      var vUp = new Vec3(0, 1, 0);
      var distToFocus = 10F;
      var aperture = 0.1F;
      var fovY = 20F;


      var myCamera = new Camera(lookFrom, lookAt, vUp, fovY, aspectRatio, aperture, distToFocus);
      //var xCoord = lowerLeftCorner.X + (x * horizontal.X / width);
      //var yCoord = lowerLeftCorner.Y + (y * vertical.Y / height);
      //var zCoord = virtDist;

      //var primary = new Ray(origin, new Vec3(xCoord, yCoord, zCoord));
      AllocConsole();
      var i = 0;
      Parallel.For(0, height, (y) => {
        var iwas = height - i++;
        Console.WriteLine(iwas);
        for (int x = 0; x < width; x++) {
          //Parallel.For(0, width, (x) => {
          var c = new Vec3(0, 0, 0);
          //var colors = Enumerable.Range(0, samplecount).AsParallel().AsOrdered().Select(s => {
          //  var u = (x + MathR.Uniform(0, 0.99F)) / (width - 1);
          //  var v = (y + MathR.Uniform(0, 0.99F)) / (height - 1);
          //  var primary = myCamera.GetRay(u, v);
          //  return GetColor(primary, scene, maxDepth) / samplecount;
          //});
          //foreach (var color in colors) {
          //  c += color;
          //}
          //image[(y * width) + x] = c;
          for (var s = 0; s < samplecount; s++) {
            var u = (x + MathR.Uniform(0, 0.99F)) / (width - 1);
            var v = (y + MathR.Uniform(0, 0.99F)) / (height - 1);
            var primary = myCamera.GetRay(u, v);

            c += GetColor(primary, scene, maxDepth) / samplecount;
          }
          image[(y * width) + x] = c;
          //image[y * width + x] = new Vec3((float)x / width, (float)y / height, 0);
          //image[(y * width) + x] = GetColor(primary, scene, maxDepth);
        }/*);*/
      });
      PPM_Writer.Write(image, width, height, "a.ppm");
      Process.Start(@"E:\WinToUSB\Portable Apps\PortableApps\GIMPPortable\GIMPPortable.exe", @"E:\Proggn\Projects\GitRepos\GitRepos\Raytracing\Raytracing\bin\Debug\netcoreapp3.1\a.ppm");
      Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());

    }

    public static Vec3 GetColor(Ray ray, HitableList scene, int depth) {
      //return new Vec3(ray.Direction.X, ray.Direction.Y, ray.Direction.Z);
      //return new Vec3(Clamp(ray.Direction.X), Clamp(ray.Direction.Y), Clamp(ray.Direction.Z));
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
      return new Vec3(1, 1, 1) * (1.0F - t) + new Vec3(0.5F, 0.7F, 1) * t;
      //return MathR.Lerp(new Vec3(0.5F, 0.7F, 1.0F), new Vec3(1, 1, 1), ray.Direction.Y.Clamp());
      //return new Vec3(Math.Abs(hit.N.X), Math.Abs(hit.N.Y), Math.Abs(hit.N.Z));
      //hit = (hit + new Vec3(1, 1, 1)) / 2;
      //return new Vec3(hit.X, hit.Y, hit.Z);
    }

    public static HitableList GenerateScene() {
      var world = new HitableList {
        //Bottom "plane"
        new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertain(new Vec3(0.5F, 0.5F, 0.5F))),

        // three large spheres
	      new Sphere(new Vec3(0, 1, 0), 1.0F, new Dielectric(1.5F)),
        new Sphere(new Vec3(-4, 1, 0), 1.0F, new Lambertain(new Vec3(0.4F, 0.2F, 0.1F))),
        new Sphere(new Vec3(4, 1, 0), 1.0F, new Metal(new Vec3(0.7F, 0.6F, 0.5F), 0.0F))

        //new Sphere(new Vec3(0, 1F, 0), 1, new Dielectric(1.5F)),
        //new Sphere(new Vec3(-4, 1, 0), 1, new Lambertain(new Vec3(0.4F, 0.2F, 0.1F))),
        //new Sphere(new Vec3(4, 1F, 0), 0.5F, new Metal(new Vec3(0.7F, 0.6F, 0.5F), 0)),

        //new Sphere(new Vec3(8, 0, -10), 2, new Lambertain(new Vec3(1, 1, 0))),            new Sphere(new Vec3(2, -1, -5), 2, new Metal(new Vec3(0.9F, 0.9F, 0.9F), 0.5F)),
        //new Sphere(new Vec3(-2, -0.5F, -5), 2, new Dielectric(1.3F)),
        //new Sphere(new Vec3(-2, -0.5F, -5), 0.5F, new Lambertain(new Vec3(0, 0, 1))),
        //new Sphere(new Vec3(8, 0, -10), 2, new Lambertain(new Vec3(1, 1, 0))),
        //new Sphere(new Vec3(-2, -1, -5), 2, new Metal(new Vec3(0.9F, 0.9F, 0.9F), 0)),

      };
      // numerous small sphere
      RandomizeSzene(world);
      //for (int i = 0; i < 20; i++) {
      //  scene.Add(new Sphere(new Vec3(GetRan(), GetRan(), GetRan()), random.Next(2, 50)));
      //}

      return world;
    }

    private static void RandomizeSzene(HitableList world) {
      for (int a = -11; a < 11; a++) {
        for (int b = -11; b < 11; b++) {
          var chooseMat = MathR.Uniform(0, 1);
          var center = new Vec3(a + 0.9F * MathR.Uniform(0, 1), 0.2F, b + 0.9F * MathR.Uniform(0, 1));
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

    #endregion Public Methods
    [DllImport("Kernel32")]
    public static extern void AllocConsole();

  }
}