using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Raytracing {

  public static class PPM_Writer {

    #region Public Methods

    public static void Write(IList<Vec3> image, int width, int height, string path, bool reverse = false) {
      var builder = new StringBuilder();
      builder.AppendLine("P3");
      builder.AppendLine($"{width} {height} {255}");
      for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
          var index = reverse ? (x + (height - (y + 1)) * width) : (width * y + x);
          //var index = x + (height - (y + 1)) * width;
          //var index = width * y + x;
          var color = image[index];
          builder.AppendLine($"{color.R * 255} {color.G * 255} {color.B * 255}");
        }
      }

      File.WriteAllText(path, builder.ToString(), Encoding.ASCII);
    }

    #endregion Public Methods
  }
}