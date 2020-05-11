using System;
using System.Diagnostics;
using System.Windows;

namespace Raytracing {

  public class Raytrace {

    #region Public Methods

    public void Calculate() {
      var width = 800;
      var height = 800;
      var image = new Vec3[width * height];

      var origin = new Vec3(0, 0, 0);
      var virtDist = -1F;
      var virtWidth = 4F;
      var virtHeight = 2F;

      var lowerLeftCorner = new Vec3(-virtWidth / 2, -virtHeight / 2, virtDist);
      var horizontal = new Vec3(virtWidth, 0, 0);
      var vertical = new Vec3(0, virtHeight, 0);

      var scene = GenerateScene();

      var i = 0;
      for (var y = 0; y < height; y++) {
        for (var x = 0; x < width; x++) {
          var xCoord = lowerLeftCorner.X + x * horizontal.X / width;
          var yCoord = lowerLeftCorner.Y + y * vertical.Y / height;
          var zCoord = virtDist;

          var primary = new Ray(origin, new Vec3(xCoord, yCoord, zCoord));
          //image[i] = new Vec3((float)x / width, (float)y / height, 0);
          image[i] = GetColor(primary, scene);

          i++;
        }
      }
      PPM_Writer.Write(image, width, height, "a.ppm");
      Process.Start(@"E:\WinToUSB\Portable Apps\PortableApps\GIMPPortable\GIMPPortable.exe", @"E:\Proggn\Projects\GitRepos\GitRepos\Raytracing\Raytracing\bin\Debug\netcoreapp3.1\a.ppm");
      Application.Current.Shutdown();
    }

    public Vec3 GetColor(Ray ray, HitableList scene) {
      //return new Vec3(ray.Direction.X, ray.Direction.Y, ray.Direction.Z);
      //return new Vec3(Clamp(ray.Direction.X), Clamp(ray.Direction.Y), Clamp(ray.Direction.Z));
      var hit = scene.Hit(ray, 0, float.PositiveInfinity);
      if (hit != null) {
        return new Vec3(Math.Abs(hit.N.X), Math.Abs(hit.N.Y), Math.Abs(hit.N.Z));
        //hit = (hit + new Vec3(1, 1, 1)) / 2;
        //return new Vec3(hit.X, hit.Y, hit.Z);
      }

      return MathR.Lerp(MathR.Clamp(ray.Direction.Y), new Vec3(0.5F, 0.7F, 1.0F), new Vec3(1, 1, 1));
    }

    public HitableList GenerateScene() {
      var scene = new HitableList();
      scene.Add(new Sphere(new Vec3(10, 0, -20), 5));
      scene.Add(new Sphere(new Vec3(0, 0, -30), 2));
      var rnd = new Random();
      for (int i = 0; i < 20; i++) {
        scene.Add(new Sphere(new Vec3(GetRan(), GetRan(), GetRan()), random.Next(2, 50)));
      }

      return scene;
    }

    public float GetRan(float minimum = -400, float maximum = 400) {
      return Convert.ToSingle(random.NextDouble() * (maximum - minimum) + minimum);
    }

    #endregion Public Methods

    #region Private Fields

    private static Random random = new Random();

    #endregion Private Fields
  }
}