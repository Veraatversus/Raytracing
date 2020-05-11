using System;

namespace Raytracing {

  public static class Vec3Math {

    #region Public Methods

    public static Vec3 Cross(this Vec3 self, Vec3 other) {
      return new Vec3(
                  self.Y * other.Z - self.Z * other.Y,
                  self.Z * other.X - self.X * other.Z,
                  self.X * other.Y - self.Y * other.X);
    }

    public static float Dot(this Vec3 self, Vec3 other) => (self.X * other.X) + (self.Y * other.Y) + (self.Z * other.Z);

    public static Vec3 Normalize(this Vec3 self) {
      var length = self.Lenght();
      return new Vec3(self.X / length, self.Y / length, self.Z / length);
    }

    public static float Lenght(this Vec3 self) => MathF.Sqrt(self.SquareLenght());

    public static float SquareLenght(this Vec3 self) => (self.X * self.X) + (self.Y * self.Y) + (self.Z * self.Z);

    #endregion Public Methods
  }

  public class Vec3 {

    #region Public Properties

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public float R { get => X; set => X = value; }

    public float G { get => Y; set => Y = value; }

    public float B { get => Z; set => Z = value; }

    #endregion Public Properties

    #region Public Constructors

    public Vec3() {
    }

    public Vec3(float x, float y, float z) {
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

    public static Vec3 operator *(Vec3 self, float other)
     => new Vec3 { X = self.X * other, Y = self.Y * other, Z = self.Z * other };

    public static Vec3 operator /(Vec3 self, Vec3 other)
     => new Vec3(self.X / other.X, self.Y / other.Y, self.Z / other.Z);

    public static Vec3 operator /(Vec3 self, float other)
     => new Vec3(self.X / other, self.Y / other, self.Z / other);

    public override string ToString() => $"{X}, {Y}, {Z}";

    #endregion Public Methods
  }
}