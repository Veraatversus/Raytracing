using System.Windows;

namespace Raytracing {

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {

    #region Public Constructors

    public MainWindow() {
      InitializeComponent();
      Loaded += MainWindow_Loaded;
    }

    #endregion Public Constructors
    #region Private Methods

    private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
      Raytrace.Calculate();
    }

    #endregion Private Methods
  }
}