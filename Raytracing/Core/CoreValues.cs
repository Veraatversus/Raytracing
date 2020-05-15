namespace Raytracing {

  public class CoreValues {

    #region Public Properties

    public float AspectRatio { get; set; } = 16F / 9F;
    public int Width { get; set; } = 1920;
    public int Height => (int)(Width / AspectRatio);
    public int maxDepth { get; set; } = 10;
    public int Samplecount { get; set; } = 50;

    public Vec3 Origin { get; set; } = new Vec3(13, 2, 3);
    public Vec3 LookAt { get; set; } = new Vec3(0, 0, 0);
    public Vec3 VecUp { get; set; } = new Vec3(0, 1, 0);
    public float DistToFocus { get; set; } = 10F;
    public float Aperture { get; set; } = 0.1F;
    public float FovY { get; set; } = 20F;

    #endregion Public Properties
  }
}