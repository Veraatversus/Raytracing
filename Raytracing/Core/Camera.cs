using System;

namespace Raytracing {

  internal class Camera {

    #region Public Properties

    public float HalfHeight { get; set; }
    public float HalfWidth { get; set; }
    public Vec3 W { get; set; }
    public Vec3 U { get; set; }
    public Vec3 V { get; set; }
    public Vec3 LowerLeftCorner { get; set; }
    public Vec3 Horizontal { get; set; }
    public Vec3 Vertical { get; set; }
    public Vec3 Origin { get; set; }
    public Vec3 LookAt { get; set; }
    public Vec3 VecUp { get; set; }
    public float FovY { get; set; }
    public float AspectRatio { get; set; }
    public float LensRadius { get; set; }
    public float DistToFocus { get; set; }

    #endregion Public Properties

    #region Public Constructors

    public Camera() {
    }

    public Camera(Vec3 origin, Vec3 lookAt, Vec3 vUp, float fovY, float aspectRatio, float aperture, float distToFocus) : this() {
      Origin = origin;
      LookAt = lookAt;
      VecUp = vUp;
      DistToFocus = distToFocus;
      AspectRatio = aspectRatio;
      FovY = MathR.ConvertDegreesToRadians(fovY);

      LensRadius = aperture / 2;
      HalfHeight = MathF.Tan(FovY / 2) * DistToFocus;
      HalfWidth = HalfHeight * AspectRatio;

      W = MathR.Normalize(LookAt - Origin);
      U = MathR.Normalize(MathR.Cross(W, vUp));

      V = MathR.Cross(U, W);
      LowerLeftCorner = Origin + (W * DistToFocus) - (V * HalfHeight) - (U * HalfWidth);
    }

    #endregion Public Constructors

    #region Public Methods

    public Ray GetRay(float s, float t) {
      var temp = MathR.RandomPointInDisc() * LensRadius;
      var apertureOffset = (U * temp.X) + (V * temp.Y);
      return new Ray(Origin + apertureOffset, LowerLeftCorner + (U * s * 2 * HalfWidth) + (V * t * 2 * HalfHeight) - (Origin + apertureOffset));
    }

    #endregion Public Methods
  }
}