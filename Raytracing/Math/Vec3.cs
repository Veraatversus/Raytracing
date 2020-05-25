﻿namespace Raytracing {

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

    public Vec3(float x, float y, float z) : this() {
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

    public static Vec3 Random() => new Vec3(Rand.Rand01(), Rand.Rand01(), Rand.Rand01());

    public void Deconstruct(out float r, out float g, out float b) {
      r = R;
      g = G;
      b = B;
    }

    public override string ToString() => $"{X}, {Y}, {Z}";

    #endregion Public Methods
  }
}