namespace Raytracing.Core {

  public class Lambertain : IMaterial {

    public Lambertain(Vec3 color) {
      Color = color;
    }

    public Vec3 Color { get; set; }

    public (Vec3 color, Ray ray) Scatter(Ray rIn, HitRecord hitRec) {
      var scatteredDirection = hitRec.N + MathR.RandomPointInSphere();
      var scatterRay = new Ray(hitRec.P, scatteredDirection);
      return (Color, scatterRay);
    }
  }
}