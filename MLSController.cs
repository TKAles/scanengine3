using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorlabs.MotionControl.Benchtop.BrushlessMotor;
using Thorlabs.MotionControl.Benchtop.BrushlessMotorCLI;
using Thorlabs.MotionControl.Benchtop.BrushlessMotorCLI.Native;
using Thorlabs.MotionControl.DeviceManagerCLI;
using Thorlabs.MotionControl.GenericMotorCLI.AdvancedMotor;
using Thorlabs.MotionControl.GenericMotorCLI.ControlParameters;

namespace scanengine
{
    internal class MLSController
    {
        public String? SerialNumber { get; set; }
        public decimal XPosition { get; private set; }
        public decimal YPosition { get; private set; }

        public bool IsHomed { get; private set; }
        public bool IsConnected { get; private set; }
        public bool IsError { get; private set; }
        public decimal RequestedXVelocity { get; set; }
        public decimal RequestedYVelocity { get; set; }

        public decimal RequestedXAcceleration { get; set; }
        public decimal RequestedYAcceleration { get; set; }
        public int InitializationTimeout { get; set; } = 10000;
        public int MovementTimeout { get; set; } = 25000;

        public event EventHandler ConnectionEvent; 
        public event EventHandler ErrorEvent;

        private BenchtopBrushlessMotor BBDController;
        private BenchtopBrushlessMotorConfiguration XAxisConfiguration;
        private BenchtopBrushlessMotorConfiguration YAxisConfiguration;
        private BrushlessMotorChannel XAxis;
        private BrushlessMotorChannel YAxis;
        private BrushlessMotorSettings XAxisSettings;
        private BrushlessMotorSettings YAxisSettings;
        private VelocityParameters XAxisVelocityParameters;
        private VelocityParameters YAxisVelocityParameters; 


        public MLSController()
        {
            DeviceManagerCLI.BuildDeviceList();
            if(this.SerialNumber != null)
            {
                this.CreateMLSConnection();
            }
            return;
        }
        /// <summary>
        /// Toggles the connection to the MLS motion controller for the
        /// microscope stage. Also initializes devices and checks that settings
        /// match the requested XAcceleration, XVelocity, etc.
        /// </summary>
        public void ToggleConnection()
        {
            if (!this.IsConnected)
            {
                this.CreateMLSConnection();
            } else {
                this.BBDController.DisconnectTidyUp();
            }

            return;
        }

        public void MoveToPosition(decimal _xcoord, decimal _ycoord)
        {
            if ((this.XAxis == null)||(this.YAxis == null))
            {
                throw new Exception();
            }
            Debug.WriteLine("Moving X axis to {0}mm..", _xcoord);
            this.XAxis.MoveTo(_xcoord, this.MovementTimeout);
            Debug.WriteLine("X axis move complete. Moving Y axis to {0}mm..", _ycoord);
            this.YAxis.MoveTo(_ycoord, this.MovementTimeout);
            Debug.WriteLine("Y axis move complete. Move completed sucessfully.");
            return;
        }

        public void SetTrigger(int _axisno, Thorlabs.MotionControl.GenericMotorCLI.Settings.HardwareTriggeringSettings.TriggerOutputEnable _triggerCondition)
        { 
            if(_axisno == 1)
            {
                this.XAxisSettings.HardwareTriggering.OutputTrigger = _triggerCondition;
                Debug.WriteLine("Modified settings for x-axis.");
                this.XAxis.SetSettings(this.XAxisSettings, true);
                Debug.WriteLine("Pushed new settings to x-axis and set persistence");
                return;
            }
            else if(_axisno == 2)
            {
                this.YAxisSettings.HardwareTriggering.OutputTrigger = _triggerCondition;
                Debug.WriteLine("Modified settings for y-axis.");
                this.YAxis.SetSettings(this.YAxisSettings, true);
                Debug.WriteLine("Pushed new settings to y-axis and set persistence");
                return;
            }
            Debug.WriteLine("Invalid axis number was specified. Ignoring request.");
            return;
        }

        public void HomeStage()
        {
            if ((this.XAxis == null) || (this.YAxis == null))
            {
                Debug.WriteLine("X and/or Y axis are null??");
                return;
            }
            Debug.WriteLine("Homing X Axis...");
            this.XAxis.Home(this.MovementTimeout);
            Debug.WriteLine("Homing Y Axis...");
            this.YAxis.Home(this.MovementTimeout);
            return;
        }

        /// <summary>
        /// Creates the initial connection to the motion control system. 
        /// </summary>
        /// s
        private void CreateMLSConnection()
        {
            // Check serial is set
            if (this.SerialNumber != null) 
            {
                // Initialize controller, grab axes and enable them.
                this.BBDController = BenchtopBrushlessMotor.CreateBenchtopBrushlessMotor(this.SerialNumber);
                this.BBDController.Connect(this.SerialNumber);
                Debug.WriteLine("Connected to " + this.SerialNumber);
                this.BBDController.ConnectionStateChanged += BBDConnectionStateChanged;
                Thread.Sleep(100);
                Debug.WriteLine("Initializing X Axis...");

                this.XAxis = this.BBDController.GetChannel(1);
                if(!this.XAxis.IsSettingsInitialized())
                {
                    this.XAxis.WaitForSettingsInitialized(this.InitializationTimeout);
                }
                Debug.WriteLine("Initializing Y Axis");
                this.YAxis = this.BBDController.GetChannel(2);
                if(!this.YAxis.IsSettingsInitialized())
                {
                    this.YAxis.WaitForSettingsInitialized(this.InitializationTimeout);
                }
                Debug.WriteLine("Enabling Axes...");
                this.XAxis.StartPolling(50);
                this.XAxis.EnableDevice();
                Thread.Sleep(50);
                this.YAxis.StartPolling(50);
                this.YAxis.EnableDevice();
                Thread.Sleep(50);
                // Get the axis motor configurations, motor settings and the velocity parameters
                Debug.WriteLine("Configuring X Axis...");
                this.XAxisConfiguration = this.XAxis.LoadMotorConfiguration(this.XAxis.DeviceID,
                    DeviceConfiguration.DeviceSettingsUseOptionType.UseDeviceSettings) as BenchtopBrushlessMotorConfiguration;
                Debug.WriteLine("Configuring Y Axis...");
                this.YAxisConfiguration = this.YAxis.LoadMotorConfiguration(this.YAxis.DeviceID,
                    DeviceConfiguration.DeviceSettingsUseOptionType.UseDeviceSettings) as BenchtopBrushlessMotorConfiguration;
                this.XAxisSettings = this.XAxis.MotorDeviceSettings as BrushlessMotorSettings;
                this.YAxisSettings = this.YAxis.MotorDeviceSettings as BrushlessMotorSettings;
                this.XAxisVelocityParameters = this.XAxis.GetVelocityParams();
                this.YAxisVelocityParameters = this.YAxis.GetVelocityParams();
                // Check the velocity parameters on the device match the requested
                // acceleration and velocity parameters.
                if(this.XAxisVelocityParameters.MaxVelocity != this.RequestedXVelocity)
                {
                    this.XAxisVelocityParameters.MaxVelocity = this.RequestedXVelocity;
                }
                if(this.XAxisVelocityParameters.Acceleration != this.RequestedXAcceleration)
                {
                    this.XAxisVelocityParameters.Acceleration = this.RequestedXAcceleration;
                }
                if(this.YAxisVelocityParameters.MaxVelocity != this.RequestedYVelocity)
                {
                    this.YAxisVelocityParameters.MaxVelocity = this.RequestedYVelocity;
                }
                if(this.YAxisVelocityParameters.Acceleration != this.RequestedYAcceleration)
                {
                    this.YAxisVelocityParameters.Acceleration = this.RequestedYAcceleration;
                }
                this.XAxis.SetVelocityParams(this.XAxisVelocityParameters);
                Debug.WriteLine("Wrote new X Axis velocity and acceleration profile information to MLS device");
                this.YAxis.SetVelocityParams(this.YAxisVelocityParameters);
                Debug.WriteLine("Wrote new Y Axis velocity and acceleration profile information to MLS device");
            }
            return;
        }

        private void BBDConnectionStateChanged(object sender, ThorlabsConnectionManager.ConnectionStateChangedEventArgs e)
        {
            this.ConnectionEvent(sender, e);
            if(e.ConnectionState == ThorlabsConnectionManager.ConnectionStates.Connected)
            {
                this.IsConnected = true;
            } else if (e.ConnectionState == ThorlabsConnectionManager.ConnectionStates.Disconnected)
            {
                this.IsConnected = false;
            }

            Debug.WriteLine("MLS Connection Event Raised\n{0}", e.ConnectionState.ToString());
        }
    }
}
