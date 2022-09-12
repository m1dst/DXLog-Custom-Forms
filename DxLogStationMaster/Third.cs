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
    public partial class Third : KForm
    {
        public static String CusWinName
        {
            get { return "Third"; }
        }

        public static Int32 CusFormID
        {
            get { return 1002; }
        }

        private ContestData _cdata = null;
        private Font _windowFont = new Font("Courier New", 10, FontStyle.Regular);

        private FrmMain mainForm = null;

        private delegate void newQsoSaved(DXQSO qso);

        public Third()
        {
            InitializeComponent();
        }

        public Third(ContestData cdata)
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
                    mainForm.NewQSOSaved += new FrmMain.NewQSOSavedEvent(MainForm_NewQSOSaved);
            }



        }

        private void MainForm_NewQSOSaved(DXQSO newQso)
        {
            if (this.InvokeRequired)
            {
                newQsoSaved d = new newQsoSaved(MainForm_NewQSOSaved);
                this.Invoke(d, new object[] { newQso });
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("New QSO is saved.");
            sb.AppendLine(String.Format("QSO time: {0}", newQso.QSOTime.ToString("dd.MM.yyyy HH:mm:ss")));
            sb.AppendLine(String.Format("Call worked: {0}", newQso.Callsign));
            sb.AppendLine();
            sb.AppendLine(String.Format("Your current score is: {0} points!", _cdata.GetFinalScore().ToString("### ### ##0")));
            lbInfo.Text = sb.ToString();
        }


    }
}
