using System;

namespace Raytracing.Materials {

  public class Dielectric : IMaterial {

    #region Public Properties

    public float RefIndex { get; set; }

    #endregion Public Properties

    #region Public Constructors

    public Dielectric(float refIndex) {
      RefIndex = refIndex;
    }

    #endregion Public Constructors

    #region Public Methods

    public (Vec3 color, Ray ray) Scatter(Ray rIn, HitRecord hitRec) {
      var col = new Vec3(1, 1, 1);
      var nOverNP = hitRec.IsFrontFace ? (1 / RefIndex) : RefIndex;
      var unitDirection = rIn.Direction.Normalize();
      var cosAlpha = Math.Min(MathR.Dot(unitDirection * -1, hitRec.N), 1.0F);
      var sinAlpha = MathF.Sqrt(1.0F - (cosAlpha * cosAlpha));

      if (nOverNP * sinAlpha > 1) {
        var reflect = MathR.Reflect(unitDirection, hitRec.N);
        var scatterRay = new Ray(hitRec.P, reflect);
        return (col, scatterRay);
      }

      //Reflection
      var reflectionProp = MathR.Schlick(cosAlpha, RefIndex);
      if (MathR.Uniform(0, 1) < reflectionProp) {
        var reflect = MathR.Reflect(unitDirection, hitRec.N);
        var scatterRay = new Ray(hitRec.P, reflect);
        return (col, scatterRay);
      }

      //Refraction
      var refract = MathR.Refract(unitDirection, hitRec.N, nOverNP);
      var scatterRay2 = new Ray(hitRec.P, refract);
      return (col, scatterRay2);
    }

    #endregion Public Methods
  }
}