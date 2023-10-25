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
    public class KinematicsMonitorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public decimal ZaberX
        {
            get { return this._zx; }
            set { this._zx = value; NotifyPropertyChanged(); }
        }
        public decimal ZaberY
        {
            get { return _zy; }
            set { _zy = value; NotifyPropertyChanged(); }
        }
        public decimal ZaberZ 
        { 
            get { return _zz; } 
            set { _zz = value; NotifyPropertyChanged(); }
        }
        public decimal MLSX
        {
            get { return _mlsx; }
            set { _mlsx = value; NotifyPropertyChanged(); }
        }
        public decimal MLSY
        {
            get { return _mlsy; }
            set { _mlsy = value; NotifyPropertyChanged(); }
        }
        public decimal T3RTheta
        {
            get { return _t3t; }
            set { _t3t = value; NotifyPropertyChanged(); }
        }

        private decimal _zx;
        private decimal _zy;
        private decimal _zz;
        private decimal _mlsx;
        private decimal _mlsy;
        private decimal _t3t;
        public ObservableCollection<TransferPoint> ZaberTransferPoints { get; set; }
        public KinematicsMonitorViewModel()
        {
            this.ZaberTransferPoints = new ObservableCollection<TransferPoint>();
            this.ZaberTransferPoints.Clear();
            this.ZaberTransferPoints.Add(new TransferPoint()
            {
                PointDescription = "Test Transfer Point",
                XPosition = 100.0m,
                YPosition = 250.0m,
                ZPosition = 300.0m
            });
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
