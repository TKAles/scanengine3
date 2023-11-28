using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace scanengine
{
    public class MonoVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string mwStartPosition
        {
            get { return _mwStartPos; } 
            set { _mwStartPos = value; NotifyPropertyChanged(); }
        }
        public string mwScanSize
        {
            get { return _mwScanSize; }
            set { _mwScanSize = value; NotifyPropertyChanged(); }
        }
        public string mwStepDistance
        {
            get { return _mwStepDistance; }
            set { _mwStepDistance = value; NotifyPropertyChanged(); }
        }
        public string mwPixelSize
        {
            get { return _mwPixelSize;}
            set { _mwPixelSize = value; NotifyPropertyChanged(); } 
        }
        public string mwAnglesInScan
        {
            get { return _mwAnglesInScan; }
            set { _mwAnglesInScan = value; NotifyPropertyChanged(); }
        }
        public string mwDetectionPower
        {
            get { return _mwDetectionPower; }
            set { _mwDetectionPower = value; NotifyPropertyChanged(); }
        }
        public string mwScanSpeed
        {
            get { return _mwScanSpeed; }
            set { _mwScanSpeed = value; NotifyPropertyChanged(); }
        }
        public string mwRobometMode
        {
            get { return _mwRobometMode; }
            set { _mwRobometMode = value; NotifyPropertyChanged(); }
        }
        public string mwRobometLayers
        {
            get { return _mwRobometLayers; }
            set { _mwRobometLayers = value; NotifyPropertyChanged(); }
        }
        public string mwMLSStatus
        {
            get { return _mwMLSStatus; }
            set { _mwMLSStatus = value; NotifyPropertyChanged(); }
        }
        public string mwZaberStatus
        {
            get { return _mwZaberStatus;}
            set {  _mwZaberStatus = value; NotifyPropertyChanged();}
        }
        public string mwHeliosStatus
        {
            get { return _mwHeliosStatus; }
            set { _mwHeliosStatus = value; NotifyPropertyChanged(); }
        }
        public string mwGenesisStatus
        {
            get { return _mwGenesisStatus; }
            set { _mwGenesisStatus = value; NotifyPropertyChanged(); }
        }
        public string mwT3RStatus
        {
            get { return _mwT3RStatus; }
            set { _mwT3RStatus = value; NotifyPropertyChanged(); }
        }
        public string mwR3DStatus
        {
            get { return _mwR3DStatus; }
            set { _mwR3DStatus = value; NotifyPropertyChanged(); }
        }

        private string _mwStartPos = "¯\\_(ツ)_/¯";
        private string _mwScanSize = "¯\\_(ツ)_/¯";
        private string _mwStepDistance = "¯\\_(ツ)_/¯";
        private string _mwPixelSize = "¯\\_(ツ)_/¯";
        private string _mwAnglesInScan = "¯\\_(ツ)_/¯";
        private string _mwDetectionPower = "¯\\_(ツ)_/¯";
        private string _mwScanSpeed = "¯\\_(ツ)_/¯";
        private string _mwRobometMode = "¯\\_(ツ)_/¯";
        private string _mwRobometLayers = "¯\\_(ツ)_/¯";
        private string _mwMLSStatus = "¯\\_(ツ)_/¯";
        private string _mwZaberStatus = "¯\\_(ツ)_/¯";
        private string _mwHeliosStatus = "¯\\_(ツ)_/¯";
        private string _mwGenesisStatus = "¯\\_(ツ)_/¯";
        private string _mwT3RStatus = "¯\\_(ツ)_/¯";
        private string _mwR3DStatus = "¯\\_(ツ)_/¯";

        public string kmZaberX
        {
            get { return _kmZaberX; }
            set { _kmZaberX = value; NotifyPropertyChanged(); }
        }
        public string kmZaberY
        {
            get { return _kmZaberY; }
            set { _kmZaberY = value; NotifyPropertyChanged(); }
        }
        public string kmZaberZ
        {
            get { return _kmZaberZ; }
            set { _kmZaberZ = value; NotifyPropertyChanged(); }
        }
        public ObservableCollection<TransferPoint> kmTransferPoints
        {
            get { return _kmTransferPoints; }
            set { _kmTransferPoints = value; NotifyPropertyChanged(); }
        }
        public string kmMLSX
        {
            get { return _kmMLSX; }
            set { _kmMLSX = value; NotifyPropertyChanged();}
        }
        public string kmMLSY
        {
            get { return _kmMLSY; }
            set { _kmMLSY = value;  NotifyPropertyChanged(); }
        }
        public string kmT3RTheta
        {
            get { return _kmT3RTheta; }
            set { _kmT3RTheta = value; NotifyPropertyChanged(); }
        }

        private string _kmZaberX;
        private string _kmZaberY;
        private string _kmZaberZ;
        private string _kmMLSX;
        private string _kmMLSY;
        private string _kmT3RTheta;
        private ObservableCollection<TransferPoint> _kmTransferPoints;

        public string lsGenesisInterlock
        {
            get { return _lsGenesisIntlk; }
            set { _lsGenesisIntlk = value; NotifyPropertyChanged(); }
        }
        public string lsGenesisState
        {
            get { return _lsGenesisState; }
            set { _lsGenesisState = value; NotifyPropertyChanged(); }
        }
        public string lsGenesisPower
        {
            get { return _lsGenesisPower; }
            set { _lsGenesisPower = value; NotifyPropertyChanged(); }
        }
        public string lsGenesisTemp
        {
            get { return _lsGenesisTemp; }
            set { _lsGenesisTemp = value; NotifyPropertyChanged(); }
        }
        public string lsGenesisCState
        {
            get { return _lsGenesisCState; }
            set { _lsGenesisCState = value; NotifyPropertyChanged(); }
        }
        public string lsHeliosInterlock
        {
            get { return _lsHeliosIntlk; }
            set { _lsHeliosIntlk = value; NotifyPropertyChanged(); }
        }
        public string lsHeliosState
        {
            get { return _lsHeliosState; }
            set { _lsHeliosState = value; NotifyPropertyChanged(); }
        }
        public string lsHeliosCurrent
        {
            get { return _lsHeliosCurrent; }
            set { _lsHeliosCurrent = value; NotifyPropertyChanged(); }

        }
        public string lsHeliosTemp
        {
            get { return _lsHeliosTemp; }
            set { _lsHeliosTemp = value; NotifyPropertyChanged(); }
        }
        public string lsHeliosCState
        {
            get { return _lsHeliosCState; }
            set { _lsHeliosCState = value; NotifyPropertyChanged(); }  
        }

        private string _lsGenesisIntlk;
        private string _lsGenesisState;
        private string _lsGenesisPower;
        private string _lsGenesisTemp;
        private string _lsGenesisCState;
        private string _lsHeliosIntlk;
        private string _lsHeliosState;
        private string _lsHeliosCurrent;
        private string _lsHeliosTemp;
        private string _lsHeliosCState;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
