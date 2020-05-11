namespace Raytracing {

  public class HitRecord {

    #region Public Properties

    public float T { get; }

    public Vec3 P { get; }

    public Vec3 N { get; }

    #endregion Public Properties

    #region Public Constructors

    public HitRecord(float t, Vec3 p, Vec3 n) : this() {
      (T, P, N) = (t, p, n);
    }

    public HitRecord() {
    }

    #endregion Public Constructors
  }
}