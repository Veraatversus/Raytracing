namespace Raytracing {

  public interface IHitable {

    #region Public Methods

    HitRecord Hit(Ray ray, float tMin, float tMax);

    #endregion Public Methods
  }
}