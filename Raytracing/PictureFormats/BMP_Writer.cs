using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;

namespace Raytracing {

  public static class BMP_Writer {

    #region Public Methods

    public static bool Save(string filename, int w, int h, byte[] data) {
      const byte iComponentCount = 3;

      using var outStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
      if (!outStream.CanWrite)
        return false;

      using var binaryWriter = new BinaryWriter(outStream);

      // write BMP-Header
      binaryWriter.Write("BM".AsSpan()); // all BMP-Files start with "BM"
      var header = new uint[3];
      var rowPad = 4 - (w * 8 * iComponentCount % 32 / 8);
      if (rowPad == 4)
        rowPad = 0;
      header[0] = (uint)(54 + (w * h * iComponentCount) + (rowPad * h));  // filesize = 54 (header) + sizeX * sizeY * numChannels
      header[1] = 0;                // reserved = 0 (4 Bytes)
      header[2] = 54;               // File offset to Raster Data
      binaryWriter.Write(MemoryMarshal.AsBytes(header.AsSpan()));
      // write BMP-Info-Header
      var infoHeader = new uint[10];
      infoHeader[0] = 40;           // size of info header
      infoHeader[1] = (uint)w;      // Bitmap Width
      infoHeader[2] = (uint)h;      //uint32_t(-(int32_t)h); // Bitmap Height (negative to flip image)
      infoHeader[3] = (uint)(1 + (65536 * 8 * iComponentCount));
      // first 2 bytes=Number of Planes (=1) next 2 bytes=BPP
      infoHeader[4] = 0;            // compression (0 = none)
      infoHeader[5] = 0;            // compressed file size (0 if no compression)
      infoHeader[6] = 11810;        // horizontal resolution: Pixels/meter (11810 = 300 dpi)
      infoHeader[7] = 11810;        // vertical resolution: Pixels/meter (11810 = 300 dpi)
      infoHeader[8] = 0;            // Number of actually used colors
      infoHeader[9] = 0;            // Number of important colors  0 = all
      binaryWriter.Write(MemoryMarshal.AsBytes(infoHeader.AsSpan()));

      // data in BMP is stored BGR, so convert scalar BGR
      var pData = new byte[iComponentCount * w * h];

      uint i = 0;
      for (uint y = 0; y < h; ++y) {
        for (uint x = 0; x < w; ++x) {
          var r = data[(4 * (x + (y * w))) + 0];
          var g = data[(4 * (x + (y * w))) + 1];
          var b = data[(4 * (x + (y * w))) + 2];
          var a = data[(4 * (x + (y * w))) + 3];

          pData[i++] = b;
          pData[i++] = g;
          pData[i++] = r;
          if (iComponentCount == 4)
            pData[i++] = a;
        }
      }

      // write data (pad if necessary)
      if (rowPad == 0) {
        binaryWriter.Write(pData);
      }
      else {
        var zeroes = new[] { byte.MinValue, byte.MinValue, byte.MinValue,
                               byte.MinValue, byte.MinValue, byte.MinValue,
                               byte.MinValue, byte.MinValue, byte.MinValue };
        for (var l = 0; l < h; l++) {
          var seq = new ReadOnlySequence<byte>(pData).Slice(iComponentCount * l * w, iComponentCount * w);
          foreach (var item in seq) {
            binaryWriter.Write(item.Span);
          }
          binaryWriter.Write(zeroes.AsSpan()[..rowPad]);
        }
      }
      return true;
    }

    #endregion Public Methods
  }
}