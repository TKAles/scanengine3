﻿using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace scanengine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinematicsMonitor KinematicsWindow = new KinematicsMonitor();
        LaserStatus LaserWindow = new LaserStatus();
        T3RStatus T3RWindow = new T3RStatus();
        ScanSpecificationJSON CurrentScanSpec;
        // New Monolithic View Model to hopefully
        // make life easier ?? 

        MonoVM ApplicationVM = new MonoVM();
        
        public MainWindow()
        {
            InitializeComponent();
            // Initialize the other windows
            this.DataContext = this.ApplicationVM;
            this.KinematicsWindow.DataContext = this.ApplicationVM;
            this.LaserWindow.DataContext = this.ApplicationVM;
            this.T3RWindow.DataContext = this.ApplicationVM;
        }

        private void LaserStatus_Click(object sender, RoutedEventArgs e)
        {
            if(this.LaserWindow.IsVisible == false)
            {
                this.LaserWindow.Show();
            } else
            {
                this.LaserWindow.Hide();
            }
        }

        private void KinematicsWindow_Click(object sender, RoutedEventArgs e)
        {
            if(this.KinematicsWindow.IsVisible == false)
            {
                this.KinematicsWindow.Show();
            } else
            {
                this.KinematicsWindow.Hide();
            }
        }

        private void LoadScanJSON_Click(object sender, RoutedEventArgs e)
        {
            // Present a file open dialog to the user and load the JSON
            // file that they select into a ScanSpecificationJSON Object.
            // Update the viewmodel with the relevant details to indicate that
            // the file is loaded and valid.
            OpenFileDialog jsonFileDialog = new OpenFileDialog()
            {
                Filter = "JSON Scan Files (.json) | *.json",
                Title = "Load Scan Parameters"
            };

            if (jsonFileDialog.ShowDialog() == true)
            {
                string _scanFile = jsonFileDialog.FileName;
                Debug.WriteLine("{0} was selected.", _scanFile);
                DetailGrid.IsEnabled = true;
                using(StreamReader  reader = new StreamReader(_scanFile))
                {
                    string _contents = reader.ReadToEnd();
                    this.CurrentScanSpec = new ScanSpecificationJSON();
                    this.CurrentScanSpec = JsonSerializer.Deserialize<ScanSpecificationJSON>(_contents);
                }
                // Check scanengine version, if not for v3 reject.
                if(this.CurrentScanSpec.scanEngine != 3)
                {
                    MessageBox.Show("This is not a version 3 JSON file. Please translate the file into v3 format.",
                                    "Not a V3 File!", 
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    this.DetailGrid.IsEnabled = false;
                    return;
                    
                }
                // Parse information from the JSON file into the viewmodel.
                ApplicationVM.mwStartPosition = string.Format("{0:00.000}mm, {1:00.000}mm", this.CurrentScanSpec.ScanOriginX,
                    this.CurrentScanSpec.ScanOriginY);
                ApplicationVM.mwScanSize = string.Format("{0:00.000}mm x {1:00.000}mm",
                    this.CurrentScanSpec.ScanDeltaX, this.CurrentScanSpec.ScanDeltaY);
                ApplicationVM.mwStepDistance = string.Format("{0:0.000}mm", this.CurrentScanSpec.RowStepSize);
                ApplicationVM.mwPixelSize = string.Format("{0:0.000}mm x {1:0.000}mm per px",
                    (this.CurrentScanSpec.ScanVelocity /
                    (this.CurrentScanSpec.HeliosFrequency / this.CurrentScanSpec.DividerValue)), this.CurrentScanSpec.RowStepSize);
                ApplicationVM.mwAnglesInScan = string.Format("{0} angles", this.CurrentScanSpec.AnglesInScan);
                ApplicationVM.mwDetectionPower = string.Format("{0:00.0}mW", this.CurrentScanSpec.DetectionPower);
                ApplicationVM.mwScanSpeed = string.Format("{0:00.0}mm/s", this.CurrentScanSpec.ScanVelocity);
                ApplicationVM.mwRobometMode = this.CurrentScanSpec.IsRobometCampaign ? "Active" : "Standalone Mode";
                ApplicationVM.mwRobometLayers = string.Format("{0:0} Layers", this.CurrentScanSpec.RobometLayers);
                
                /*
                this.ScanDescriptionVM.ScanLoaded = true;
                // Load relevant display information about the scan into the view
                // model for the main display.
                this.ScanDescriptionVM.XOrigin = this.CurrentScanSpec.ScanOriginX;
                this.ScanDescriptionVM.YOrigin = this.CurrentScanSpec.ScanOriginY;
                this.ScanDescriptionVM.XDelta = this.CurrentScanSpec.ScanDeltaX;
                this.ScanDescriptionVM.YDelta = this.CurrentScanSpec.ScanDeltaY;
                this.ScanDescriptionVM.RowDelta = this.CurrentScanSpec.RowStepSize;
                this.ScanDescriptionVM.THORSerialNumber = this.CurrentScanSpec.THORSerialNumber;
                this.ScanDescriptionVM.HeliosCOMPort = this.CurrentScanSpec.HeliosCOMPort;
                this.ScanDescriptionVM.T3RCOMPort = this.CurrentScanSpec.T3COMPort;
                this.ScanDescriptionVM.ZaberCOMPort = this.CurrentScanSpec.ZaberPort;
                this.ScanDescriptionVM.EffectiveFrequency = (this.CurrentScanSpec.HeliosFrequency /
                                                             this.CurrentScanSpec.DividerValue);
                this.ScanDescriptionVM.ScanVelocity = this.CurrentScanSpec.ScanVelocity;
                this.ScanDescriptionVM.RobometMode = this.CurrentScanSpec.IsRobometCampaign;
                this.ScanDescriptionVM.RobometLayers = this.CurrentScanSpec.RobometLayers;
                this.ScanDescriptionVM.NumAngles = this.CurrentScanSpec.AnglesInScan;
                this.ScanDescriptionVM.NumSteps = (int)this.CurrentScanSpec.StepsPerAngle;
                this.ScanDescriptionVM.DetectionPower = this.CurrentScanSpec.DetectionPower;
                this.ScanDescriptionVM.RobometMode = this.CurrentScanSpec.IsRobometCampaign;
                this.ScanDescriptionVM.RobometLayers = this.CurrentScanSpec.RobometLayers;
                */
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = 5;
            this.Top = 5;
            this.KinematicsWindow.Show();
            this.KinematicsWindow.Top = this.Top;
            this.KinematicsWindow.Left = this.Width + this.Left + 5;
            this.T3RWindow.Top = this.Top + this.Height + 5;
            this.T3RWindow.Left = this.Left + this.LaserWindow.Width + 5;
            this.T3RWindow.Show();
            this.LaserWindow.Top = this.Top + this.Height + 5;
            this.LaserWindow.Left = this.Left;
            this.LaserWindow.Show();
        }
    }
}
