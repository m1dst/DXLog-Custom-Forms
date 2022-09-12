using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Maps.Common;
using System.Windows.Forms.Maps.Elements;
using System.Windows.Forms.Maps.Layers;

namespace DXLog.net
{
    public partial class GridSquares : KForm
    {
        public static string CusWinName => "Grid Squares";
        public static int CusFormID => 20220911;

        private ContestData _contestData;
        private FrmMain _frmMain;

        private Font _normalFont = new Font("Courier New", 10, FontStyle.Regular);
        private Font _boldFont = new Font("Courier New", 10, FontStyle.Bold);

        private List<string> _workedLocators = new List<string>();
        private List<string> _workedStations = new List<string>();
        private List<string> _notWorkedSpotLocators = new List<string>();

        private GridSquareSettings _gridSquareSettings = new GridSquareSettings();

        private void handle_FormLayoutChangeEvent() => InitializeLayout();

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
            this.FormID = CusFormID;

            mapControl.CacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MapControl");
            mapControl.MinZoomLevel = 4;
            mapControl.MaxZoomLevel = 14;
            mapControl.ZoomLevel = 4;

        }

        public GridSquares(ContestData contestData)
        {
            InitializeComponent();
            this.FormID = CusFormID;

            mapControl.CacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MapControl");
            mapControl.MinZoomLevel = 4;
            mapControl.MaxZoomLevel = 14;
            mapControl.ZoomLevel = 4;
            mapControl.TileServer = new OpenStreetMapTileServer(userAgent: "DXLog");

            _contestData = contestData;

            // group layers together
            fieldGridLayerGroup.AddLayer(fieldGridMarkerLayer);
            fieldGridLayerGroup.AddLayer(fieldGridPolygonLayer);

            squareGridLayerGroup.AddLayer(squareGridMarkerLayer);
            squareGridLayerGroup.AddLayer(squareGridPolygonLayer);

            subsquareGridLayerGroup.AddLayer(subsquareGridMarkerLayer);
            subsquareGridLayerGroup.AddLayer(subsquareGridPolygonLayer);

            // add layers to map
            mapControl.AddLayer(subsquareGridLayerGroup);
            mapControl.AddLayer(squareGridLayerGroup);
            mapControl.AddLayer(fieldGridLayerGroup);

            mapControl.CenterChanged += MapControl_CenterChanged;
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

            RefreshWorkedData();
            UpdateMap();

        }

        private void Handle_FormLayoutChangeEvent()
        {
            InitializeLayout();
        }

        private void MapControl_ElementLeave(object sender, MapControlElementEventArgs e)
        {
            if (e.Element is Polygon)
            {
            }
            else if (e.Element is Marker)
            {
            }
        }

        private void MapControl_ElementEnter(object sender, MapControlElementEventArgs e)
        {
            if (e.Element is Polygon)
            {
            }
            else if (e.Element is Marker)
            {
            }
        }

        private void MapControl_ElementClick(object sender, MapControlElementEventArgs e)
        {
            if (e.Element is Polygon)
            {
            }
            else if (e.Element is Marker)
            {
            }

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

        private void mapControl_MouseWheel(object sender, MouseEventArgs e)
        {
            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            GeoPoint g = mapControl.Mouse;
            var locator = DXLogCalculators.MaidenheadLocator.LatLongToLocator(g.Latitude, g.Longitude);
            Text = $"{locator}";
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

                DrawSubsquareGrids(true, _workedStations);
                DrawSquareGrids(true, _workedLocators);
                DrawFieldGrids(true);


                //chart1.Series["Azimuth"].Points.Clear();
                //foreach (var nameGroup in groupByAzimuthQuery)
                //{
                //    //chart1.Series["Azimuth"].Points.AddXY(nameGroup.Key, nameGroup.Count());
                //}
            }

        }


        public void RefreshWorkedData()
        {
            this._workedLocators.Clear();
            this._workedStations.Clear();
            string aband = _contestData.ActiveR12Band;
            Parallel.ForEach<DXQSO>((IEnumerable<DXQSO>)_contestData.QSOList.FindAll((Predicate<DXQSO>)(obj => obj.Band == aband)).ToList<DXQSO>(), (Action<DXQSO>)(obj =>
            {
                string str = string.Empty;
                if (_contestData.activeContest.cdata.field_rcvd_type.StartsWith("GRID"))
                    str = obj.Rcvd4;
                else if (_contestData.activeContest.cdata.field_recinfo_type.StartsWith("GRID"))
                    str = obj.RecInfo;
                else if (_contestData.activeContest.cdata.field_recinfo2_type.StartsWith("GRID"))
                    str = obj.RecInfo2;
                else if (_contestData.activeContest.cdata.field_recinfo3_type.StartsWith("GRID"))
                    str = obj.RecInfo3;
                if (!(str != string.Empty))
                    return;
                
                //if (this.mapProperties.optColorizeWorked)
                {
                    lock (this._workedLocators)
                    {
                        if (!this._workedLocators.Contains(str.Substring(0, 4)))
                            this._workedLocators.Add(str.Substring(0, 4));
                    }
                }
                //if (!this.mapProperties.optShowContacts)
                //    return;
                lock (this._workedStations)
                {
                    if (this._workedStations.Contains(str))
                        return;
                    this._workedStations.Add(str);
                }
            }));
            FrmMain frmMain = this.ParentForm == null ? (FrmMain)this.Owner : (FrmMain)this.ParentForm;
            if (frmMain == null)
                return;
            this._notWorkedSpotLocators = frmMain.GetLocatorsFromSpots(aband);
        }

        public void AddWorkedLocator(DXQSO qso)
        {
            string str = string.Empty;
            if (_contestData.activeContest.cdata.field_rcvd_type.StartsWith("GRID"))
                str = qso.Rcvd4;
            else if (_contestData.activeContest.cdata.field_recinfo_type.StartsWith("GRID"))
                str = qso.RecInfo;
            else if (_contestData.activeContest.cdata.field_recinfo2_type.StartsWith("GRID"))
                str = qso.RecInfo2;
            else if (_contestData.activeContest.cdata.field_recinfo3_type.StartsWith("GRID"))
                str = qso.RecInfo3;
            lock (this._workedLocators)
            {
                if (!this._workedLocators.Contains(str.Substring(0, 4)))
                    this._workedLocators.Add(str.Substring(0, 4));
            }
            lock (this._workedStations)
            {
                if (this._workedStations.Contains(str))
                    return;
                this._workedStations.Add(str);
            }
        }

        public void AddSpot(string grid)
        {
            if (!(grid != string.Empty))
                return;
            lock (this._notWorkedSpotLocators)
            {
                if (this._notWorkedSpotLocators.Contains(grid))
                    return;
                this._notWorkedSpotLocators.Add(grid);
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

            //chart1.BackColor = getColorByType("Background");
            //chart1.Series[0].Color = getColorByType("Line");
            //
            //chart1.ChartAreas[0].BackColor = getColorByType("Background");
            //
            //chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX.MajorTickMark.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX.MinorGrid.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX.MinorTickMark.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX.LabelStyle.Font = _normalFont;
            //
            //chart1.ChartAreas[0].AxisX2.LabelStyle.ForeColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX2.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX2.MajorGrid.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX2.MajorTickMark.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX2.MinorGrid.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX2.MinorTickMark.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisX2.LabelStyle.Font = _normalFont;
            //
            //chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY.MajorTickMark.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY.MinorGrid.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY.MinorTickMark.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY.LabelStyle.Font = _normalFont;
            //
            //chart1.ChartAreas[0].AxisY2.LabelStyle.ForeColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY2.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY2.MajorTickMark.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY2.MinorGrid.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY2.MinorTickMark.LineColor = getColorByType("Grid");
            //chart1.ChartAreas[0].AxisY2.LabelStyle.Font = _normalFont;
            //
            //if (base.FormLayout.FontName.Contains("Courier"))
            //{
            //    _normalFont = new Font(base.FormLayout.FontName, base.FormLayout.FontSize, FontStyle.Regular);
            //    _boldFont = new Font(base.FormLayout.FontName, base.FormLayout.FontSize, FontStyle.Bold);
            //}
            //else
            //{
            //    _normalFont = Helper.GetSpecialFont(FontStyle.Regular, base.FormLayout.FontSize);
            //    _boldFont = Helper.GetSpecialFont(FontStyle.Bold, base.FormLayout.FontSize);
            //}

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
            AddWorkedLocator(newQso);
        }

        private void DrawFieldGrids(bool showLabels)
        {

            var polygonStyle = new PolygonStyle(new SolidBrush(Color.FromArgb(0, Color.Blue)), new Pen(Color.Blue, 3));
            var brush = new SolidBrush(Color.Red);
            var markerStyle = new MarkerStyle(null, 0, null, null, brush, new Font(FontFamily.GenericMonospace, 40f, FontStyle.Bold), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            var unit = 10f;

            var e = mapControl.TopLeft.Longitude;
            var w = mapControl.TopRight.Longitude;
            var n = mapControl.TopLeft.Latitude;
            var s = mapControl.BottomRight.Latitude;

            if (n > 85) n = 85;
            if (s < -85) s = -85;

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

            if (n > 85) n = 85;
            if (s < -85) s = -85;

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

            if (n > 85) n = 85;
            if (s < -85) s = -85;

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
            Form prop = new GridSquareProperties(_gridSquareSettings);
            if (prop.ShowDialog() == DialogResult.OK)
            {
                GetConfig(false);
            }
        }

        void GetConfig(bool all)
        {
            try
            {
                //Set.LowerEdgeCW = Config.Read("WaterfallLowerEdgeCW", Def.LowerEdgeCW).Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.UpperEdgeCW = Config.Read("WaterfallUpperEdgeCW", Def.UpperEdgeCW).Split(';').Select(s => int.Parse(s)).ToArray();

                //Set.LowerEdgePhone = Config.Read("WaterfallLowerEdgePhone", Def.LowerEdgePhone).Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.UpperEdgePhone = Config.Read("WaterfallUpperEdgePhone", Def.UpperEdgePhone).Split(';').Select(s => int.Parse(s)).ToArray();

                //Set.LowerEdgeDigital = Config.Read("WaterfallLowerEdgeDigital", Def.LowerEdgeDigital).Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.UpperEdgeDigital = Config.Read("WaterfallUpperEdgeDigital", Def.UpperEdgeDigital).Split(';').Select(s => int.Parse(s)).ToArray();

                //Set.EdgeSet = Config.Read("WaterfallEdgeSet", Def.EdgeSet);
                //Set.Scrolling = Config.Read("WaterfallScrolling", Def.UseScrolling);

                //if (all)
                //{
                //    Set.RefLevelCW = Config.Read("WaterfallRefCW", Def.RefLevelCW).Split(';').Select(s => int.Parse(s)).ToArray();
                //    Set.RefLevelPhone = Config.Read("WaterfallRefPhone", Def.RefLevelPhone).Split(';').Select(s => int.Parse(s)).ToArray();
                //    Set.RefLevelDigital = Config.Read("WaterfallRefDigital", Def.RefLevelDigital).Split(';').Select(s => int.Parse(s)).ToArray();

                //    Set.PwrLevelCW = Config.Read("TransmitPowerCW", Def.PwrLevelCW).Split(';').Select(s => int.Parse(s)).ToArray();
                //    Set.PwrLevelPhone = Config.Read("TransmitPowerPhone", Def.PwrLevelPhone).Split(';').Select(s => int.Parse(s)).ToArray();
                //    Set.PwrLevelDigital = Config.Read("TransmitPowerDigital", Def.PwrLevelDigital).Split(';').Select(s => int.Parse(s)).ToArray();
                //}
            }
            catch
            {
                // Settings are somehow corrupted. Reset everything to default.
                //Set.LowerEdgeCW = Def.LowerEdgeCW.Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.UpperEdgeCW = Def.UpperEdgeCW.Split(';').Select(s => int.Parse(s)).ToArray();

                //Set.LowerEdgePhone = Def.LowerEdgePhone.Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.UpperEdgePhone = Def.UpperEdgePhone.Split(';').Select(s => int.Parse(s)).ToArray();

                //Set.LowerEdgeDigital = Def.LowerEdgeDigital.Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.UpperEdgeDigital = Def.UpperEdgeDigital.Split(';').Select(s => int.Parse(s)).ToArray();

                //Set.EdgeSet = Def.EdgeSet;
                //Set.Scrolling = Def.UseScrolling;

                //Set.RefLevelCW = Def.RefLevelCW.Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.RefLevelPhone = Def.RefLevelPhone.Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.RefLevelDigital = Def.RefLevelDigital.Split(';').Select(s => int.Parse(s)).ToArray();

                //Set.PwrLevelPhone = Def.PwrLevelPhone.Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.PwrLevelCW = Def.PwrLevelCW.Split(';').Select(s => int.Parse(s)).ToArray();
                //Set.PwrLevelDigital = Def.PwrLevelDigital.Split(';').Select(s => int.Parse(s)).ToArray();
            }
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            //Config.Save("WaterfallRefCW", string.Join(";", Set.RefLevelCW.Select(i => i.ToString()).ToArray()));
            //Config.Save("WaterfallRefPhone", string.Join(";", Set.RefLevelPhone.Select(i => i.ToString()).ToArray()));
            //Config.Save("WaterfallRefDigital", string.Join(";", Set.RefLevelDigital.Select(i => i.ToString()).ToArray()));

            //Config.Save("TransmitPowerCW", string.Join(";", Set.PwrLevelCW.Select(i => i.ToString()).ToArray()));
            //Config.Save("TransmitPowerPhone", string.Join(";", Set.PwrLevelPhone.Select(i => i.ToString()).ToArray()));
            //Config.Save("TransmitPowerDigital", string.Join(";", Set.PwrLevelDigital.Select(i => i.ToString()).ToArray()));

            //mainForm.scheduler.Second -= UpdateRadio;
            //_cdata.ActiveVFOChanged -= UpdateRadio;
            //_cdata.ActiveRadioBandChanged -= UpdateRadio;
        }
    }

}
