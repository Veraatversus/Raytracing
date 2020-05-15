namespace Raytracing.Core {

  public class Lambertain : IMaterial {

    #region Public Properties

    public Vec3 Color { get; set; }

    #endregion Public Properties

    #region Public Constructors

    public Lambertain(Vec3 color) {
      Color = color;
    }

    #endregion Public Constructors

    #region Public Methods

    public (Vec3 color, Ray ray) Scatter(Ray rIn, HitRecord hitRec) {
      var scatteredDirection = hitRec.N + MathR.RandomPointInSphere();
      var scatterRay = new Ray(hitRec.P, scatteredDirection);
      return (Color, scatterRay);
    }

    #endregion Public Methods
  }
}