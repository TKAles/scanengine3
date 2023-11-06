using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Zaber.Motion;

namespace scanengine
{
    public struct TransferPoint
    {
        public string PointDescription { get; set; }
        public decimal XPosition { get; set; }
        public decimal YPosition { get; set; }
        public decimal ZPosition { get; set; }
        public bool RobometRTS { get; set; }
        public bool RobometRTL { get; set; }
        public bool RobometESTOP { get; set; }
        public bool SRASReady { get; set; }
        public bool SRASCTL { get; set; }
        public bool SRASDone { get; set; }
        public bool SRASError { get; set; }
        public bool GripperActive { get; set; }

    }
    public class RobometTransferSystem : INotifyPropertyChanged
    {
        public ObservableCollection<TransferPoint> transferPoints = new ObservableCollection<TransferPoint>();
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsConnected
        {
            get { return this.is_connected; }
            set { this.is_connected = value; NotifyPropertyChanged(); }
        }
        public bool IsHomed
        {
            get { return this.is_homed; }
            set { this.is_homed = value; NotifyPropertyChanged(); }
        }

        public bool[] InputStates
        {
            get { return this.input_states; }
            set { this.input_states = value; NotifyPropertyChanged(); }
        }
        public bool[] OutputStates
        {
            get { return this.output_states; }
            set { this.output_states = value; NotifyPropertyChanged(); }
        }

        public decimal XPosition
        {
            get { return this.x_pos; }
            set { this.x_pos = value; NotifyPropertyChanged(); }
        }
        public decimal YPosition
        {
            get { return this.y_pos; }
            set { this.y_pos = value; NotifyPropertyChanged(); }
        }

        public decimal ZPosition
        {
            get { return this.z_pos; }
            set { this.z_pos = value; NotifyPropertyChanged(); }
        }
        
        public string ZaberCOMPort
        {
            get { return this.zaber_com_port; }
            set { this.zaber_com_port = value; NotifyPropertyChanged(); }
        }
        private bool is_connected;
        private bool is_homed;
        private bool[] input_states = { false, false, false, false };
        private bool[] output_states = { false, false, false, false, false };
        private decimal x_pos;
        private decimal y_pos;
        private decimal z_pos;
        private string zaber_com_port;

        public RobometTransferSystem()
        {
            
        }

        public void InitializeConnection()
        {

        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
