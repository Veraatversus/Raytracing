using System.Collections.Generic;

namespace Raytracing {

  public class HitableList : List<IHitable> {

    #region Public Properties

    public List<IHitable> Objects => this;

    #endregion Public Properties

    #region Public Methods

    public HitRecord GetNearestHit(Ray ray, float tMin = 0F, float tMax = float.PositiveInfinity) {
      HitRecord hit = null;
      foreach (var item in this) {
        var currentHit = item.Hit(ray, tMin, tMax);
        if (currentHit != null) {
          tMax = currentHit.T;
          hit = currentHit;
        }
      }
      return hit;
    }

    #endregion Public Methods
  }
}