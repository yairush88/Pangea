using CommunityToolkit.Mvvm.ComponentModel;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using Pangea.Core.Model;
using Pangea.Core.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Pangea.Adapters.Esri
{
    public class Map2D : ObservableObject, IMap2D
    {
        private MapView _mapView;
        private GraphicsOverlay _mainOverlay;
        private SimpleLineSymbol _simpleLineSymbol;
        private SimpleFillSymbol _simpleFillSymbol;
        private Dictionary<SketchMode, SketchCreationMode> _sketchCreationModes;

        private const string IdAttributeName = "Id";
        private const double SelectGraphicTolerance = 2;

        private bool _isMovingGraphic = false;

        public SketchEditor SketchEditor => _mapView?.SketchEditor;

        public Map2D()
        {
            _mainOverlay = new GraphicsOverlay();

            InitSymbols();
            InitSketchCreationModes();
            GraphicsOverlays = new GraphicsOverlayCollection
            {
                _mainOverlay,
            };
        }

        private void InitSketchEditor()
        {
            SketchEditor.GeometryChanged += SketchEditor_GeometryChanged;
        }

        private void InitSketchCreationModes()
        {
            _sketchCreationModes = new Dictionary<SketchMode, SketchCreationMode>
            {
                [SketchMode.Point] = SketchCreationMode.Point,
                [SketchMode.Polyline] = SketchCreationMode.Polyline,
                [SketchMode.Polygon] = SketchCreationMode.Polygon,
                [SketchMode.Arrow] = SketchCreationMode.Arrow,
                [SketchMode.Circle] = SketchCreationMode.Circle,
                [SketchMode.Ellipse] = SketchCreationMode.Ellipse,
                [SketchMode.Rectangle] = SketchCreationMode.Rectangle,
                [SketchMode.Triangle] = SketchCreationMode.Triangle,
                [SketchMode.FreehandLine] = SketchCreationMode.FreehandLine,
                [SketchMode.FreehandPolygon] = SketchCreationMode.FreehandPolygon,
                [SketchMode.Multipoint] = SketchCreationMode.Multipoint
            };
        }

        private void InitSymbols()
        {
            _simpleLineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, Color.Blue, 5);
            _simpleFillSymbol = new SimpleFillSymbol(SimpleFillSymbolStyle.Solid, Color.FromArgb(100, Color.Red), _simpleLineSymbol);
        }

        private Map _map;
        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        private GraphicsOverlayCollection _graphicsOverlays;
        public GraphicsOverlayCollection GraphicsOverlays
        {
            get => _graphicsOverlays;
            set => _graphicsOverlays = value;
        }

        public void SetMapView(object mapView)
        {
            _mapView = mapView as MapView;
            InitSketchEditor();
        }

        public async Task LoadMap(Uri source)
        {
            var layer = new ArcGISTiledLayer(source);
            await layer.LoadAsync();
            //Map = new Map(new Basemap(layer));

            // For testing with online maps, remove later...
            Map = new Map(BasemapStyle.ArcGISTopographic) { ReferenceScale = 10000 };
        }

        public async Task SetViewpoint(double latitude, double longitude, double scale)
        {
            await _mapView.SetViewpointAsync(new Viewpoint(latitude, longitude, scale));
        }

        public void AddPolyline(string Id, IEnumerable<GeoLocation> points)
        {
            var polyline = new Polyline(GetMapPoints(points));
            var graphic = new Graphic(polyline, CreateAttributes(Id), _simpleLineSymbol);
            _mainOverlay.Graphics.Add(graphic);
        }

        public void AddPolygon(string Id, IEnumerable<GeoLocation> points)
        {
            var polygon = new Polygon(GetMapPoints(points));
            var graphic = new Graphic(polygon, CreateAttributes(Id), _simpleLineSymbol);
            _mainOverlay.Graphics.Add(graphic);
        }

        public void AddEllipse(string Id, GeoLocation centerPoint, double width, double height)
        {
            var parameters = new GeodesicEllipseParameters
            {
                Center = GetMapPoint(centerPoint),
                GeometryType = GeometryType.Polygon,
                SemiAxis1Length = height,
                SemiAxis2Length = width,
                AxisDirection = 0,
                MaxPointCount = 200,
                AngularUnit = AngularUnits.Degrees,
                LinearUnit = LinearUnits.Meters,
                MaxSegmentLength = 50
            };

            var ellipse = (Polygon)GeometryEngine.EllipseGeodesic(parameters);
            var graphic = new Graphic(ellipse, CreateAttributes(Id), _simpleFillSymbol);
            _mainOverlay.Graphics.Add(graphic);
        }

        public void AddEntityGroup(MapEntityGroup entityGroup)
        {
            var parentEntity = entityGroup.Entities.FirstOrDefault(x => x.IsParent);
            var compositeSymbol = new CompositeSymbol();
            foreach (var entity in entityGroup.Entities)
            {
                var symbol = CreatePictureMarkerSymbol(entity);
                compositeSymbol.Symbols.Add(symbol);
            }

            TextSymbol textSymbol = new TextSymbol("Yair", Color.White, 16, HorizontalAlignment.Center, VerticalAlignment.Middle)
            {
                BackgroundColor = Color.Transparent,
                FontWeight = FontWeight.Bold,
                OutlineColor = Color.Black,
                OutlineWidth = 1,
                OffsetX = 30,
            };

            compositeSymbol.Symbols.Add(textSymbol);
            AddEntityToOverlay(parentEntity, compositeSymbol);
        }

        //public void AddEntityGroup(MapEntityGroup entityGroup)
        //{
        //    var parentEntity = entityGroup.Entities.FirstOrDefault(x => x.IsParent);
        //    var compositeSymbol = new CompositeSymbol();
        //    foreach (var entity in entityGroup.Entities)
        //    {
        //        var symbol = CreatePictureMarkerSymbol(entity);
        //        compositeSymbol.Symbols.Add(symbol);
        //    }

        //    AddEntityToOverlay(parentEntity, compositeSymbol);
        //}

        public void AddEntity(MapEntity entity)
        {
            var symbol = new PictureMarkerSymbol(entity.Source)
            {
                Width = entity.Width,
                Height = entity.Height,
                Angle = entity.Angle,
            };

            AddEntityToOverlay(entity, symbol);
        }

        public void RemoveSelectedEntities()
        {
            foreach (var graphic in _mainOverlay.SelectedGraphics)
            {
                _mainOverlay.Graphics.Remove(graphic);
            }

            CancelSketch();
        }


        private static MapPoint GetMapPoint(GeoLocation centerPoint)
        {
            return new MapPoint(centerPoint.Longitude, centerPoint.Latitude, SpatialReferences.Wgs84);
        }

        private static IEnumerable<MapPoint> GetMapPoints(IEnumerable<GeoLocation> points)
        {
            return points.Select(p => new MapPoint(p.Longitude, p.Latitude, SpatialReferences.Wgs84)).ToList();
        }

        private static Dictionary<string, object> CreateAttributes(MapEntity mapEntity)
        {
            return new Dictionary<string, object>
            {
                { IdAttributeName, mapEntity.Id },
                { "IsSelectable", mapEntity.IsSelectable }
            };
        }

        private static Dictionary<string, object> CreateAttributes(string Id, bool isSelectable = true)
        {
            return new Dictionary<string, object> { { IdAttributeName, Id } };
        }

        private static PictureMarkerSymbol CreatePictureMarkerSymbol(MapEntity entity)
        {
            var symbol = new PictureMarkerSymbol(entity.Source)
            {
                Width = entity.Width,
                Height = entity.Height,
                Angle = entity.Angle,
                OffsetX = entity.OffsetX,
                OffsetY = entity.OffsetY
            };

            return symbol;
        }

        private void AddEntityToOverlay(MapEntity parentEntity, Symbol symbol)
        {
            var graphic = new Graphic(GetMapPoint(parentEntity.Location), CreateAttributes(parentEntity), symbol);
            _mainOverlay.Graphics.Add(graphic);
        }

        // ****** EVENTS ******************************************************************

        public async void OnGeoViewTapped(object arguments)
        {
            var geoViewInputArgs = arguments as GeoViewInputEventArgs;
            if (geoViewInputArgs != null)
            {
                await TrySelectEntity(geoViewInputArgs);
            }
        }

        private async Task TrySelectEntity(GeoViewInputEventArgs argument)
        {
            const int maximumResults = 1; // Only return one graphic
            bool onlyReturnPopups = false; // Return more than popups

            try
            {
                // Use the following method to identify graphics in a specific graphics overlay
                IdentifyGraphicsOverlayResult identifyResults = await _mapView.IdentifyGraphicsOverlayAsync(
                    _mainOverlay,
                    argument.Position,
                    SelectGraphicTolerance,
                    onlyReturnPopups,
                    maximumResults);

                _mainOverlay.ClearSelection();
                var graphic = identifyResults?.Graphics?.FirstOrDefault();
                if (graphic != null && (bool)graphic.Attributes["IsSelectable"])
                {
                    graphic.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                // TODO: Log...
            }
        }

        public async Task StartSketch(SketchMode sketchMode, bool drawAndEdit = true)
        {
            try
            {
                // Let the user draw on the map view using the chosen sketch mode.
                var config = new SketchEditConfiguration { RequireSelectionBeforeDrag = false, AllowMove = true, AllowRotate = true };
                var geometry = await SketchEditor.StartAsync(_sketchCreationModes[sketchMode], config);
                var graphic = CreateGraphic(geometry);
                if (graphic != null)
                {
                    _mainOverlay.Graphics.Add(graphic);
                }
            }
            catch (TaskCanceledException)
            {
                // Ignore ... let the user cancel drawing.
            }
            catch (Exception ex)
            {
                // TODO: Log...
            }
        }

        public async Task EditSketch()
        {
            // Allow the user to select a graphic.
            var graphic = await SelectGraphic();

            if (graphic != null)
            {
                var newGeometry = await SketchEditor.StartAsync(graphic.Geometry);
                graphic.Geometry = newGeometry;
            }
        }

        public async Task MoveEntity()
        {
            // Allow the user to select a graphic.
            var graphic = await SelectGraphic();

            if (graphic != null)
            {
                var geometry = GeometryEngine.Project(graphic.Geometry, SpatialReferences.WebMercator);
                var config = new SketchEditConfiguration { RequireSelectionBeforeDrag = false, AllowMove = true };

                _isMovingGraphic = true;
                // Let the user make changes to the graphic's geometry, await the result (updated geometry).
                var newGeometry = await SketchEditor.StartAsync(geometry, SketchCreationMode.Point, config);

                // Display the updated geometry in the graphic.
                graphic.Geometry = newGeometry;
            }
        }

        public bool RemoveEntity(string Id)
        {
            var graphic = _mainOverlay.Graphics.FirstOrDefault(x => x.Attributes[IdAttributeName].ToString() == Id);
            var graphicRemoved = false;
            if (graphic != null)
            {
                //_mainOverlay.Graphics.Remove(graphic);
                graphicRemoved = true;
            }

            return graphicRemoved;
        }


        private void SketchEditor_GeometryChanged(object sender, GeometryChangedEventArgs e)
        {
            if (_isMovingGraphic)
            {
                CompleteSketch();
                _isMovingGraphic = false;
            }
        }

        private async Task<Graphic> SelectGraphic()
        {
            // Wait for the user to click a location on the map.
            var mapPoint = await SketchEditor.StartAsync(SketchCreationMode.Point, false);

            // Convert the map point to a screen point.
            var screenCoordinate = _mapView.LocationToScreen((MapPoint)mapPoint);

            var results = await _mapView.IdentifyGraphicsOverlaysAsync(screenCoordinate, SelectGraphicTolerance, false);

            Graphic graphic = null;
            var idResult = results.FirstOrDefault();
            if (idResult != null && idResult.Graphics.Any())
            {
                graphic = idResult.Graphics.FirstOrDefault();
                graphic.IsSelected = true;
            }

            return graphic;
        }

        private Graphic CreateGraphic(Geometry geometry)
        {
            if (geometry != null)
            {
                var graphic = new Graphic(geometry, _simpleFillSymbol);
                graphic.Attributes.Add(new KeyValuePair<string, object>("Id", null));
                graphic.Attributes.Add(new KeyValuePair<string, object>("IsSelectable", true));
                return graphic;
            }

            return null;
        }

        public void CompleteSketch()
        {
            if (SketchEditor.CompleteCommand.CanExecute(null))
            {
                SketchEditor.CompleteCommand.Execute(null);
            }

            //_mainOverlay.ClearSelection();
        }

        public void CancelSketch()
        {
            if (SketchEditor.CancelCommand.CanExecute(null))
            {
                SketchEditor.CancelCommand.Execute(null);
            }

            _mainOverlay.ClearSelection();
        }
    }
}