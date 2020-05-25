using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace Raytracing {

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    public App() {
      AllocConsole();
      Task.Run(() => {
        var cv = new CoreValues {
          Samplecount = 1000,
          maxDepth = 20,
        };
        Raytrace.Calculate(cv);
      });

    }

    [DllImport("Kernel32")]
    private static extern void AllocConsole();
  }
}