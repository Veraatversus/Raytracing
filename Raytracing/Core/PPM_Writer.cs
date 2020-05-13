using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Raytracing {

  public static class PPM_Writer {

    #region Public Methods

    public static void Write(IList<Vec3> image, int width, int height, string path, bool reverse = true) {
      var builder = new StringBuilder();
      builder.AppendLine("P3");
      builder.Append(width).Append(' ').Append(height).Append(' ').Append(255).AppendLine();
      for (var y = 0; y < height; y++) {
        for (var x = 0; x < width; x++) {
          var index = reverse ? (x + ((height - (y + 1)) * width)) : ((width * y) + x);
          var color = image[index];
          builder.Append((int)(color.R * 255)).Append(' ').Append((int)(color.G * 255)).Append(' ').Append((int)(color.B * 255)).AppendLine();
        }
      }

      File.WriteAllText(path, builder.ToString(), Encoding.ASCII);
    }

    public static Bitmap ReadBitmapFromPPM(string file) {
      using (var reader = new BinaryReader(new FileStream(file, FileMode.Open))) {
        if (reader.ReadChar() != 'P' || reader.ReadChar() != '6')
          return null;
        reader.ReadChar(); //Eat newline
        var widths = "";
        var heights = "";
        char temp;
        var builder = new StringBuilder();
        builder.Append(widths);
        while ((temp = reader.ReadChar()) != ' ')
          builder.Append(temp);

        widths = builder.ToString();
        var builder1 = new StringBuilder();
        builder1.Append(heights);
        while ((temp = reader.ReadChar()) >= '0' && temp <= '9')
          builder1.Append(temp);

        heights = builder1.ToString();
        if (reader.ReadChar() != '2' || reader.ReadChar() != '5' || reader.ReadChar() != '5')
          return null;

        reader.ReadChar();
        var width = int.Parse(widths);
        var height = int.Parse(heights);
        var bitmap = new Bitmap(width, height);

        for (int y = 0; y < height; y++)
          for (int x = 0; x < width; x++)
            bitmap.SetPixel(x, y, Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte()));
        return bitmap;
      }
    }
  }

  #endregion Public Methods
}