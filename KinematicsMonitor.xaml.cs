using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace scanengine
{
    /// <summary>
    /// Interaction logic for KinematicsMonitor.xaml
    /// </summary>
    public partial class KinematicsMonitor : Window
    {
        public RobometTransferSystem RobometTransfer;
        public KinematicsMonitorViewModel KineVM = new();
        
        public KinematicsMonitor()
        {
            InitializeComponent();
             
            this.DataContext = this.KineVM;
            // Subscribe the viewmodel to the transfer system
            // daemon.
            
        }

        private void AddTransferPosition_Click(object sender, RoutedEventArgs e)
        {
            this.KineVM.ZaberTransferPoints.Add(new TransferPoint()
            {
                PointDescription = "<New Point Added>"
            });
        }

        private void RemoveTransferPosition_Click(object sender, RoutedEventArgs e)
        {
            int selected = this.TransferSystemDataGrid.SelectedIndex;
            if(selected  == -1)
            {
                return;
            }
            this.KineVM.ZaberTransferPoints.Remove(
                this.KineVM.ZaberTransferPoints[selected] );
            return;
        }

        private void ForceManualCycle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseKinematicsWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
