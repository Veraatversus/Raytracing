using System;

namespace Raytracing {
  internal class Camera {
    private Vec3 Origin { get; set; }
    private Vec3 LookAt { get; set; }
    private Vec3 VUp { get; set; }
    private float FovY { get; set; }
    private float LensRadius { get; set; }
    public float HalfHeight { get; set; }
    public float HalfWidth { get; set; }
    public Vec3 W { get; set; }
    public Vec3 U { get; set; }
    public Vec3 V { get; set; }
    public Vec3 LowerLeftCorner { get; set; }
    public Vec3 Horizontal { get; set; }
    public Vec3 Vertical { get; set; }

    public Camera() {
    }

    public Camera(Vec3 origin, Vec3 lookAt, Vec3 vUp, float fovY, float aspectRatio, float aperture, float distToFocus) {
      LookAt = lookAt;
      Origin = origin;
      VUp = vUp;
      LensRadius = aperture / 2;
      FovY = MathR.ConvertDegreesToRadians(fovY);
      HalfHeight = MathF.Tan(FovY / 2);
      HalfWidth = HalfHeight * aspectRatio;
      W = MathR.Normalize(Origin - LookAt);
      U = MathR.Normalize(MathR.Cross(VUp, W));
      V = MathR.Cross(W, U);
      LowerLeftCorner = Origin - U * HalfWidth * distToFocus - V * HalfWidth * distToFocus - W * distToFocus;
      Horizontal = U * HalfWidth * distToFocus * 2;
      Vertical = V * HalfHeight * distToFocus * 2;
    }

    public Ray GetRay(float s, float t) {
      var rd = MathR.RandomPointInDisc() * LensRadius;
      var offset = U * rd.X + V * rd.Y;
      return new Ray(Origin + offset, LowerLeftCorner + Horizontal *s + Vertical * t - Origin -offset);
      //var temp = MathR.RandomPointInDisc() * LensRadius;
      //var apertureOffset = (U * temp.X) + (V * temp.Y);
      //return new Ray(Origin + apertureOffset, LowerLeftCorner + (U * x * 2 * HalfWidth) + (V * y * 2 * HalfHeight));
    }
  }
}