using System;

namespace Raytracing {

  public class Raytracing2 {

    #region Public Methods

    public static Vec3 GetColor(Ray ray) {
      //return Color.FromArgb((int)Clamp(ray.Direction.X, 0, 1), (int)Clamp(ray.Direction.Y), (int)Clamp(ray.Direction.Z));
      return Lerp(Clamp(ray.Direction.Y), new Vec3(0.5F, 0.7F, 1.0F), new Vec3(1, 1, 1));
    }

    public static double Clamp(double value, double min = 0, double max = 1) => Math.Max(Math.Min(value, max), min);

    public static Vec3 Lerp(double t, Vec3 a, Vec3 b) => a * (1 - t) + b * t;

    public void Calculate() {
      var width = 256;
      var height = 256;

      var image = new Vec3[width * height];

      var origin = new Vec3();
      //var direction = new Vec3(0, 0, -1);
      var virtDist = -1;
      var virtWidth = 4;
      var virtHeight = 2;

      var lowerLeftCorner = new Vec3(-virtWidth / 2, -virtHeight / 2, virtDist);
      var horizontal = new Vec3(virtWidth, 0, 0);
      var vertical = new Vec3(0, virtHeight, 0);

      var i = 0;
      for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
          var xCoord = lowerLeftCorner.X + x * horizontal.X / width;
          var yCoord = lowerLeftCorner.Y + y * vertical.Y / height;
          var zCoord = virtDist;

          var primary = new Ray(origin, new Vec3(xCoord, yCoord, zCoord));
          image[i] = GetColor(primary);
          //image[i] = new Vec3(Convert.ToInt32(primary.Direction.X / (double)width * 255), Convert.ToInt32(primary.Direction.Y / (double)height * 255), 0);

          //var color = IntersectScene(ray);

          //Debug.WriteLine(primary);
          i++;
        }
      }
      PPM_Writer.Write(image, width, height, "a.ppm");
    }

    #endregion Public Methods
  }
}