using System;

namespace Raytracing {

  public class Sphere : IHitable {

    #region Public Properties

    public Vec3 Center { get; }

    public float Radius { get; }
    public IMaterial Material { get; }

    #endregion Public Properties

    #region Public Constructors

    public Sphere() {
    }

    public Sphere(Vec3 center, float radius, IMaterial material) {
      Center = center;
      Radius = radius;
      Material = material;
    }

    #endregion Public Constructors

    #region Public Methods

    public HitRecord Hit(Ray ray, float tMin, float tMax) {
      var oc = ray.Origin - Center;
      var a = ray.Direction.SquareLenght();
      var halfB = ray.Direction.Dot(oc);
      var c = oc.SquareLenght() - (Radius * Radius);

      var discriminante = (halfB * halfB) - (a * c);

      if (discriminante >= 0) {
        var root = MathF.Sqrt(discriminante);
        var t = (-halfB - root) / a;

        if (t < tMax && t > tMin) {
          var intersect = ray.PointAtParameter(t);
          var normal = (intersect - Center) / Radius;
          return new HitRecord(t, intersect, normal, Material);
        }
      }
      return null;
    }

    #endregion Public Methods
  }
}