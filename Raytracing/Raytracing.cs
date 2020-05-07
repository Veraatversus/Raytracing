using System.Drawing;

namespace Raytracing {

  public class Raytracing2 {

    #region Public Methods

    public void Calculate() {
      var width = 200;
      var height = 200;

      var image = new Color[width * height];

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
          var xCoord = lowerLeftCorner.X + horizontal.X / width;
          var yCoord = lowerLeftCorner.Y + vertical.Y / height;
          var zCoord = virtDist;

          var ray = new Ray(origin, new Vec3(xCoord, yCoord, zCoord));

          var color = IntersectScene(ray);

          image[i] = color;
          //image[i] = Color.FromArgb(Convert.ToInt32(x / (double)width * 255), Convert.ToInt32(y / (double)height * 255), 0);
          i++;
        }
      }
      PPM_Writer.Write(image, width, height, "a.ppm");
    }

    #endregion Public Methods
  }
}