using IOComm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DxLogStationMaster
{
    public class StationMaster : INotifyPropertyChanged, IDisposable
    {

        private COMPort _comPort;
        private List<Band> _bands;

        private bool _autoInformationStatusEnabled;
        private bool _locked;
        private bool _softwareLocked;
        private Band _rxBand;
        private Band _txBand;
        private Antenna _selectedRxAntenna;
        private Antenna _selectedTxAntenna;
        private bool _rxSplitEnabled;
        private bool _txSplitEnabled;
        private bool _isPttActive;
        private bool _connected;
        private Rotator _rotator;
        private string _serialNumber;
        private string _firmwareVersion;
        private string _routerVersion;
        private bool _isDeluxe;

        public event PropertyChangedEventHandler PropertyChanged;

        public StationMaster()
        {
            _rotator = new Rotator();
        }

        public StationMaster(COMPort comPort) : base()
        {
            ComPort = comPort;
        }

        #region Properties

        public COMPort ComPort
        {
            get
            {
                return _comPort;
            }

            set
            {
                if (value != _comPort)
                {
                    _comPort = value;
                    NotifyPropertyChanged();

                    _comPort.DataReceivedEvent += _comPort_DataReceivedEvent;
                }
            }
        }

        private string partialReceiveData;
        private void _comPort_DataReceivedEvent(byte[] rxData)
        {
            var data = System.Text.Encoding.UTF8.GetString(rxData, 0, rxData.Length);

            if (!string.IsNullOrEmpty(partialReceiveData))
            {
                data = partialReceiveData + data;
                partialReceiveData = string.Empty;
            }

            var lines = data.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.StartsWith("G#"))
                {
                    if (!ProcessConfigurationResponse(line))
                    {
                        partialReceiveData += line;
                    }
                }
                else if (line.StartsWith("I"))
                {
                    if (!ProcessAutoInformationStatusResponse(line))
                    {
                        partialReceiveData += line;
                    }
                }
                else if (line.StartsWith("L"))
                {
                    if (!ProcessLockStatusResponse(line))
                    {
                        partialReceiveData += line;
                    }
                }
                else if (line.StartsWith("C"))
                {
                    if (!ProcessConnectionStatusResponse(line))
                    {
                        partialReceiveData += line;
                    }
                }
                else if (line.StartsWith("P"))
                {
                    if (!ProcessPttStatusResponse(line))
                    {
                        partialReceiveData += line;
                    }
                }
                else if (line.StartsWith("V"))
                {
                    if (!ProcessVersionResponse(line))
                    {
                        partialReceiveData += line;
                    }
                }
                else if (line.StartsWith("U#"))
                {
                    if (!ProcessStatusResponse(line))
                    {
                        partialReceiveData += line;
                    }
                }
                else if (line.StartsWith("R#"))
                {
                    if (!ProcessRotatorResponse(line))
                    {
                        partialReceiveData += line;
                    }
                }
                else
                    Console.WriteLine($"Unknown response - {line}");
            }
        }

        public Antenna SelectedRxAntenna
        {
            get
            {
                return _selectedRxAntenna;
            }

            private set
            {
                if (value != _selectedRxAntenna)
                {
                    _selectedRxAntenna = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Antenna SelectedTxAntenna
        {
            get
            {
                return _selectedTxAntenna;
            }

            private set
            {
                if (value != _selectedTxAntenna)
                {
                    _selectedTxAntenna = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool RxSplitEnabled
        {
            get
            {
                return _rxSplitEnabled;
            }
            private set
            {
                if (value != _rxSplitEnabled)
                {
                    _rxSplitEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool TxSplitEnabled
        {
            get
            {
                return _txSplitEnabled;
            }
            private set
            {
                if (value != _txSplitEnabled)
                {
                    _txSplitEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Rotator Rotator
        {
            get
            {
                return _rotator;
            }
            private set
            {
                if (value != _rotator)
                {
                    _rotator = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Band RxBand
        {
            get
            {
                return _rxBand;
            }
            private set
            {
                if (value.Name != _rxBand?.Name)
                {
                    _rxBand = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Band TxBand
        {
            get
            {
                return _txBand;
            }
            private set
            {
                if (value.Name != _txBand?.Name)
                {
                    _txBand = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool AutoInformationStatusEnabled
        {
            get
            {
                return _autoInformationStatusEnabled;
            }
            private set
            {
                if (value != _autoInformationStatusEnabled)
                {
                    _autoInformationStatusEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsPttActive
        {
            get
            {
                return _isPttActive;
            }
            private set
            {
                if (value != _isPttActive)
                {
                    _isPttActive = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Connected
        {
            get
            {
                return _connected;
            }
            private set
            {
                if (value != _connected)
                {
                    _connected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsDeluxe
        {
            get
            {
                return _isDeluxe;
            }
            private set
            {
                if (value != _isDeluxe)
                {
                    _isDeluxe = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Locked
        {
            get
            {
                return _locked;
            }
            private set
            {
                if (value != _locked)
                {
                    _locked = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool SoftwareLocked
        {
            get
            {
                return _softwareLocked;
            }
            private set
            {
                if (value != _softwareLocked)
                {
                    _softwareLocked = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            private set
            {
                if (value != _serialNumber)
                {
                    _serialNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string FirmwareVersion
        {
            get
            {
                return _firmwareVersion;
            }
            private set
            {
                if (value != _firmwareVersion)
                {
                    _firmwareVersion = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string RouterVersion
        {
            get
            {
                return _routerVersion;
            }
            private set
            {
                if (value != _routerVersion)
                {
                    _routerVersion = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<Band> Bands
        {
            get
            {
                return _bands;
            }

            private set
            {
                if (value != _bands)
                {
                    _bands = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        public void GetAutoInformationStatus()
        {
            if (_comPort.IsOpen)
                _comPort.Write("I\r");
        }

        public void GetConnectionStatus()
        {
            if (_comPort.IsOpen)
                _comPort.Write("C\r");
        }

        public void GetVersionInformation()
        {
            if (_comPort.IsOpen)
                _comPort.Write("V\r");
        }
        public void GetDeviceSerialNumber()
        {
            if (_comPort.IsOpen)
                _comPort.Write("VS\r");
        }
        public void GetDeviceFirmwareVersion()
        {
            if (_comPort.IsOpen)
                _comPort.Write("VF\r");
        }
        public void GetRouterVersion()
        {
            if (_comPort.IsOpen)
                _comPort.Write("VR\r");
        }

        public void SetAutoInformationStatus(bool enable)
        {
            if (_comPort.IsOpen)
                _comPort.Write("I" + (enable ? "1" : "0") + "\r");
        }

        public void GetConfigurationStatus()
        {
            if (_comPort.IsOpen)
                _comPort.Write("G#\r");
        }

        public void GetStatus()
        {
            if (_comPort.IsOpen)
                _comPort.Write("U#\r");
        }

        public void SetRxAntenna(int selectionIndex)
        {
            if (_comPort.IsOpen)
                _comPort.Write($"UXRS{selectionIndex}\r");
        }

        public void SetTxAntenna(int selectionIndex)
        {
            if (_comPort.IsOpen)
                _comPort.Write($"UXTS{selectionIndex}\r");
        }

        public void SetNextAlternativeTxAntenna()
        {
            if (IsDeluxe)
            {
                if (_comPort.IsOpen)
                {
                    _comPort.Write($"UXTA\r");
                }
            }
            else
            {
                var index = Array.IndexOf(TxBand.Antennas, SelectedTxAntenna);
                if (index == RxBand.Antennas.Length - 1)
                {
                    SetTxAntenna(1);
                }
                else if (index < TxBand.Antennas.Length - 1)
                {
                    SetTxAntenna(index + 2);
                }
            }
        }

        public void SetNextAlternativeRxAntenna()
        {
            if (IsDeluxe)
            {
                if (_comPort.IsOpen)
                {
                    _comPort.Write($"UXRA\r");
                }
            }
            else
            {
                var index = Array.IndexOf(RxBand.Antennas, SelectedRxAntenna);
                if (index == RxBand.Antennas.Length - 1)
                {
                    SetRxAntenna(1);
                }
                else if (index < RxBand.Antennas.Length - 1)
                {
                    SetRxAntenna(index + 2);
                }
            }
        }

        #endregion

        #region Response Processing

        private bool ProcessConnectionStatusResponse(string data)
        {
            var match = Regex.Match(data, "C[01]");
            if (match.Success)
            {
                Connected = data[1] == '1';
                return true;
            }
            return false;
        }

        private bool ProcessRotatorResponse(string data)
        {
            // EG: R#{{6,?}}

            var rotatorStatus = Regex.Match(data, @"R#{{(?'actual'\d{1,3}|\?),(?'target'\d{1,3}|\?)}}");
            if (rotatorStatus.Success)
            {

                var rotator = new Rotator();

                rotator.ActualAzimuth = Convert.ToInt32(rotatorStatus.Groups["actual"].Value);

                if (rotatorStatus.Groups["target"].Value == "?")
                {
                    rotator.TargetAzimuth = null;
                }
                else
                {
                    rotator.TargetAzimuth = Convert.ToInt32(rotatorStatus.Groups["target"].Value);
                }

                Rotator = rotator;

                return true;

            }

            return false;
        }

        private bool ProcessPttStatusResponse(string data)
        {
            var pttStatus = Regex.Match(data, "P([01]){1,2}");
            if (pttStatus.Success)
            {
                IsPttActive = pttStatus.Groups[1].Value == "1";
                return true;
            }
            return false;
        }

        private bool ProcessVersionResponse(string data)
        {

            var serialNumberMatch = Regex.Match(data, "VS(.+)$");
            var firmwareVersionMatch = Regex.Match(data, "VF(.+)$");
            var routerVersionMatch = Regex.Match(data, "VR(.+)$");

            if (serialNumberMatch.Success)
            {
                SerialNumber = serialNumberMatch.Groups[1].Value;
                if (SerialNumber.StartsWith("SD"))
                {
                    IsDeluxe = true;
                }
                return true;
            }
            else if (firmwareVersionMatch.Success)
            {
                FirmwareVersion = firmwareVersionMatch.Groups[1].Value;
                return true;
            }
            else if (routerVersionMatch.Success)
            {
                RouterVersion = routerVersionMatch.Groups[1].Value;
                return true;
            }

            return false;

        }

        private bool ProcessLockStatusResponse(string data)
        {
            var lockStatus = Regex.Match(data, "(LS?)([01]){1,2}");
            if (lockStatus.Success)
            {

                if (lockStatus.Groups[1].Value == "LS")
                {
                    SoftwareLocked = lockStatus.Groups[2].Value == "1";
                }
                else
                {
                    Locked = lockStatus.Groups[2].Value == "1";

                }
                return true;
            }
            return false;
        }

        private bool ProcessAutoInformationStatusResponse(string data)
        {
            var match = Regex.Match(data, "I[01]");
            if (match.Success)
            {
                AutoInformationStatusEnabled = data[1] == '1';
                return true;
            }
            return false;
           
            return true;
        }

        private bool ProcessStatusResponse(string data)
        {
            // Example Response
            // U#0,0,{{1,2,0},{1,2,0}}

            if (_bands == null)
            {
                GetConfigurationStatus();
                GetStatus();
                return true;
            }

            var match = Regex.Match(data, @"U#(?'rxSplitEnabled'[01]),(?'txSplitEnabled'[01]),{{(?'bandIndexRx'[\d-]),(?'selectionIndexRx'[\d-]),(?'selectionGroupRx'[\d-])},{(?'bandIndexTx'[\d-]),(?'selectionIndexTx'[\d-]),(?'selectionGroupTx'[\d-])}}");

            if (match.Success)
            {

                var rxSplitEnabled = match.Groups["rxSplitEnabled"].Value == "1";
                var txSplitEnabled = match.Groups["txSplitEnabled"].Value == "1";
                var bandIndexRx = Convert.ToInt16(match.Groups["bandIndexRx"].Value) - 1;
                var selectionIndexRx = Convert.ToInt16(match.Groups["selectionIndexRx"].Value) - 1;
                var selectionGroupRx = Convert.ToInt16(match.Groups["selectionGroupRx"].Value) - 1;
                var bandIndexTx = Convert.ToInt16(match.Groups["bandIndexTx"].Value) - 1;
                var selectionIndexTx = Convert.ToInt16(match.Groups["selectionIndexTx"].Value) - 1;
                var selectionGroupTx = Convert.ToInt16(match.Groups["selectionGroupTx"].Value) - 1;

                RxBand = Bands[bandIndexRx];
                TxBand = Bands[bandIndexTx];
                RxSplitEnabled = rxSplitEnabled;
                TxSplitEnabled = txSplitEnabled;
                SelectedRxAntenna = Bands[bandIndexRx].Antennas[selectionIndexRx];
                SelectedTxAntenna = Bands[bandIndexTx].Antennas[selectionIndexTx];

                return true;
            }

            return false;

        }

        private bool ProcessConfigurationResponse(string data)
        {
            // Example Response
            // G#0,0,{{Rotator}},{{2M,144000000,146001000,{{{{A,0,X510N Vert,VERT,{},{}},{A,0,12 ELE,12ELE,{},{}},{A,0,XQuad 144H,144QH,{},{}},{A,0,XQuad 144V,144QV,{},{}}}}}},{70CM,430000000,440001000,{{{{A,0,X510N Vert,VERT,{},{}},{A,0,15 ELE,15ELE,{},{}},{A,0,XQuad 432H,432QH,{},{}},{A,0,XQuad 432V,432QV,{},{}}}}}},{23CM,1240000000,1300001000,{{{{A,0,X510N Vert,VERT,{},{}}}}}}}

            var fullRegex = new Regex(@"^G#(?'stationMasterAddress'[\d{1,2}]),(?'rxSplitEnabled'[01]),{{Rotator}},{(?'bands'.+)}");
            if (fullRegex.IsMatch(data))
            {

                Bands = new List<Band>();

                var f = fullRegex.Match(data);
                var stationMasterAddress = f.Groups["stationMasterAddress"].Value;
                var rxSplitEnabled = f.Groups["rxSplitEnabled"].Value == "1";

                var bandMatches = Regex.Matches(f.Groups["bands"].Value, @"{\w+,\d+,\d+,{.+?}{6},?");
                foreach (Match bm in bandMatches)
                {
                    var bandString = Regex.Match(bm.Value, @"{(?'bandName'\w+),(?'startFreq'\d+),(?'endFreq'\d+),{{{((?:{A,[01],[\w\d\s]+,[\w\d\s\s]+,{}.{}},?){1,})}}}}");

                    var band = new Band()
                    {
                        Name = bandString.Groups["bandName"].Value,
                        StartFrequency = Convert.ToInt32(bandString.Groups["startFreq"].Value),
                        EndFrequency = Convert.ToInt32(bandString.Groups["endFreq"].Value)
                    };

                    var antennaMatches = Regex.Matches(bandString.Groups[1].Value, @"{A,(?'rxOnly'[01]),(?'antennaName'[\w\d\s]+),(?'antennaLabel'[\w\d\s]+),{},{}},?");

                    band.Antennas = new Antenna[antennaMatches.Count];
                    var antennaIndex = 0;
                    foreach (Match am in antennaMatches)
                    {
                        var antenna = new Antenna()
                        {
                            IsReceiveOnly = am.Groups["rxOnly"].Value == "1",
                            AntennaName = am.Groups["antennaName"].Value,
                            AntennaLabel = am.Groups["antennaLabel"].Value
                        };
                        band.Antennas[antennaIndex] = antenna;
                        antennaIndex++;

                    }

                    Bands.Add(band);
                }

                return true;

            }

            return false;

        }

        #endregion

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            //_comPort.Dispose();
        }
    }
}
