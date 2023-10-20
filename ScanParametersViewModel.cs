using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace scanengine
{
    internal class ScanParametersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool ScanLoaded
        {
            get { return _scanLoaded; }
            set { this._scanLoaded = value; this.TriggerMainUIUpdate(); }
        }
        public decimal XOrigin
        {
            get
            {
                return this._xo;
            }
            set
            {
                this._xo = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("OriginDisplay");
            }
        }
        public decimal YOrigin
        {
            get
            {
                return this._yo;
            }
            set
            {
                this._yo = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("OriginDisplay");
            }
        }
        public decimal XDelta
        {
            get { return this._xd; }
            set { this._xd = value; NotifyPropertyChanged(); NotifyPropertyChanged("ScanSizeDisplay"); }
        }
        public decimal YDelta
        {
            get { return this._yd; }
            set { this._yd = value; NotifyPropertyChanged(); NotifyPropertyChanged("ScanSizeDisplay"); }
        }
        public decimal RowDelta
        {
            get { return this._rowDelta; }
            set { this._rowDelta = value; NotifyPropertyChanged(); NotifyPropertyChanged("StepDistanceDisplay"); }
        }
        public int NumAngles
        {
            get { return this._numAngles; }
            set { this._numAngles = value; NotifyPropertyChanged();
                NotifyPropertyChanged("AngleDisplay");
            }
        }
        public int NumSteps
        {
            get { return this._numSteps; }
            set { this._numSteps = value; NotifyPropertyChanged(); 
                NotifyPropertyChanged("AngleDisplay"); }

        }
        public string THORSerialNumber
        {
            get { return this._thorserial; }
            set { this._thorserial = value; NotifyPropertyChanged();  }
        }
        public string HeliosCOMPort
        {
            get { return this._heliosCOM; }
            set { this._heliosCOM = value; NotifyPropertyChanged(); }
        }
        public string T3RCOMPort
        {
            get { return this._T3COM; }
            set { this._T3COM = value; NotifyPropertyChanged(); }   
        }
        public string ZaberCOMPort
        {
            get { return this._ZaberCOM; }
            set { this._ZaberCOM = value; NotifyPropertyChanged(); }
        }
        public decimal EffectiveFrequency
        {
            get { return this._effectiveFreq; }
            set { this._effectiveFreq = value; NotifyPropertyChanged(); NotifyPropertyChanged("PixelSizeDisplay"); }
        }
        public decimal ScanVelocity
        {
            get { return this._scanVelocity; }
            set
            {
                this._scanVelocity = value; NotifyPropertyChanged();
                NotifyPropertyChanged("PixelSizeDisplay"); 
                NotifyPropertyChanged("ScanVelocityDisplay");
            }

        }
        public string OriginDisplay
        {
            get
            {
                if (this.ScanLoaded == false)
                {
                    return "No Scan Loaded";
                } else
                {
                    return String.Format("{0:0.00}mm, {1:0.00mm}", this._xo, this._yo);
                }
            }
        }
        public string StepDistanceDisplay
        {
            get
            {
                if(this.ScanLoaded == false)
                {
                    return "No Scan Loaded";
                } else
                {
                    return String.Format("{0:0.000}mm per scanline", this._rowDelta);
                }
            }
        }
        public string ScanSizeDisplay
        {
            get
            {
                if(this.ScanLoaded == false)
                {
                    return "No Scan Loaded";
                } else
                {
                    return String.Format("{0:0.00}mm, {1:0.00}mm", this._xd, this._yd);
                }
            }
        }
        public string PixelSizeDisplay
        {
            get
            {
                if (!this.ScanLoaded)
                {
                    return "No Scan Loaded";
                } else
                {
                    if(this._effectiveFreq == 0.00m)
                    {
                        return "Not Enough Information.";
                    }
                    return String.Format("{0:0.000}x{1:0.000}mm/px", 
                        (this._scanVelocity / this._effectiveFreq), this._rowDelta);
                }
            }
        }
        public bool RobometMode
        {
            get { return this._robometMode; }
            set { this._robometMode = value; NotifyPropertyChanged(); }
        }
        public int RobometLayers
        {
            get { return this._robometLayers; }
            set { this._robometLayers = value; NotifyPropertyChanged(); }
        }
        public string AngleDisplay
        {
            get { 
                if (!this.ScanLoaded) { return "No Scan Loaded"; }
                return String.Format("{0:0} angles, {1:0} steps/angle",
                    this._numAngles, this._numSteps);
                }
        }
        public string LaserPowerDisplay
        {
            get
            {
                if(!this.ScanLoaded) { return "No Scan Loaded"; }
                return String.Format("{0:0}mW", this._DetPower);
            }
        }
        public string ScanVelocityDisplay
        {
            get
            {
                if(!this.ScanLoaded) { return "No Scan Loaded";  }
                return String.Format("{0:0.0}mm/s", this._scanVelocity);
            }
        }
        public string RobometCampaignDisplay
        {
            get
            {
                if(this.ScanLoaded && (this._robometMode == true))
                {
                    return "Robo-Met.3D Mode Active";
                } else if (this.ScanLoaded && (this._robometMode == false))
                {
                    return "Standalone Mode Active";
                }
                return "No Scan Loaded";
            }
        }

        public decimal DetectionPower
        {
            get { return this._DetPower; }
            set { this._DetPower = value; NotifyPropertyChanged(); NotifyPropertyChanged("LaserPowerDisplay"); }
        }
        private decimal _xo;
        private decimal _yo;
        private decimal _xd;
        private decimal _yd;
        private decimal _rowDelta;
        private decimal _effectiveFreq = 0.00m;
        private decimal _scanVelocity;
        private int _numAngles;
        private int _numSteps;
        private string _thorserial;
        private string _heliosCOM;
        private string _T3COM;
        private string _ZaberCOM;
        private decimal _DetPower;
        private bool _robometMode;
        private int _robometLayers;

        private bool _scanLoaded = false;
        public ScanParametersViewModel() {
            this.ScanLoaded = false;
        }
        private void TriggerMainUIUpdate()
        {
            NotifyPropertyChanged("OriginDisplay");
            NotifyPropertyChanged("StepDistanceDisplay");
            NotifyPropertyChanged("ScanSizeDisplay");
            NotifyPropertyChanged("PixelDistanceDisplay");
            NotifyPropertyChanged("AngleDisplay");
            NotifyPropertyChanged("LaserPowerDisplay");
            NotifyPropertyChanged("ScanVelocityDisplay");
            NotifyPropertyChanged("RobometCampaignDisplay");
            return;
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
