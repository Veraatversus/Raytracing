namespace Raytracing {

  public class Metal : IMaterial {

    #region Public Properties

    public Vec3 Color { get; set; }

    public float Fuzz { get; set; }

    #endregion Public Properties

    #region Public Constructors

    public Metal(Vec3 color, float fuzz) {
      Color = color;
      Fuzz = fuzz;
    }

    #endregion Public Constructors

    #region Public Methods

    public (Vec3 color, Ray ray) Scatter(Ray rIn, HitRecord hitRec) {
      var reflect = rIn.Direction.Reflect(hitRec.N);
      var scatteredDirection = reflect + (MathR.RandomPointInSphere() * Fuzz);

      if (scatteredDirection.Dot(hitRec.N) > 0) {
        var scatterRay = new Ray(hitRec.P, scatteredDirection);
        return (Color, scatterRay);
      }
      return default;
    }

    #endregion Public Methods
  }
}