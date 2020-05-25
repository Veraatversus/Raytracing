using System.Collections.Generic;
using Raytracing.Core;
using Raytracing.Materials;

namespace Raytracing {

  public class HitableList : List<IHitable> {

    static HitableList() {
      DefaultWorld = new HitableList {
        //Bottom "plane"
        new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertain(new Vec3(0.5F, 0.5F, 0.5F))),

        // three large spheres
	      new Sphere(new Vec3(4, 1, 0), 1.0F, new Dielectric(1.5F)),
        new Sphere(new Vec3(-4, 1, 0), 1.0F, new Lambertain(new Vec3(0.4F, 0.2F, 0.1F))),
        new Sphere(new Vec3(0, 1, 0), 1.0F, new Metal(new Vec3(0.7F, 0.6F, 0.5F), 0.0F))
      };

      // numerous small sphere
      RandomizeWorld(DefaultWorld);
    }

    public static HitableList DefaultWorld { get; set; }

    #region Public Properties

    private static void RandomizeWorld(HitableList world) {
      for (var a = -11; a < 11; a++) {
        for (var b = -11; b < 11; b++) {
          var chooseMat = Rand.Rand01();
          var center = new Vec3(a + (0.9F * Rand.Rand01()), 0.2F, b + (0.9F * Rand.Rand01()));
          if ((center - new Vec3(4, 0.2F, 0)).Lenght() > 0.9F) {
            if (chooseMat < 0.8F) {
              // diffuse
              var albedo = Vec3.Random() * Vec3.Random();

              world.Add(new Sphere(center, 0.2F, new Lambertain(albedo)));
            }
            else if (chooseMat < 0.95) {
              // metal
              var albedo = Rand.Rand01();
              var albedo2 = Rand.Rand01();
              var albedo3 = Rand.Rand01();

              var fuzz = Rand.Rand01();

              world.Add(new Sphere(center, 0.2F, new Metal(new Vec3(albedo, albedo2, albedo3), fuzz)));
            }
            else {
              // glass
              world.Add(new Sphere(center, 0.2F, new Dielectric(Rand.Uniform(0.5F, 1.5F))));
            }
          }
        }
      }
    }

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