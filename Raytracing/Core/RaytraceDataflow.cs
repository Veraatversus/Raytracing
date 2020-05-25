using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Raytracing {

  public struct InfoStruct {

    #region Public Properties

    public int X { get; set; }

    public int Y { get; set; }

    public int Depth { get; set; }

    public Vec3 Color { get; set; }

    #endregion Public Properties

    #region Public Constructors

    public InfoStruct(CoreValues coreValues) {
      X = 0;
      Y = 0;
      Depth = coreValues.MaxDepth;
      Color = new Vec3(0, 0, 0);
    }

    #endregion Public Constructors
  }

  public class RaytraceDataflow {

    #region Public Properties

    public Camera MyCamera { get; set; }
    public Vec3[] Image { get; set; }
    public HitableList World { get; set; }
    public CoreValues CoreValues { get; set; }
    public long Count { get; set; }
    public TransformManyBlock<int, InfoStruct> PixelFloats { get; private set; }
    public ActionBlock<InfoStruct> MergeColors { get; private set; }
    public ActionBlock<InfoStruct> WriteImage { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public void Start(CoreValues cv) {
      CoreValues = cv;
      MyCamera = new Camera(cv.Origin, cv.LookAt, cv.VecUp, cv.FovY, cv.AspectRatio, cv.Aperture, cv.DistToFocus);
      Image = new Vec3[cv.Width * cv.Height];
      World = HitableList.DefaultWorld;
      Count = cv.Width * cv.Height;

      PixelFloats = new TransformManyBlock<int, InfoStruct>(GetSampleRays);
      MergeColors = new ActionBlock<InfoStruct>(DoMergeColors, new ExecutionDataflowBlockOptions { EnsureOrdered = false, MaxDegreeOfParallelism = -1 });
      //writeImage = new ActionBlock<InfoStruct>(DoWriteImage, new ExecutionDataflowBlockOptions { EnsureOrdered = false, MaxDegreeOfParallelism = -1 });

      var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

      PixelFloats.LinkTo(MergeColors, linkOptions);
      //MergeColors.LinkTo(writeImage, linkOptions);

      PixelFloats.Post(0);

      PixelFloats.Complete();
      MergeColors.Completion.Wait();
      DoSaveImage();
    }

    public Vec3 GetColor(Ray ray, HitableList scene, int depth) {
      if (depth == 0) {
        return new Vec3(0, 0, 0);
      }

      var hit = scene.GetNearestHit(ray, 0.0001F);

      if (hit != null) {
        var result = hit.Material.Scatter(ray, hit);
        if (result != default) {
          return GetColor(result.ray, scene, depth - 1) * result.color;
        }
      }
      var t = (MathR.Normalize(ray.Direction).Y + 1) * 0.5F;
      return (new Vec3(1, 1, 1) * (1.0F - t)) + (new Vec3(0.5F, 0.7F, 1) * t);
    }

    #endregion Public Methods

    #region Private Methods

    private void DoSaveImage() {
      var name = DateTime.Now.ToShortTimeString() + ".ppm";
      PPM_Writer.Write(Image, CoreValues.Width, CoreValues.Height, name);
      Process.Start(Raytrace.gimpPath, Path.Combine(Directory.GetCurrentDirectory(), name));
    }

    //private void DoWriteImage(InfoStruct info) {
    //  Count--;
    //  Image[(info.Y * CoreValues.Width) + info.X] = info.Color;
    //  Console.WriteLine(Count);
    //}

    private void DoMergeColors(InfoStruct info) {
      var samplecount = CoreValues.SamplesPerPixel;
      var c = new Vec3(0, 0, 0);
      var co = new object();
      Parallel.For(0, samplecount, i => {
        //for (var i = 0; i < samplecount; i++) {
        var mycam = MyCamera.GetRay((info.X + Rand.Uniform(0, 0.99F)) / (CoreValues.Width - 1), (info.Y + Rand.Uniform(0, 0.99F)) / (CoreValues.Height - 1));
        var newcolor = GetColor(mycam, World, CoreValues.MaxDepth) / samplecount;
        lock (co) {
          c += newcolor;
        }
      });
      //info.Color = c;
      Image[(info.Y * CoreValues.Width) + info.X] = c;
      Console.WriteLine(Count--);
      //return info;
    }

    private IEnumerable<InfoStruct> GetSampleRays(int _) {
      for (var x = 0; x < CoreValues.Width; x++) {
        for (var y = 0; y < CoreValues.Height; y++) {
          var newinfo = new InfoStruct { X = x, Y = y, Depth = CoreValues.MaxDepth };
          yield return newinfo;
        }
      }
    }

    #endregion Private Methods
  }
}