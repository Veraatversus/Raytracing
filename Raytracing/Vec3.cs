using System;

namespace Raytracing {

  public static class Vec3Ext {

    #region Public Methods

    public static Vec3 Cross(this Vec3 self, Vec3 other) {
      return new Vec3(
                  self.Y * other.Z - self.Z * other.Y,
                  self.Z * other.X - self.X * other.Z,
                  self.X * other.Y - self.Y * other.X);
    }

    public static double Dot(this Vec3 self, Vec3 other) => (self.X * other.X) + (self.Y * other.Y) + (self.Z * other.Z);

    public static Vec3 Normalize(this Vec3 self) {
      var length = self.Lenght();
      return new Vec3(self.X / length, self.Y / length, self.Z / length);
    }

    public static double Lenght(this Vec3 self) => Math.Sqrt(self.SquareLenght());

    public static double SquareLenght(this Vec3 self) => (self.X * self.X) + (self.Y * self.Y) + (self.Z * self.Z);

    #endregion Public Methods
  }

  public class Vec3 {

    #region Public Properties

    public double[] Data { get; set; } = new double[3];

    public double X { get => Data[0]; set => Data[0] = value; }

    public double Y { get => Data[1]; set => Data[1] = value; }

    public double Z { get => Data[2]; set => Data[2] = value; }

    public double R { get => Data[0]; set => Data[0] = value; }

    public double G { get => Data[1]; set => Data[1] = value; }

    public double B { get => Data[2]; set => Data[2] = value; }

    #endregion Public Properties

    #region Public Constructors

    public Vec3() {
    }

    public Vec3(double x, double y, double z) {
      (X, Y, Z) = (x, y, z);
    }

    #endregion Public Constructors

    #region Public Methods

    public static Vec3 operator +(Vec3 self, Vec3 other)
      => new Vec3 { X = self.X + other.X, Y = self.Y + other.Y, Z = self.Z + other.Z };

    public static Vec3 operator -(Vec3 self, Vec3 other)
     => new Vec3 { X = self.X - other.X, Y = self.Y - other.Y, Z = self.Z - other.Z };

    public static Vec3 operator *(Vec3 self, Vec3 other)
     => new Vec3 { X = self.X * other.X, Y = self.Y * other.Y, Z = self.Z * other.Z };

    public static Vec3 operator /(Vec3 self, Vec3 other)
     => new Vec3(
              self.X / other.X,
              self.Y / other.Y,
              self.Z / other.Z);

    #endregion Public Methods
  }
}