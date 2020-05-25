using System;
using System.Threading.Tasks;

namespace Raytracing {

  internal class Programm {

    #region Public Methods

    static public void Main(string[] _) {
      var cv = new CoreValues();
      var dataflow = false;
      Console.WriteLine($"Width ({cv.Width}):");
      if (int.TryParse(Console.ReadLine(), out var width)) {
        cv.Width = width;
      }

      Console.WriteLine($"Samplecount ({cv.SamplesPerPixel}):");
      if (int.TryParse(Console.ReadLine(), out var samplecount)) {
        cv.SamplesPerPixel = samplecount;
      }

      Console.WriteLine($"maxDepth ({cv.MaxDepth}):");
      if (int.TryParse(Console.ReadLine(), out var maxDepth)) {
        cv.MaxDepth = maxDepth;
      }

      Console.WriteLine($"DistToFocus ({cv.DistToFocus}):");
      if (float.TryParse(Console.ReadLine(), out var distToFocus)) {
        cv.DistToFocus = distToFocus;
      }

      Console.WriteLine($"FovY ({cv.FovY}):");
      if (float.TryParse(Console.ReadLine(), out var fovY)) {
        cv.FovY = fovY;
      }

      Console.WriteLine($"Dataflow? ({dataflow}):");
      if (bool.TryParse(Console.ReadLine(), out var flow)) {
        dataflow = flow;
      }
      Task.Factory.StartNew(creationOptions: TaskCreationOptions.LongRunning, action: () => {
        var start = DateTime.Now;
        Console.WriteLine("Started.....");
        if (dataflow) {
          new RaytraceDataflow().Start(cv);
        }
        else {
          Raytrace.Calculate(cv);
        }

        var elapsed = DateTime.Now - start;
        Console.WriteLine($"Process Took: {elapsed:dd\\.hh\\:mm\\:ss} days");
      });
      Console.ReadLine();
    }

    #endregion Public Methods
  }
}