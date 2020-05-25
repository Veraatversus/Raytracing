namespace Raytracing {

  public class HitRecord {

    #region Public Properties

    public float T { get; }

    public Vec3 P { get; }

    public Vec3 N { get; }
    public Ray R { get; }
    public IMaterial Material { get; }
    public bool IsFrontFace { get; internal set; }

    #endregion Public Properties

    #region Public Constructors

    public HitRecord(float t, Vec3 p, Vec3 n, Ray r, IMaterial material) : this() {
      (T, P, R, N, Material) = (t, p, r, n, material);
      IsFrontFace = MathR.Dot(r.Direction, N) < 0;
      N = IsFrontFace ? n : n * -1;
    }

    public HitRecord() {
    }

    #endregion Public Constructors
  }
}