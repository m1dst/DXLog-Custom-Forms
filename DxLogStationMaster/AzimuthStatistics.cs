using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DXLog.net
{
    public partial class AzimuthStatistics : KForm
    {
        public static string CusWinName => "Azimuth Statistics";
        public static int CusFormID => 20220906;

        private ContestData _contestData = null;
        private FrmMain _frmMain = null;

        private Font _normalFont = new Font("Courier New", 10, FontStyle.Regular);
        private Font _boldFont = new Font("Courier New", 10, FontStyle.Bold);

        private void handle_FormLayoutChangeEvent() => InitializeLayout();


        public AzimuthStatistics()
        {
            InitializeComponent();
            this.FormID = CusFormID;
        }

        public AzimuthStatistics(ContestData contestData)
        {
            InitializeComponent();
            this.FormID = CusFormID;

            _contestData = contestData;

            ColorSetTypes = new[]
            {
                "Background",
                "Font",
                "Grid",
                "Line"
            };

            DefaultColors = new[] {
                Color.MediumBlue,
                Color.White,
                Color.White,
                Color.Yellow
            };

            FormLayoutChangeEvent += new FormLayoutChange(Handle_FormLayoutChangeEvent);

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
            var groupByAzimuthQuery =
                from qso in _contestData.QSOList
                where !qso.InvalidQSO
                group qso by Convert.ToInt16(qso.Az) into newGroup
                orderby newGroup.Key
                select newGroup;

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateData));
            }
            else
            {
                chart1.Series["Azimuth"].Points.Clear();
                foreach (var nameGroup in groupByAzimuthQuery)
                {
                    chart1.Series["Azimuth"].Points.AddXY(nameGroup.Key, nameGroup.Count());
                }
            }

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout(_normalFont);

            chart1.BackColor = getColorByType("Background");
            chart1.Series[0].Color = getColorByType("Line");

            chart1.ChartAreas[0].BackColor = getColorByType("Background");

            chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX.MajorTickMark.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX.MinorGrid.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX.MinorTickMark.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = _normalFont;

            chart1.ChartAreas[0].AxisX2.LabelStyle.ForeColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX2.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX2.MajorGrid.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX2.MajorTickMark.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX2.MinorGrid.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX2.MinorTickMark.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisX2.LabelStyle.Font = _normalFont;

            chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY.MajorTickMark.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY.MinorGrid.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY.MinorTickMark.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = _normalFont;

            chart1.ChartAreas[0].AxisY2.LabelStyle.ForeColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY2.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY2.MajorTickMark.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY2.MinorGrid.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY2.MinorTickMark.LineColor = getColorByType("Grid");
            chart1.ChartAreas[0].AxisY2.LabelStyle.Font = _normalFont;

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

            if (_frmMain == null)
            {
                _frmMain = (FrmMain)(ParentForm == null ? Owner : ParentForm);
                if (_frmMain != null)
                {
                    _frmMain.NewQSOSaved += new FrmMain.NewQSOSavedEvent(MainForm_NewQSOSaved);
                }
            }

        }

        private void MainForm_NewQSOSaved(DXQSO newQso)
        {
            UpdateData();
        }

    }

}
