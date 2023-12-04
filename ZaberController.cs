using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using Zaber.Motion.Ascii;
using Device = Zaber.Motion.Ascii.Device;

namespace scanengine
{
    internal class ZaberController
    {
        public string COMPort { get; set; }
        public bool IsConnected { get; private set; }
        public bool IsHomed { get; private set; }
        public bool IsError { get; private set; }

        public decimal RequestedXVelocity { get; set; }
        public decimal RequestedYVelocity { get; set; }
        public decimal RequestedZVelocity { get; set; }
        public decimal RequestedXAcceleration { get; set; }
        public decimal RequestedYAcceleration { get; set; }
        public decimal RequestedZAcceleration { get; set; }
        public decimal XPosition { get; private set; }
        public decimal YPosition { get; private set; }
        public decimal ZPosition { get; private set; }


        public bool[]? PrimaryOutputs { get; set; }
        public bool[]? SecondaryOutputs { get; set; }
        public bool[]? PrimaryInputs { get; private set; }
        public bool[]? SecondaryInputs { get; private set; }

        public event EventHandler? ConnectionEvent;
        public event EventHandler? ErrorEvent;

        private Connection ZaberConnection;
        private Device PrimaryController;
        private Device SecondaryController;
        private Axis XTransferAxis;
        private Axis YTransferAxis;
        private Axis ZTransferAxis;

        public ZaberController()
        {
            this.PrimaryOutputs = [false, false, false, false];
            this.SecondaryOutputs = [false, false];
            this.PrimaryInputs = [false, false, false, false];
            this.SecondaryInputs = [false, false];

            return;
        }

        public void ToggleConnection()
        {
            if(this.IsConnected)
            {
                this.ZaberConnection.Close();
                this.IsConnected = false;
                return;
            } else
            {
                if(this.COMPort != null)
                { 
                    this.ZaberConnection = Connection.OpenSerialPort(
                                           this.COMPort);
                    Debug.WriteLine("Serial connection sucessfully " +
                        "opened on " + this.COMPort);
                    this.PrimaryController = this.ZaberConnection.GetDevice(1);
                    this.SecondaryController = this.ZaberConnection.GetDevice(2);
                    Debug.WriteLine("Controller devices have been found. Primary/Secondary");
                    this.PrimaryController.Identify();
                    this.SecondaryController.Identify();
                    Debug.WriteLine("Devices connected to controllers " +
                        "have been identified and units are available.");

                    this.PrimaryController.IO.SetAllDigitalOutputs(
                        this.PrimaryOutputs);
                    this.SecondaryController.IO.SetAllDigitalOutputs(
                        this.SecondaryOutputs);
                    Debug.WriteLine("All controller outputs set to 0.");

                    this.XTransferAxis = this.PrimaryController.GetAxis(1);
                    Debug.WriteLine("XT Axis: " + this.XTransferAxis.Identity);
                    this.YTransferAxis = this.PrimaryController.GetAxis(2);
                    Debug.WriteLine("YT Axis:" + this.YTransferAxis.Identity);
                    this.ZTransferAxis = this.PrimaryController.GetAxis(1);
                    Debug.WriteLine("ZT Axis: " + this.ZTransferAxis.Identity);

                    this.HomeAllAxes();
                    Debug.WriteLine("Connection setup is finished and axes " +
                        "have been homed sucessfully.");

                    return;
                }
                else
                {
                    Debug.WriteLine("COMPort property is null. Cannot" +
                        " connect to the void. Check code/configuration");
                }
            }
            return;
        }
        private void HomeAllAxes()
        {
            // Start with homing Z, then Y then X to minimize collision risk.
            Debug.WriteLine("Homing Z transfer axis....");
            this.ZTransferAxis.Home();
            Debug.WriteLine("Homing Y transfer axis....");
            this.YTransferAxis.Home();
            Debug.WriteLine("Homing Z transfer axis....");
            this.XTransferAxis.Home();
            Debug.WriteLine("All axes have been homed...");
        }
        public void MoveToPosition(int _axisNumber, decimal _requestedPosition)
        {
            return;
        }   
    }
}
