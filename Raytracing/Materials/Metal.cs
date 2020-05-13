﻿namespace Raytracing {

  public class Metal : IMaterial {

    public Metal(Vec3 color, float fuzz) {
      Color = color;
      Fuzz = fuzz;
    }

    public Vec3 Color { get; }
    public float Fuzz { get; }

    public (Vec3 color, Ray ray) Scatter(Ray rIn, HitRecord hitRec) {
      var reflect = rIn.Direction.Reflect(hitRec.N);
      var scatteredDirection = reflect + (MathR.RandomPointInSphere() * Fuzz);

      if (scatteredDirection.Dot(hitRec.N) > 0) {
        var scatterRay = new Ray(hitRec.P, scatteredDirection);
        return (Color, scatterRay);
      }
      return default;
    }
  }
}