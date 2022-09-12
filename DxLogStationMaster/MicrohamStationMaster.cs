using DxLogStationMaster;
using IOComm;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DXLog.net;

namespace DXLog.net
{
    public partial class MicrohamStationMaster : KForm
    {
        private ContestData _contestData;
        private FrmMain _frmMain = null;

        private Font _normalFont = new Font("Courier New", 10, FontStyle.Regular);
        private Font _boldFont = new Font("Courier New", 10, FontStyle.Bold);

        private Color _backColor = Color.Turquoise;
        private Color _foreColor = Color.Black;
        private Color _headingColor = Color.Black;

        private StationMaster _stationMaster;

        public static string CusWinName => "Station Master Status";
        public static int CusFormID => 20220901;

        public MicrohamStationMaster()
        {
            InitializeComponent();
            this.FormID = CusFormID;
        }

        public MicrohamStationMaster(ContestData contestData)
        {
            InitializeComponent();
            this.FormID = CusFormID;

            ColorSetTypes = new String[] {
                "Background",
                "Color",
                "Heading" };

            DefaultColors = new Color[] {
                Color.Turquoise,
                Color.Black,
                Color.Gray
            };

            _contestData = contestData;
            FormLayoutChangeEvent += new FormLayoutChange(Handle_FormLayoutChangeEvent);

            this.FormClosing += MicrohamStationMaster_FormClosing;
        }

        private void MicrohamStationMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            _stationMaster.Dispose();
        }

        private void Handle_FormLayoutChangeEvent()
        {
            InitializeLayout();
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout(_normalFont);

            try
            {
                _backColor = getColorByType("Background");
                _foreColor = getColorByType("Color");
                _headingColor = getColorByType("Heading");
                BackColor = _backColor;
                ForeColor = _foreColor;
            }
            catch
            {
            }

            if (base.FormLayout.FontName.Contains("Courier"))
            {
                _normalFont = new Font(base.FormLayout.FontName, base.FormLayout.FontSize, FontStyle.Regular);
                _boldFont = new Font(base.FormLayout.FontName, base.FormLayout.FontSize, FontStyle.Bold);
            }
            else
            {
                _normalFont = Helper.GetSpecialFont(FontStyle.Regular, base.FormLayout.FontSize);
                _boldFont = Helper.GetSpecialFont(FontStyle.Bold, base.FormLayout.FontSize);
            }

            lblHeaderRx.Font = _boldFont;
            lblHeaderRx.ForeColor = _headingColor;
            lblHeaderAntenna.Font = _boldFont;
            lblHeaderAntenna.ForeColor = _headingColor;
            lblHeaderTx.Font = _boldFont;
            lblHeaderTx.ForeColor = _headingColor;

            lblBandRxHeader.Font = _boldFont;
            lblBandRxHeader.ForeColor = _headingColor;
            lblBandTxHeader.Font = _boldFont;
            lblBandTxHeader.ForeColor = _headingColor;

            lblBandRx.Font = _normalFont;
            lblBandRx.ForeColor = _foreColor;
            lblBandTx.Font = _normalFont;
            lblBandTx.ForeColor = _foreColor;

            for (int i = 1; i < flowAntennaNames.Controls.Count; i++)
            {
                var l = (Label)flowAntennaNames.Controls[i];
                l.Font = _normalFont;
                l.ForeColor = _foreColor;
            }

            flowAntennas.Left = (this.ClientSize.Width - flowAntennas.Width) / 2;

            if (_frmMain == null)
            {
                _frmMain = (FrmMain)(ParentForm == null ? Owner : ParentForm);
                if (_frmMain != null)
                {

                    _contestData = _frmMain.ContestDataProvider;

                    var port = _frmMain.COMMainProvider._com.Where(x => x._portDeviceName == "microHAM").FirstOrDefault();

                    _stationMaster = new StationMaster(port);
                    _stationMaster.PropertyChanged += _stationMaster_PropertyChanged;

                    _stationMaster.GetConnectionStatus();
                    _stationMaster.GetVersionInformation();
                    _stationMaster.SetAutoInformationStatus(true);
                    _stationMaster.GetConfigurationStatus();

                    // Hookup keyboard monitoring.
                    _frmMain.KeyUp += _frmMain_KeyUp;
                }

                base.Text = "Microham SM";
            }
        }

        private void _frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Alt && e.KeyCode == Keys.F11)
            {
                _stationMaster.SetNextAlternativeTxAntenna();
            }
            else if (e.Alt && e.KeyCode == Keys.F11)
            {
                _stationMaster.SetNextAlternativeRxAntenna();
            }
            else if (e.Shift && e.Control && e.Alt && e.KeyValue >= 112 && e.KeyValue <= 118)
            {
                _stationMaster.SetRxAntenna(e.KeyValue - 111);
            }

        }

        private void _stationMaster_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_stationMaster.Bands):
                    break;
                case nameof(_stationMaster.TxBand):
                case nameof(_stationMaster.RxBand):
                    AddAntennaSelectionUI();
                    break;
                case nameof(_stationMaster.SelectedRxAntenna):
                case nameof(_stationMaster.SelectedTxAntenna):
                    UpdateAntennaSelectionUI();
                    break;
                case nameof(_stationMaster.SerialNumber):
                    UpdateWindowTitle();
                    break;
                case nameof(_stationMaster.Rotator):
                    UpdateRotator();
                    break;
            }
        }



        private void UpdateWindowTitle()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateWindowTitle));
            }
            else
            {
                base.Text = $"Microham SM [{_stationMaster.SerialNumber}]";
            }
        }

        private void UpdateRotator()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateRotator));
            }
            else
            {
                lblRotator.Text = $"{_stationMaster.Rotator.ActualAzimuth.ToString("D3")}°";
                if (_stationMaster.Rotator.TargetAzimuth.HasValue)
                {
                    lblRotator.Text += $" ({_stationMaster.Rotator.TargetAzimuth?.ToString("D3")}°)";
                }
            }
        }

        private void UpdateAntennaSelectionUI()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateAntennaSelectionUI));
            }
            else
            {

                if (_stationMaster.RxBand == null || _stationMaster.TxBand == null)
                {
                    return;
                }

                lblBandRx.Text = _stationMaster.RxBand.Name;
                lblBandTx.Text = _stationMaster.TxBand.Name;

                if (_stationMaster.SelectedRxAntenna != null)
                {
                    var rxid = Array.IndexOf(_stationMaster.RxBand.Antennas, _stationMaster.SelectedRxAntenna);
                    if (rxid > -1)
                        ((RadioButton)flowRxAntennas.Controls[rxid + 1]).Checked = true;
                }
                if (_stationMaster.SelectedTxAntenna != null)
                {
                    var txid = Array.IndexOf(_stationMaster.TxBand.Antennas, _stationMaster.SelectedTxAntenna);
                    if (txid > -1)
                        ((RadioButton)flowTxAntennas.Controls[txid + 1]).Checked = !_stationMaster.SelectedTxAntenna.IsReceiveOnly && true;
                }

            }
        }

        private void AddAntennaSelectionUI()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(AddAntennaSelectionUI));
            }
            else
            {

                if (_stationMaster.TxBand == null)
                {
                    return;
                }

                lblBandTx.Text = _stationMaster.TxBand.Name;
                lblBandRx.Text = _stationMaster.RxBand.Name;

                var countOfExistingAntennas = flowTxAntennas.Controls.Count;
                var countOfNewAntennas = _stationMaster.TxBand?.Antennas.Length ?? 0;

                // We don't want flickering so rather than clear and add controls we are going to reuse.
                // Clear any extra placeholders we don't need.

                for (int i = 1; i < countOfExistingAntennas - countOfNewAntennas; i++)
                {
                    flowRxAntennas.Controls.RemoveAt(1);
                    flowAntennaNames.Controls.RemoveAt(1);
                    flowTxAntennas.Controls.RemoveAt(1);
                }

                for (int i = 0; i < _stationMaster.TxBand.Antennas.Length; i++)
                {
                    Antenna antenna = _stationMaster.TxBand.Antennas[i];

                    if (flowAntennaNames.Controls.Count > i + 1)
                    {
                        flowAntennaNames.Controls[i + 1].Text = antenna.AntennaName;
                        flowTxAntennas.Controls[i + 1].Enabled = !antenna.IsReceiveOnly;
                    }
                    else
                    {
                        var rxRadioButton = new RadioButton()
                        {
                            AutoSize = true,
                            Margin = new Padding(9, 3, 3, 3),
                            Size = new Size(14, 13),
                            TabStop = true,
                            UseVisualStyleBackColor = true,
                        };
                        rxRadioButton.CheckedChanged += RxRadioButton_CheckedChanged;

                        var lblAntennaName = new Label()
                        {
                            Text = antenna.AntennaName,
                            Margin = new Padding(3, 0, 3, 1),
                            AutoSize = false,
                            Size = new Size(100, 18),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Font = _normalFont,
                            ForeColor = _foreColor
                        };
                        var txRadioButton = new RadioButton()
                        {
                            AutoSize = true,
                            Margin = new Padding(9, 3, 3, 3),
                            Size = new Size(14, 13),
                            TabStop = true,
                            UseVisualStyleBackColor = true,
                            Enabled = !antenna.IsReceiveOnly
                        };

                        txRadioButton.CheckedChanged += TxRadioButton_CheckedChanged; ;

                        flowRxAntennas.Controls.Add(rxRadioButton);
                        flowAntennaNames.Controls.Add(lblAntennaName);
                        flowTxAntennas.Controls.Add(txRadioButton);

                    }
                }
            }
        }

        private void TxRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            if (rb.Checked)
            {
                var txid = flowTxAntennas.Controls.IndexOf((RadioButton)sender);
                if (txid > -1)
                {
                    _stationMaster.SetTxAntenna(txid);
                }
            }
        }

        private void RxRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            if (rb.Checked)
            {
                var rxid = flowRxAntennas.Controls.IndexOf((RadioButton)sender);
                if (rxid > -1)
                {
                    _stationMaster.SetRxAntenna(rxid);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var frm = new MicrohamStationMaster();
            frm.Show();

            var frm2 = new MicrohamStationMaster();
            frm2.Show();

            this.Close();
        }

        private void MicrohamStationMaster_Load(object sender, EventArgs e)
        {
            var lbl1 = new Label() { Text = "jhjhjh" };
            var lbl2 = new Label() { Text = "jhjhjh" };
            var lbl3 = new Label() { Text = "jhjhjh" };

            tableLayoutPanel2.Controls.Add(lbl1, 0, 0);
            tableLayoutPanel2.Controls.Add(lbl2, 0, 1);
            tableLayoutPanel2.Controls.Add(lbl3, 0, 2);
        }
    }
}
