using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DXLog.net;

namespace DXLog.net
{
    public partial class SpotsOfMe : KForm
    {
        public static String CusWinName
        {
            get { return "Spots of me"; }
        }

        public static Int32 CusFormID
        {
            get { return 1000; }
        }
        

        private Font _windowFont = new Font("Courier New", 10, FontStyle.Regular);
        private static readonly int Shownspots = 8;
        private DXCLine[] _spotLines = new DXCLine[Shownspots];

        private ContestData _cdata = null;
        // private EDI _edi = null;
        private FrmMain mainForm = null;

        //private delegate void newQsoSaved(DXQSO qso);

        public SpotsOfMe()
        {
            InitializeComponent();
        }

        public SpotsOfMe(ContestData cdata)
        {
            InitializeComponent();
            ColorSetTypes = new String[] { "Background", "Color", "Header back color", "Header color", "Footer back color", "Footer color", "Final score color", "Selection back color", "Selection color" };
            DefaultColors = new Color[] { Color.Turquoise, Color.Black, Color.Gray, Color.Black, Color.Silver, Color.Black, Color.Blue, Color.SteelBlue, Color.White };
            _cdata = cdata;
            this.FormLayoutChangeEvent += new FormLayoutChange(Handle_FormLayoutChangeEvent);

        }

        private void Handle_FormLayoutChangeEvent()
        {
            InitializeLayout();
        }

        public override void InitializeLayout() 
        {
            base.InitializeLayout(_windowFont);

            if (base.FormLayout.FontName.Contains("Courier"))
                _windowFont = new Font(base.FormLayout.FontName, base.FormLayout.FontSize, FontStyle.Regular);
            else
                _windowFont = Helper.GetSpecialFont(FontStyle.Regular, base.FormLayout.FontSize);

            if (mainForm == null)
            {
                mainForm = (FrmMain)(this.ParentForm == null ? this.Owner : this.ParentForm);
                if (mainForm != null)
                    _cdata.SpotReceived += new ContestData.SpotReceivedDelegate(MainForm_NewClusterLine);
            }
            //base.Text = String.Format("Spots of {0}", _cdata.activeContest.dalHeader.Callsign);
            base.Text = "80m spots"; 
        }

        private void MainForm_NewClusterLine(DXCLine dXCLine)
        {
            int i;
            StringBuilder sb = new StringBuilder();


            if ((dXCLine.Callsign == _cdata.activeContest.dalHeader.Callsign || true) && ((int)(dXCLine.Freq / 1000.0) == 3)) { 
                // Shift list one step up
                for (i = 0; i < Shownspots - 1; i++)
                {
                    if (_spotLines[i+1] != null)
                    {
                        _spotLines[i] = _spotLines[i + 1];
                        sb.Append(String.Format("{0,-10} de ", _spotLines[i].Callsign));
                        //sb.AppendLine(String.Format("{0} on {1:0.0}kHz at {2}Z", _spotLines[i].Sender, _spotLines[i].Freq, _spotLines[i].UTC.ToString("HH:mm")));
                        sb.AppendLine(String.Format("{0,-10} on {1:0.0}", _spotLines[i].Sender, _spotLines[i].Freq));
                    }
                    else
                        sb.AppendLine("");
                }
                _spotLines[Shownspots - 1] = dXCLine;
                sb.Append(String.Format("{0,-10} de ", dXCLine.Callsign));
                //sb.AppendLine(String.Format("{0} on {1:0.0}kHz at {2}Z", dXCLine.Sender, dXCLine.Freq, dXCLine.UTC.ToString("HH:mm")));
                sb.AppendLine(String.Format("{0,-10} on {1:0.0}", dXCLine.Sender, dXCLine.Freq));

                lbInfo.Text = sb.ToString();
                //SpotsOfMe.ActiveForm.Text = sb.ToString();
            }
        }
    }
}
