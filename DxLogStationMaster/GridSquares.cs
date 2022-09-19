using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Maps.Common;
using System.Windows.Forms.Maps.Elements;
using System.Windows.Forms.Maps.Layers;
using ConfigFile;

namespace DXLog.net
{
    public partial class GridSquares : KForm
    {
        public static string CusWinName => "Grid Squares";
        public static int CusFormID => 20220911;

        private ContestData _contestData;
        private FrmMain _frmMain;

        private Font _normalFont = new Font("Courier New", 10, FontStyle.Regular);

        private List<string> _workedLocators = new List<string>();
        private List<string> _workedStations = new List<string>();
        private List<string> _notWorkedSpotLocators = new List<string>();

        private GridSquareSettings _gridSquareSettings = new GridSquareSettings();

        private LayerGroup fieldGridLayerGroup = new LayerGroup();
        private PolygonLayer fieldGridPolygonLayer = new PolygonLayer(20);
        private MarkerLayer fieldGridMarkerLayer = new MarkerLayer(2);

        private LayerGroup squareGridLayerGroup = new LayerGroup();
        private PolygonLayer squareGridPolygonLayer = new PolygonLayer(20);
        private MarkerLayer squareGridMarkerLayer = new MarkerLayer(2);

        private LayerGroup subsquareGridLayerGroup = new LayerGroup();
        private PolygonLayer subsquareGridPolygonLayer = new PolygonLayer(0);
        private MarkerLayer subsquareGridMarkerLayer = new MarkerLayer(100);

        private MarkerLayer qsoMarkerLayer = new MarkerLayer(1);
        private MarkerLayer spotMarkerLayer = new MarkerLayer(1);


        public GridSquares()
        {
            InitializeComponent();
            FormID = CusFormID;

            mapControl.CacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MapControl");
            mapControl.MinZoomLevel = 4;
            mapControl.MaxZoomLevel = 14;
            mapControl.ZoomLevel = 4;

        }

        public GridSquares(ContestData contestData)
        {
            InitializeComponent();
            FormID = CusFormID;

            if (_frmMain == null)
            {
                _frmMain = (FrmMain)(ParentForm ?? Owner);
            }

            _contestData = contestData;

            ColorSetTypes = new[]
            {
                "Field Text",
                "Field Grid",
                "Grid Square Text",
                "Grid Square Grid",
                "Grid Square Worked Fill",
                "Grid Square Worked Text",
                "Sub Grid Square Text",
                "Sub Grid Square Grid",
                "Sub Grid Square Worked Fill",
                "Sub Grid Square Worked Text",
                "Sub Grid Square Spot Fill",
                "Sub Grid Square Spot Text"
            };

            DefaultColors = new[] {
                Color.Coral,
                Color.Gray,
                Color.Red,
                Color.DarkGray,
                Color.Black,
                Color.Red,
                Color.Red,
                Color.Black,
                Color.Red,
                Color.Black,
                Color.Green,
                Color.White
            };

            FormLayoutChangeEvent += Handle_FormLayoutChangeEvent;

            while (contextMenuStrip1.Items.Count > 0)
                contextMenuStrip2.Items.Add(contextMenuStrip1.Items[0]);

            mapControl.CacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MapControl");
            mapControl.MinZoomLevel = 4;
            mapControl.MaxZoomLevel = 14;
            mapControl.ZoomLevel = 4;
            mapControl.TileServer = new OpenStreetMapTileServer(userAgent: "DXLog");

            // group layers together
            fieldGridLayerGroup.AddLayer(fieldGridMarkerLayer);
            fieldGridLayerGroup.AddLayer(fieldGridPolygonLayer);

            squareGridLayerGroup.AddLayer(squareGridMarkerLayer);
            squareGridLayerGroup.AddLayer(squareGridPolygonLayer);

            subsquareGridLayerGroup.AddLayer(subsquareGridMarkerLayer);
            subsquareGridLayerGroup.AddLayer(subsquareGridPolygonLayer);

            // add layers to map
            mapControl.AddLayer(squareGridLayerGroup);
            mapControl.AddLayer(fieldGridLayerGroup);
            mapControl.AddLayer(subsquareGridLayerGroup);

            mapControl.ElementClick += MapControl_ElementClick;
            mapControl.ElementEnter += MapControl_ElementEnter;
            mapControl.ElementLeave += MapControl_ElementLeave;

            var tileServers = new ITileServer[]
            {
                new OpenStreetMapTileServer(userAgent: "DemoApp for WinFormsMapControl 1.0 contact example@example.com"),
                new StamenTerrainTileServer(),
                new OpenTopoMapServer(),
                new OfflineTileServer(),
                new BingMapsAerialTileServer(),
                new BingMapsRoadsTileServer(),
                new BingMapsHybridTileServer(),
            };

            var locator = DXLogCalculators.MaidenheadLocator.LocatorToLatLong("JO60XA");
            mapControl.Center = new GeoPoint((float)locator.Long, (float)locator.Lat);

            LoadConfiguration();
            RefreshWorkedData();

        }

        private void Handle_FormLayoutChangeEvent()
        {
            InitializeLayout();
        }

        private void MapControl_ElementLeave(object sender, MapControlElementEventArgs e)
        {
            //if (e.Element is Polygon)
            //{
            //}
            //else if (e.Element is Marker)
            //{
            //}
        }

        private void MapControl_ElementEnter(object sender, MapControlElementEventArgs e)
        {
            //if (e.Element is Polygon)
            //{
            //}
            //else if (e.Element is Marker)
            //{
            //}
        }

        private void MapControl_ElementClick(object sender, MapControlElementEventArgs e)
        {
            //if (e.Element is Polygon)
            //{
            //}
            //else if (e.Element is Marker)
            //{
            //}
        }

        private void MapControl_CenterChanged(object sender, EventArgs e)
        {
            RefreshWorkedData();
            UpdateMap();
        }

        private void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            var g = mapControl.Mouse;
            var locator = DXLogCalculators.MaidenheadLocator.LatLongToLocator(g.Latitude, g.Longitude);
            Text = locator;
        }

        private void UpdateMap()
        {

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateMap));
            }
            else
            {

                fieldGridPolygonLayer.Clear();
                fieldGridMarkerLayer.Clear();

                squareGridPolygonLayer.Clear();
                squareGridMarkerLayer.Clear();

                subsquareGridPolygonLayer.Clear();
                subsquareGridMarkerLayer.Clear();

                qsoMarkerLayer.Clear();
                spotMarkerLayer.Clear();

                if (_gridSquareSettings.ShowSubsquares)
                {
                    DrawSubsquareGrids(_gridSquareSettings.ShowSubsquaresLabel, _gridSquareSettings.DisplayContacts ? _workedStations : new List<string>());
                }

                if (_gridSquareSettings.ShowGridSquares)
                {
                    DrawSquareGrids(_gridSquareSettings.ShowGridSquaresLabel, _gridSquareSettings.ColourWorkedGridSquares ? _workedLocators : new List<string>());
                }

                if (_gridSquareSettings.ShowFields)
                {
                    DrawFieldGrids(_gridSquareSettings.ShowFieldsLabel);
                }

            }

        }

        private void RefreshWorkedData()
        {
            lock (_workedLocators)
            {
                _workedLocators.Clear();
            }
            lock (_workedStations)
            {
                _workedStations.Clear();
            }

            var activeBand = _contestData.ActiveR12Band;
            Parallel.ForEach(_contestData.QSOList.FindAll(obj => obj.Band == activeBand).ToList(), obj =>
            {
                var str = string.Empty;
                if (_contestData.activeContest.cdata.field_rcvd_type.StartsWith("GRID"))
                {
                    str = obj.Rcvd4;
                }
                else if (_contestData.activeContest.cdata.field_recinfo_type.StartsWith("GRID"))
                {
                    str = obj.RecInfo;
                }
                else if (_contestData.activeContest.cdata.field_recinfo2_type.StartsWith("GRID"))
                {
                    str = obj.RecInfo2;
                }
                else if (_contestData.activeContest.cdata.field_recinfo3_type.StartsWith("GRID"))
                {
                    str = obj.RecInfo3;
                }
                if (str == string.Empty)
                {
                    return;
                }

                lock (_workedLocators)
                {
                    if (!_workedLocators.Contains(str.Substring(0, 4)))
                    {
                        _workedLocators.Add(str.Substring(0, 4));
                    }
                }

                lock (_workedStations)
                {
                    if (_workedStations.Contains(str))
                    {
                        return;
                    }

                    _workedStations.Add(str);
                }
            });

            if (_frmMain == null)
            {
                _frmMain = (FrmMain)(ParentForm ?? Owner);
                if (_frmMain == null)
                    return;
            }

            lock (_notWorkedSpotLocators)
            {
                _notWorkedSpotLocators = _frmMain.GetLocatorsFromSpots(activeBand);
            }

        }

        private void AddWorkedLocator(DXQSO qso)
        {
            var str = string.Empty;
            if (_contestData.activeContest.cdata.field_rcvd_type.StartsWith("GRID"))
            {
                str = qso.Rcvd4;
            }
            else if (_contestData.activeContest.cdata.field_recinfo_type.StartsWith("GRID"))
            {
                str = qso.RecInfo;
            }
            else if (_contestData.activeContest.cdata.field_recinfo2_type.StartsWith("GRID"))
            {
                str = qso.RecInfo2;
            }
            else if (_contestData.activeContest.cdata.field_recinfo3_type.StartsWith("GRID"))
            {
                str = qso.RecInfo3;
            }
            lock (_workedLocators)
            {
                if (!_workedLocators.Contains(str.Substring(0, 4)))
                {
                    _workedLocators.Add(str.Substring(0, 4));
                }
            }
            lock (_workedStations)
            {
                if (_workedStations.Contains(str))
                {
                    return;
                }
                _workedStations.Add(str);
            }
        }

        public void AddSpot(string grid)
        {
            if (grid == string.Empty)
            {
                return;
            }
            lock (_notWorkedSpotLocators)
            {
                if (_notWorkedSpotLocators.Contains(grid))
                {
                    return;
                }
                _notWorkedSpotLocators.Add(grid);
            }
        }

        public void ActiveBandChanged()
        {
            RefreshWorkedData();
            UpdateMap();
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout(_normalFont);

            if (_frmMain == null)
            {
                _frmMain = (FrmMain)(ParentForm ?? Owner);
                if (_frmMain != null)
                {
                    _frmMain.NewQSOSaved += MainForm_NewQSOSaved;
                    mapControl.CenterChanged += MapControl_CenterChanged;
                }
            }

            UpdateMap();

        }

        private void MainForm_NewQSOSaved(DXQSO newQso)
        {
            AddWorkedLocator(newQso);
        }

        private void DrawFieldGrids(bool showLabels)
        {

            var fieldGridColor = getColorByType("Field Grid");

            var polygonStyle = new PolygonStyle(new SolidBrush(Color.FromArgb(0, fieldGridColor)), new Pen(fieldGridColor, 3));
            var brush = new SolidBrush(getColorByType("Field Text"));
            var markerStyle = new MarkerStyle(null, 0, null, null, brush, new Font(FontFamily.GenericMonospace, 40f, FontStyle.Bold), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            var unit = 10f;

            var e = mapControl.TopLeft.Longitude;
            var w = mapControl.TopRight.Longitude;
            var n = mapControl.TopLeft.Latitude;
            var s = mapControl.BottomRight.Latitude;

            if (n > 85)
            {
                n = 85;
            }
            if (s < -85)
            {
                s = -85;
            }

            var left = (float)(Math.Floor(e / (unit * 2)) * (unit * 2));
            var right = (float)(Math.Ceiling(w / (unit * 2)) * (unit * 2));
            var top = (float)(Math.Ceiling(n / unit) * unit);
            var bottom = (float)(Math.Floor(s / unit) * unit);

            for (var lon = left; lon < right; lon += (unit * 2))
            {
                for (var lat = bottom; lat < top; lat += unit)
                {
                    var poly = new Polygon(polygonStyle)
                        {
                            new GeoPoint(lon, lat),
                            new GeoPoint(lon, lat + unit),
                            new GeoPoint(lon + (unit * 2), lat + unit),
                            new GeoPoint(lon + (unit * 2), lat)
                        };
                    fieldGridPolygonLayer.AddPolygon(poly);

                    if (showLabels && mapControl.ZoomLevel < 7)
                    {
                        var labelGeoPoint = new GeoPoint(lon + unit, lat + (unit / 2));
                        var locator = DXLogCalculators.MaidenheadLocator.LatLongToLocator(labelGeoPoint.Latitude, labelGeoPoint.Longitude).Remove(2);
                        var locatorText = new Marker(labelGeoPoint, markerStyle, locator);
                        fieldGridMarkerLayer.AddMarker(locatorText);
                    }

                }
            }

        }

        private void DrawSquareGrids(bool showLabels, IEnumerable<string> locators)
        {

            var unit = 1f;

            var e = mapControl.TopLeft.Longitude;
            var w = mapControl.TopRight.Longitude;
            var n = mapControl.TopLeft.Latitude;
            var s = mapControl.BottomRight.Latitude;

            if (n > 85)
            {
                n = 85;
            }
            if (s < -85)
            {
                s = -85;
            }

            var left = (float)(Math.Floor(e / (unit * 2)) * (unit * 2));
            var right = (float)(Math.Ceiling(w / (unit * 2)) * (unit * 2));
            var top = (float)(Math.Ceiling(n / unit) * unit);
            var bottom = (float)(Math.Floor(s / unit) * unit);

            var brush = new SolidBrush(Color.Red);
            var markerStyle = new MarkerStyle(null, 0, null, null, new SolidBrush(Color.Red), new Font(FontFamily.GenericMonospace, 30f, FontStyle.Bold), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            var pen = new Pen(mapControl.ZoomLevel < 6 ? Color.DarkGray : Color.DarkSlateGray, mapControl.ZoomLevel < 6 ? 1 : 2);

            var normalLocatorStyle = new PolygonStyle(new SolidBrush(Color.FromArgb(0, Color.Black)), pen);
            var workedLocatorStyle = new PolygonStyle(new SolidBrush(Color.FromArgb(60, Color.Black)), pen);

            for (var lon = left; lon < right; lon += (unit * 2))
            {
                for (var lat = bottom; lat < top + unit; lat += unit)
                {

                    var labelGeoPoint = new GeoPoint(lon + unit, lat - (unit / 2));
                    var locator = DXLogCalculators.MaidenheadLocator.LatLongToLocator(labelGeoPoint.Latitude, labelGeoPoint.Longitude).Remove(4);

                    var poly = new Polygon(locators.Any(x => x == locator) && mapControl.ZoomLevel < 10 ? workedLocatorStyle : normalLocatorStyle)
                    {
                        new GeoPoint(lon, lat),
                        new GeoPoint(lon, lat - unit),
                        new GeoPoint(lon + (unit * 2), lat - unit),
                        new GeoPoint(lon + (unit * 2), lat)
                    };

                    squareGridPolygonLayer.AddPolygon(poly);

                    if (showLabels && mapControl.ZoomLevel > 6 && mapControl.ZoomLevel < 11)
                    {
                        var locatorText = new Marker(labelGeoPoint, markerStyle, locator);
                        squareGridMarkerLayer.AddMarker(locatorText);
                    }

                }
            }

        }

        private void DrawSubsquareGrids(bool showLabels, IEnumerable<string> locators)
        {

            var normalLocatorStyle = new PolygonStyle(new SolidBrush(Color.FromArgb(0, Color.Black)), Pens.Black);
            var workedLocatorStyle = new PolygonStyle(new SolidBrush(Color.FromArgb(100, Color.Red)), Pens.Red);

            var brush = new SolidBrush(Color.Red);
            var markerStyle = new MarkerStyle(null, 0, null, null, brush, new Font(FontFamily.GenericMonospace, 30f, FontStyle.Bold), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            var workedMarkerStyle = new MarkerStyle(null, 0, null, null, new SolidBrush(Color.Black), new Font(FontFamily.GenericMonospace, 30f, FontStyle.Bold), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            var unit = (float)1 / 24;

            if (mapControl.ZoomLevel <= 10)
            {
                // Only draw locators we've worked.

                foreach (var locator in locators)
                {
                    var loc = DXLogCalculators.MaidenheadLocator.LocatorToLatLong(locator);

                    var latRng = 0.0416666666f;
                    var lonRng = 0.0833333333f;
                    var boxLat = (float)(Math.Floor(loc.Lat / latRng) * latRng);
                    var boxLon = (float)(Math.Floor(loc.Long / lonRng) * lonRng);

                    var poly = new Polygon(locators.Contains(locator) ? workedLocatorStyle : normalLocatorStyle)
                    {
                        new GeoPoint(boxLon, boxLat),
                        new GeoPoint(boxLon, boxLat + latRng),
                        new GeoPoint(boxLon + lonRng , boxLat + latRng),
                        new GeoPoint(boxLon + lonRng, boxLat)
                    };

                    subsquareGridPolygonLayer.AddPolygon(poly);

                    if (mapControl.ZoomLevel == 10)
                    {
                        var labelGeoPoint = new GeoPoint((float)(loc.Long), (float)(loc.Lat - 0.004));
                        var locatorText = new Marker(labelGeoPoint, locators.Contains(locator) ? workedMarkerStyle : markerStyle, locator.Remove(0, 4));
                        subsquareGridMarkerLayer.AddMarker(locatorText);
                    }

                }

                return;
            }

            var e = mapControl.TopLeft.Longitude;
            var w = mapControl.TopRight.Longitude;
            var n = mapControl.TopLeft.Latitude;
            var s = mapControl.BottomRight.Latitude;

            if (n > 85)
            {
                n = 85;
            }
            if (s < -85)
            {
                s = -85;
            }

            var left = (float)(Math.Floor(e / (unit * 2)) * (unit * 2));
            var right = (float)(Math.Ceiling(w / (unit * 2)) * (unit * 2));
            var top = (float)(Math.Ceiling(n / unit) * unit);
            var bottom = (float)(Math.Floor(s / unit) * unit);


            for (var lon = left; lon < right; lon += (unit * 2))
            {
                for (var lat = bottom; lat < top; lat += unit)
                {

                    var labelGeoPoint = new GeoPoint(lon + unit, lat + (unit / 2));
                    var locator = DXLogCalculators.MaidenheadLocator.LatLongToLocator(labelGeoPoint.Latitude, labelGeoPoint.Longitude);

                    var poly = new Polygon(locators.Contains(locator) ? workedLocatorStyle : normalLocatorStyle)
                    {
                        new GeoPoint(lon, lat),
                        new GeoPoint(lon, lat + unit),
                        new GeoPoint(lon + (unit * 2), lat + unit),
                        new GeoPoint(lon + (unit * 2), lat)
                    };

                    if (mapControl.ZoomLevel > 10)
                    {

                        subsquareGridPolygonLayer.AddPolygon(poly);

                        var locatorText = new Marker(labelGeoPoint, locators.Contains(locator) ? workedMarkerStyle : markerStyle, mapControl.ZoomLevel == 11 ? locator.Remove(0, 4) : locator);
                        subsquareGridMarkerLayer.AddMarker(locatorText);

                    }
                }
            }

        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new GridSquareProperties(_gridSquareSettings);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadConfiguration();
                UpdateMap();
            }
        }

        void LoadConfiguration()
        {
            try
            {
                _gridSquareSettings.CentreMapOnLocator = Config.Read(nameof(GridSquareSettings.CentreMapOnLocator), new GridSquareSettings().CentreMapOnLocator).ToUpper();
                _gridSquareSettings.CentreMapOnQth = Config.Read(nameof(GridSquareSettings.CentreMapOnQth), new GridSquareSettings().CentreMapOnQth);
                _gridSquareSettings.ColourWorkedGridSquares = Config.Read(nameof(GridSquareSettings.ColourWorkedGridSquares), new GridSquareSettings().ColourWorkedGridSquares);
                _gridSquareSettings.DisplayContacts = Config.Read(nameof(GridSquareSettings.DisplayContacts), new GridSquareSettings().DisplayContacts);
                _gridSquareSettings.DisplaySpots = Config.Read(nameof(GridSquareSettings.DisplaySpots), new GridSquareSettings().DisplaySpots);
                _gridSquareSettings.MapSourceProvider = Config.Read(nameof(GridSquareSettings.MapSourceProvider), new GridSquareSettings().MapSourceProvider);
                _gridSquareSettings.MaxZoom = Config.Read(nameof(GridSquareSettings.MaxZoom), new GridSquareSettings().MaxZoom);
                _gridSquareSettings.MinZoom = Config.Read(nameof(GridSquareSettings.MinZoom), new GridSquareSettings().MinZoom);
                _gridSquareSettings.ShowFields = Config.Read(nameof(GridSquareSettings.ShowFields), new GridSquareSettings().ShowFields);
                _gridSquareSettings.ShowFieldsLabel = Config.Read(nameof(GridSquareSettings.ShowFieldsLabel), new GridSquareSettings().ShowFieldsLabel);
                _gridSquareSettings.ShowGridSquares = Config.Read(nameof(GridSquareSettings.ShowGridSquares), new GridSquareSettings().ShowGridSquares);
                _gridSquareSettings.ShowGridSquaresLabel = Config.Read(nameof(GridSquareSettings.ShowGridSquaresLabel), new GridSquareSettings().ShowGridSquaresLabel);
                _gridSquareSettings.ShowSubsquares = Config.Read(nameof(GridSquareSettings.ShowSubsquares), new GridSquareSettings().ShowSubsquares);
                _gridSquareSettings.ShowSubsquaresLabel = Config.Read(nameof(GridSquareSettings.ShowSubsquaresLabel), new GridSquareSettings().ShowSubsquaresLabel);
                _gridSquareSettings.StartZoom = Config.Read(nameof(GridSquareSettings.StartZoom), new GridSquareSettings().StartZoom);
                _gridSquareSettings.ZoomToQsos = Config.Read(nameof(GridSquareSettings.ZoomToQsos), new GridSquareSettings().ZoomToQsos);
            }
            catch
            {
                // Settings were corrupted.  Load defaults.
                _gridSquareSettings = new GridSquareSettings();
            }

        }

        private void clearCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapControl.ClearCache(true);
            ActiveControl = mapControl;
        }
    }

}
