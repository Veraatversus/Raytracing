namespace Raytracing {

  public interface IMaterial {

    #region Public Methods

    (Vec3 color, Ray ray) Scatter(Ray rIn, HitRecord hitRec);

    #endregion Public Methods
  }
}