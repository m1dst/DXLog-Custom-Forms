using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DxLogStationMaster
{
    public class Rotator : INotifyPropertyChanged
    {

        private int _actualAzimuth;
        private int? _targetAzimuth;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ActualAzimuth
        {
            get
            {
                return _actualAzimuth;
            }
            set
            {
                if (value != _actualAzimuth)
                {
                    _actualAzimuth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int? TargetAzimuth
        {
            get
            {
                return _targetAzimuth;
            }
            set
            {
                if (value != _targetAzimuth)
                {
                    _targetAzimuth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
