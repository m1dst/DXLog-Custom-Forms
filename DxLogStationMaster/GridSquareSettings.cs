using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DXLog.net
{
    public class GridSquareSettings : INotifyPropertyChanged
    {

        #region Fields

        private bool _colourWorkedGridSquares = true;
        private bool _showFields  = true;
        private bool _showFieldsLabel  = true;
        private bool _showGridSquares  = true;
        private bool _showGridSquaresLabel  = true;
        private bool _showSubsquares = true;
        private bool _showSubsquaresLabel = true;
        private bool _displaySpots  = false;
        private bool _displayContacts  = true;
        private bool _zoomToQsos  = true;
        private string _centreMapOnLocator  = "";
        private bool _centreMapOnQth  = true;
        private string _mapSourceProvider;
        private int _minZoom  = 4;
        private int _maxZoom  = 14;
        private int _startZoom  = 4;

        #endregion

        #region Properties

        public bool ColourWorkedGridSquares
        {
            get => _colourWorkedGridSquares;

            set
            {
                if (value == _colourWorkedGridSquares) return;
                _colourWorkedGridSquares = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowFields
        {
            get => _showFields;
            set
            {
                if (value == _showFields) return;
                _showFields = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowFieldsLabel
        {
            get => _showFieldsLabel;
            set
            {
                if (value == _showFieldsLabel) return;
                _showFieldsLabel = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowGridSquares
        {
            get => _showGridSquares;
            set
            {
                if (value == _showGridSquares) return;
                _showGridSquares = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowGridSquaresLabel
        {
            get => _showGridSquaresLabel;
            set
            {
                if (value == _showGridSquaresLabel) return;
                _showGridSquaresLabel = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowSubsquares
        {
            get => _showSubsquares;
            set
            {
                if (value == _showSubsquares) return;
                _showSubsquares = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowSubsquaresLabel
        {
            get => _showSubsquaresLabel;
            set
            {
                if (value == _showSubsquaresLabel) return;
                _showSubsquaresLabel = value;
                NotifyPropertyChanged();
            }
        }

        public bool DisplaySpots
        {
            get => _displaySpots;
            set
            {
                if (value == _displaySpots) return;
                _displaySpots = value;
                NotifyPropertyChanged();
            }
        }

        public bool DisplayContacts
        {
            get => _displayContacts;
            set
            {
                if (value == _displayContacts) return;
                _displayContacts = value;
                NotifyPropertyChanged();
            }
        }

        public bool ZoomToQsos
        {
            get => _zoomToQsos;
            set
            {
                if (value == _zoomToQsos) return;
                _zoomToQsos = value;
                NotifyPropertyChanged();
            }
        }

        public string CentreMapOnLocator
        {
            get => _centreMapOnLocator;
            set
            {
                if (value == _centreMapOnLocator) return;
                _centreMapOnLocator = value;
                NotifyPropertyChanged();
            }
        }

        public bool CentreMapOnQth
        {
            get => _centreMapOnQth;
            set
            {
                if (value == _centreMapOnQth) return;
                _centreMapOnQth = value;
                NotifyPropertyChanged();
            }
        }

        public string MapSourceProvider
        {
            get => _mapSourceProvider;
            set
            {
                if (value == _mapSourceProvider) return;
                _mapSourceProvider = value;
                NotifyPropertyChanged();
            }
        }

        public int MinZoom
        {
            get => _minZoom;
            set
            {
                if (value == _minZoom) return;
                _minZoom = value;
                NotifyPropertyChanged();
            }
        }

        public int MaxZoom
        {
            get => _maxZoom;
            set
            {
                if (value == _maxZoom) return;
                _maxZoom = value;
                NotifyPropertyChanged();
            }
        }

        public int StartZoom
        {
            get => _startZoom;
            set
            {
                if (value == _startZoom) return;
                _startZoom = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
