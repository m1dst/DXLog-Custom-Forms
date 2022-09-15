using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DXLog.net
{
    public partial class ExchangeInfo : KForm
    {
        public static string CusWinName => "Exchange Info";
        public static int CusFormID => 20220914;

        private readonly ContestData _contestData;
        private FrmMain _frmMain;

        private Font _normalFont = new Font("Courier New", 10, FontStyle.Regular);
        private Font _boldFont = new Font("Courier New", 10, FontStyle.Bold);

        public ExchangeInfo()
        {
            InitializeComponent();
            FormID = CusFormID;
        }

        public ExchangeInfo(ContestData contestData)
        {

            InitializeComponent();
            FormID = CusFormID;

            _contestData = contestData;

            ColorSetTypes = new[]
            {
                "Background",
                "Font"
            };

            DefaultColors = new[] {
                Color.MediumBlue,
                Color.White
            };

            FormLayoutChangeEvent += Handle_FormLayoutChangeEvent;

            while (contextMenuStrip1.Items.Count > 0)
                contextMenuStrip2.Items.Add(contextMenuStrip1.Items[0]);

            UpdateData();

        }

        private void Handle_FormLayoutChangeEvent()
        {
            InitializeLayout();
        }

        private void UpdateData()
        {

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateData));
            }
            else
            {
                if (!string.IsNullOrEmpty(_contestData.activeContest.dalHeader.CWMessage2))
                {
                    var result = _contestData.activeContest.dalHeader.CWMessage2;
                    var macro = @"\$[\w\d]+";
                    foreach (Match m in Regex.Matches(_contestData.activeContest.dalHeader.CWMessage2, macro))
                    {
                        switch (m.Value)
                        {
                            case "$CQZONE":
                            case "$WAZZONE":
                                if (!int.TryParse(_contestData.dalHeader.WAZZone, out var zone))
                                {
                                    zone = int.Parse(_contestData.activeContest._myDXCC?.CQZone ?? string.Empty);
                                }
                                result = result.Replace(m.Value, zone.ToString());
                                break;
                            case "$EXCHANGE":
                                result = result.Replace(m.Value, _contestData.dalHeader.Exchange);
                                break;
                            case "$GRID4":
                                result = result.Replace(m.Value, _contestData.dalHeader.GridSquare.Substring(0, 4));
                                break;
                            case "$GRID":
                                result = result.Replace(m.Value, _contestData.dalHeader.GridSquare);
                                break;
                            case "$MYDXCC":
                                var dxcc = string.Empty;
                                if (_contestData.dalHeader.DXCCPrefix.Trim() != string.Empty)
                                {
                                    dxcc = _contestData.dalHeader.DXCCPrefix.Trim();
                                }
                                else if (_contestData.activeContest._myDXCC != null)
                                {
                                    dxcc = _contestData.activeContest._myDXCC.MainPrefix;
                                }
                                result = result.Replace(m.Value, dxcc);
                                break;
                            case "$RST":
                                if (!string.IsNullOrEmpty(_frmMain?.CurrentEntryLine?.ActualQSO?.Sent))
                                {
                                    var rst = _frmMain.CurrentEntryLine.ActualQSO.Sent;
                                    result = result.Replace(m.Value, rst);
                                }
                                else
                                {
                                    result = result.Replace(m.Value, _contestData.TXMode == "CW" ? "599" : "59");
                                }
                                break;
                            case "$SERIAL":
                                result = result.Replace(m.Value, _frmMain?.CurrentEntryLine?.ActualQSO != null ? _frmMain.CurrentEntryLine.ActualQSO.Nr.ToString() : "000");
                                break;
                            case "$STATE":
                                result = result.Replace(m.Value, _contestData.dalHeader.State);
                                break;
                        }
                    }

                    lblExchange.Text = result;

                }
                else
                {
                    lblExchange.Text = string.Empty;
                }
            }

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout(_normalFont);

            if (FormLayout.FontName.Contains("Courier"))
            {
                _normalFont = new Font(base.FormLayout.FontName, base.FormLayout.FontSize, FontStyle.Regular);
                _boldFont = new Font(base.FormLayout.FontName, base.FormLayout.FontSize, FontStyle.Bold);
            }
            else
            {
                _normalFont = Helper.GetSpecialFont(FontStyle.Regular, base.FormLayout.FontSize);
                _boldFont = Helper.GetSpecialFont(FontStyle.Bold, base.FormLayout.FontSize);
            }

            if (_frmMain == null)
            {
                _frmMain = (FrmMain)(ParentForm ?? Owner);
                if (_frmMain != null)
                {
                    _contestData.RadioDataChanged += Handle_RadioDataChanged;
                    _frmMain.CurrentEntryLine.ActualQSO.DataChangedEvent += ActualQSO_DataChangedEvent;

                    foreach (var ctrl in _frmMain.CurrentEntryLine.Controls)
                    {
                        if (ctrl is TextBox textBox)
                        {
                            textBox.BackColor = Color.LightGray;
                            textBox.TextChanged += TextBox_TextChanged;
                        }
                    }
                }
            }

            BackColor = getColorByType("Background");
            lblExchange.ForeColor = getColorByType("Font");
            lblExchange.Font = _boldFont;

            UpdateData();

        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void ActualQSO_DataChangedEvent(string property)
        {
            UpdateData();
        }

        private void Handle_RadioDataChanged(int radioNumber, string vfo)
        {
            UpdateData();
        }

    }

}
