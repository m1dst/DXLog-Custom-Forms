using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ConfigFile;

namespace DXLog.net
{
    public partial class GridSquareProperties : Form
    {
        private GridSquareSettings _settings;

        public GridSquareProperties()
        {
            InitializeComponent();
        }

        public GridSquareProperties(GridSquareSettings settings)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            _settings = settings;
            _settings.PropertyChanged += _settings_PropertyChanged;

            chkColourWorkedGridSquares.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.ColourWorkedGridSquares), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkShowGridFields.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.ShowFields), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkShowGridFieldsLabel.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.ShowFieldsLabel), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkShowGridSquares.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.ShowGridSquares), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkShowGridSquaresLabel.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.ShowGridSquaresLabel), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkShowGridSubsquares.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.ShowSubsquares), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkShowGridSubsquaresLabel.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.ShowSubsquaresLabel), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkDisplaySpots.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.DisplaySpots), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkDisplayContacts.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.DisplayContacts), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkZoomToQsos.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.ZoomToQsos), false, DataSourceUpdateMode.OnPropertyChanged, false));
            txtCentreMapOnLocator.DataBindings.Add(new Binding("Text", _settings, nameof(GridSquareSettings.CentreMapOnLocator), false, DataSourceUpdateMode.OnPropertyChanged, false));
            chkCentreMapOnQth.DataBindings.Add(new Binding("Checked", _settings, nameof(GridSquareSettings.CentreMapOnQth), false, DataSourceUpdateMode.OnPropertyChanged, false));
            //cboMapProvider.DataBindings.Add(new Binding("SelectedItem", _settings, nameof(GridSquareSettings.ColourWorkedGridSquares), false, DataSourceUpdateMode.OnPropertyChanged, false));
            cboMinZoom.DataBindings.Add(new Binding("SelectedIndex", _settings, nameof(GridSquareSettings.MinZoom), false, DataSourceUpdateMode.OnPropertyChanged, false));
            cboMaxZoom.DataBindings.Add(new Binding("SelectedIndex", _settings, nameof(GridSquareSettings.MaxZoom), false, DataSourceUpdateMode.OnPropertyChanged, false));
            cboStartZoom.DataBindings.Add(new Binding("SelectedIndex", _settings, nameof(GridSquareSettings.StartZoom), false, DataSourceUpdateMode.OnPropertyChanged, false));

            chkShowGridMaster.Checked = chkShowGridFields.Checked && chkShowGridSquares.Checked && chkShowGridSubsquares.Checked;
            chkShowGridLabelsMaster.Checked = chkShowGridFieldsLabel.Checked && chkShowGridSquaresLabel.Checked && chkShowGridSubsquaresLabel.Checked;

            for (var zoomLevel = 0; zoomLevel < 18; zoomLevel++)
            {
                cboMinZoom.Items.Add(zoomLevel);
                cboMaxZoom.Items.Add(zoomLevel);
                cboStartZoom.Items.Add(zoomLevel);
            }

        }

        private void _settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //switch (e.PropertyName)
            //{
            //    case nameof(_stationMaster.Bands):
            //        break;
            //    case nameof(_stationMaster.TxBand):
            //    case nameof(_stationMaster.RxBand):
            //        AddAntennaSelectionUI();
            //        break;
            //    case nameof(_stationMaster.SelectedRxAntenna):
            //    case nameof(_stationMaster.SelectedTxAntenna):
            //        UpdateAntennaSelectionUI();
            //        break;
            //    case nameof(_stationMaster.SerialNumber):
            //        UpdateWindowTitle();
            //        break;
            //    case nameof(_stationMaster.Rotator):
            //        UpdateRotator();
            //        break;
            //}
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            // Validate the locator is OK.
            if(!Regex.IsMatch(txtCentreMapOnLocator.Text, @"^[A-R]{2}[\d]{2}[A-X]{2}$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Locator is not valid.", "Error!");
                return;
            }

            Config.Save("ColourWorkedGridSquares", chkColourWorkedGridSquares.Checked);
            Config.Save("ShowFields", chkShowGridFields.Checked);
            Config.Save("ShowFieldsLabel", chkShowGridFieldsLabel.Checked);
            Config.Save("ShowGridSquares", chkShowGridSquares.Checked);
            Config.Save("ShowGridSquaresLabel", chkShowGridSquaresLabel.Checked);
            Config.Save("ShowSubsquares", chkShowGridSubsquares.Checked);
            Config.Save("ShowSubsquaresLabel", chkShowGridSubsquaresLabel.Checked);
            Config.Save("DisplaySpots", chkDisplaySpots.Checked);
            Config.Save("DisplayContacts", chkDisplayContacts.Checked);
            Config.Save("ZoomToQsos", chkZoomToQsos.Checked);
            Config.Save("CentreMapOnLocator", txtCentreMapOnLocator.Text.ToUpper());
            Config.Save("CentreMapOnQth", chkCentreMapOnQth.Checked);
            //Config.Save("MapSourceProvider", cboMapProvider.SelectedItem);
            Config.Save("MinZoom", cboMinZoom.SelectedIndex);
            Config.Save("MaxZoom", cboMaxZoom.SelectedIndex);
            Config.Save("StartZoom", cboStartZoom.SelectedIndex);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            var settings = new GridSquareSettings();
            settings.CopyProperties(_settings);
        }

        private void chkShowGridLabel_CheckedChanged(object sender, EventArgs e)
        {
            chkShowGridLabelsMaster.Checked = chkShowGridFieldsLabel.Checked && chkShowGridSquaresLabel.Checked && chkShowGridSubsquaresLabel.Checked;
        }

        private void chkShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            chkShowGridMaster.Checked = chkShowGridFields.Checked && chkShowGridSquares.Checked && chkShowGridSubsquares.Checked;
        }

        private void chkShowGridMaster_Click(object sender, EventArgs e)
        {
            chkShowGridFields.Checked = chkShowGridMaster.Checked;
            chkShowGridSquares.Checked = chkShowGridMaster.Checked;
            chkShowGridSubsquares.Checked = chkShowGridMaster.Checked;
        }

        private void chkShowGridLabelsMaster_Click(object sender, EventArgs e)
        {
            chkShowGridFieldsLabel.Checked = chkShowGridLabelsMaster.Checked;
            chkShowGridSquaresLabel.Checked = chkShowGridLabelsMaster.Checked;
            chkShowGridSubsquaresLabel.Checked = chkShowGridLabelsMaster.Checked;
        }
    }
}
