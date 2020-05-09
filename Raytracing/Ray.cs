namespace Raytracing {

  public class Ray {

    #region Public Properties

    public Vec3 Origin { get; set; }
    public Vec3 Direction { get; set; }

    #endregion Public Properties

    #region Public Constructors

    public Ray(Vec3 origin, Vec3 direction) {
      this.Origin = origin;
      this.Direction = direction;
    }

    public Ray() {
    }

    #endregion Public Constructors

    #region Public Methods

    public Vec3 PointAtParameter(Ray self, Vec3 t) => self.Origin + self.Direction * t;

    public override string ToString() => $"{{{Origin}}} + {{{Direction}}} * t";

    #endregion Public Methods
  }
}