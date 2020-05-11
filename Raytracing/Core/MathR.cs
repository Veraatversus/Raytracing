using System;

namespace Raytracing {

  public static class MathR {

    #region Public Methods

    public static float Clamp(this float value, float min = 0F, float max = 1F) => Math.Max(Math.Min(value, max), min);

    public static Vec3 Lerp(this float t, Vec3 a, Vec3 b) => a * (1 - t) + b * t;

    #endregion Public Methods
  }
}