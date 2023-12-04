#define MLSTestSection
#define ZaberTestSection
#undef MLSTestSection
using System.Diagnostics;
using System.Windows;

namespace scanengine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if MLSTestSection
            MLSController TestController = new MLSController();
            TestController.SerialNumber = "73990090";
            TestController.RequestedXVelocity = 200.0m;
            TestController.RequestedYVelocity = 200.0m;
            TestController.RequestedXAcceleration = 1800.0m;
            TestController.RequestedYAcceleration = 1800.0m;

            TestController.ToggleConnection();
            Thread.Sleep(1000);
            TestController.HomeStage();
            Thread.Sleep(1000);
            TestController.MoveToPosition(10.0m, 10.0m);
            Thread.Sleep(1000);
            TestController.MoveToPosition(70.0m, 45.0m);
#endif
#if ZaberTestSection
            ZaberController TestController = new ZaberController();
            TestController.COMPort = "COM7";
            Debug.WriteLine("Running Zaber Test");
            Debug.WriteLine("COM Port has been set to " +
                TestController.COMPort);
            Debug.WriteLine("RUNNING TOGGLECONNECTION TEST! ");
            TestController.ToggleConnection();
            Debug.WriteLine("Closing Connection...");
            TestController.ToggleConnection();
#endif      

        }
    }
}