using System;

namespace Raytracing {

  public static class MathR {

    #region Public Methods

    public static Vec3 Lerp(this Vec3 a, Vec3 b, float t) => (a * (1 - t)) + (b * t);

    public static Vec3 Cross(this Vec3 self, Vec3 other) {
      return new Vec3(
                  (self.Y * other.Z) - (self.Z * other.Y),
                  (self.Z * other.X) - (self.X * other.Z),
                  (self.X * other.Y) - (self.Y * other.X));
    }

    public static float Dot(this Vec3 self, Vec3 other) => (self.X * other.X) + (self.Y * other.Y) + (self.Z * other.Z);

    public static Vec3 Normalize(this Vec3 self) => self / self.Lenght();

    public static float Lenght(this Vec3 self) => MathF.Sqrt(self.SquareLenght());

    public static float SquareLenght(this Vec3 self) => (self.X * self.X) + (self.Y * self.Y) + (self.Z * self.Z);

    public static Vec3 Reflect(this Vec3 self, Vec3 other) => self - (other * Dot(self, other) * 2);

    public static Vec3 Refract(this Vec3 uv, Vec3 n, float etaiOverEtat) {
      var cosTheta = Math.Min(-MathR.Dot(uv, n), 1);
      var rOutParallel = (uv + n * cosTheta) * etaiOverEtat;
      var rOutPerpendicular = n * -MathF.Sqrt(1 - Math.Min(rOutParallel.SquareLenght(), 1));
      return rOutPerpendicular + rOutParallel;
    }

    public static float Clamp(this float value, float min = 0F, float max = 1F) => Math.Max(Math.Min(value, max), min);

    public static float Uniform(this float min, float max) {
      lock (randomLock)
        return ((float)random.NextDouble() * (max - min)) + min;
    }

    public static Vec3 RandomPointInSphere(float min = -1, float max = 1, float squareLenght = 1) {
      while (true) {
        var p = new Vec3(Uniform(min, max), Uniform(min, max), Uniform(min, max));
        if (p.SquareLenght() > squareLenght)
          continue;
        return p;
      }
    }

    #endregion Public Methods

    #region Private Fields

    private static readonly object randomLock = new object();
    private static readonly Random random = new Random();

    #endregion Private Fields
  }
}