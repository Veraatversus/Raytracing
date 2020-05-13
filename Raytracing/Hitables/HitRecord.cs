namespace Raytracing {

  public class HitRecord {

    #region Public Properties

    public float T { get; }

    public Vec3 P { get; }

    public Vec3 N { get; }
    public IMaterial Material { get; }

    #endregion Public Properties

    #region Public Constructors

    public HitRecord(float t, Vec3 p, Vec3 n, IMaterial material) : this() {
      (T, P, N, Material) = (t, p, n, material);
    }

    public HitRecord() {
    }

    #endregion Public Constructors
  }
}