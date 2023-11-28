using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Thorlabs.MotionControl.Benchtop.BrushlessMotorCLI;
using Thorlabs.MotionControl.GenericMotor;
using Zaber.Motion;
using Zaber.Motion.Ascii;
using System.Text.Json;

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
    public class RobometTransferSystem 
        : INotifyPropertyChanged
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
        public Device PrimaryController
        {
            get { return _primaryController; }
        }
        public Device SecondaryController
        {
            get { return _secondaryController; }
        }
        public Axis XAxis
        {
            get { return _xaxis; }
        }
        public Axis YAxis
        {
            get { return _yaxis; }
        }

        public Axis ZAxis
        {
            get { return _zaxis; }
        }
        private Axis _xaxis;
        private Axis _yaxis;
        private Axis _zaxis;
        private Device _primaryController;
        private Device _secondaryController;
        private Connection _serialConnection;

        private MonoVM _viewModel;
        public RobometTransferSystem(ref MonoVM _appViewModel)
        {
            // Upon construction look for a transfer_position.json file
            // and load that into the observable collection, otherwise create
            // a dummy json file.
            if(File.Exists("transfer_positions.json"))
            {
                Debug.WriteLine("ZABER: R3D Transfer Position JSON file found. Loading points into memory.");
            }
            else
            {
                Debug.WriteLine("ZABER: Could not find a Transfer Position JSON file. Creating a blank one.");
            }
            ref MonoVM _viewModel = ref _appViewModel;
        }

        public void ToggleConnection()
        {
            if(IsConnected ==  false)
            {
                _serialConnection = Connection.OpenSerialPort(zaber_com_port);
                IsConnected = true;

                _primaryController = _serialConnection.GetDevice(1);
                _secondaryController = _serialConnection.GetDevice(2);
                
                _xaxis = _primaryController.GetAxis(1);
                _yaxis = _primaryController.GetAxis(2);
                _zaxis = _secondaryController.GetAxis(1);

                _primaryController.Identify();
                _secondaryController.Identify();

                _xaxis.Home();
                _zaxis.Home();
                _yaxis.Home();

            } else
            {
                _serialConnection.Close();
            }
            IsConnected = _serialConnection.IsConnected;
        }

        public void LoadTransferPoints(ref MonoVM _applicationVM)
        {
            if(File.Exists("transfer_positions.json"))
            {
                using(StreamReader _fileReader = new StreamReader("transfer_positions.json"))
                {
                    string _contents = _fileReader.ReadToEnd();
                    transferPoints = JsonSerializer.Deserialize<ObservableCollection<TransferPoint>>(_contents);
                    _applicationVM.kmTransferPoints = transferPoints;
                }
            } else
            {
                // Create a new file with a couple dummy entries in it
                transferPoints = new ObservableCollection<TransferPoint>();
                var _p1 = new TransferPoint();
                _p1.PointDescription = "Please enter";
                var _p2 = new TransferPoint();
                _p2.PointDescription = "point information";
                transferPoints.Add(_p1);
                transferPoints.Add(_p2);
                using (StreamWriter _fileWriter = new StreamWriter("transfer_positions.json"))
                {
                    var _json_out = JsonSerializer.Serialize<ObservableCollection<TransferPoint>>(transferPoints);
                    _fileWriter.Write(_json_out);
                    _fileWriter.Close();
                }
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
