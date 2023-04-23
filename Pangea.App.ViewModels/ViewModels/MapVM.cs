using CommunityToolkit.Mvvm.Input;
using Pangea.Core.Model;
using Pangea.Core.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pangea.App.ViewModels
{
    public class MapVM : ViewModelBase, IMapVM
    {
        private GeoLocation _viewpoint;
        
        public MapVM(IMap2D map)
        {
            Map = map;
            _viewpoint = new GeoLocation(34.0006, -118.8066);
        }

        public ICommand StartSketchCommand { get; set; }
        public ICommand EditSketchCommand { get; set; }
        public ICommand CompleteSketchCommand { get; set; }
        public ICommand CancelSketchCommand { get; set; }
        public ICommand MoveEntityCommand { get; set; }
        public ICommand RemoveSelectedEntitiesCommand { get; set; }

        private IMap2D _map;
        public IMap2D Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        protected override void SetupCommands()
        {
            StartSketchCommand = new RelayCommand(OnStartSketch);
            EditSketchCommand = new RelayCommand(OnEditSketch);
            CompleteSketchCommand = new RelayCommand(OnCompleteSketch);
            CancelSketchCommand = new RelayCommand(OnCancelSketch);
            MoveEntityCommand = new RelayCommand(OnMoveEntity);
            RemoveSelectedEntitiesCommand = new RelayCommand(OnRemoveSelectedEntities);
        }

        private void OnMoveEntity()
        {
            Map.MoveEntity();
        }

        public async Task Init(object mapView)
        {
            Map.SetMapView(mapView);
            
            await Map.LoadMap(new Uri(@"C:\Dev\Maps\map.tpk"));
            await Map.SetViewpoint(_viewpoint.Latitude, _viewpoint.Longitude, 100000);

            TestMap();
        }

        private void TestMap()
        {
            var p = new GeoLocation(_viewpoint);

            // Polyline
            var mapPoints = new List<GeoLocation>
            {
                new GeoLocation(p.Latitude, p.Longitude),
                new GeoLocation(p.Latitude += 0.01, p.Longitude -= 0.05),
                new GeoLocation(p.Latitude -= 0.05, p.Longitude -= 0.05),
                new GeoLocation(p.Latitude += 0.05, p.Longitude -= 0.02),
                new GeoLocation(p.Latitude += 0.03, p.Longitude += 0.05),
                new GeoLocation(p.Latitude += 0.05, p.Longitude -= 0.02),
            };

            Map.AddPolyline("pline1", mapPoints);

            //// Polygon
            //mapPoints = new List<GeoLocation>
            //{
            //    new GeoLocation(p.Latitude, p.Longitude),
            //    new GeoLocation(p.Latitude += 0.01, p.Longitude -= 0.05),
            //    new GeoLocation(p.Latitude -= 0.05, p.Longitude -= 0.05),
            //    new GeoLocation(p.Latitude += 0.05, p.Longitude -= 0.02),
            //    new GeoLocation(p.Latitude += 0.03, p.Longitude += 0.05),
            //    new GeoLocation(p.Latitude += 0.05, p.Longitude -= 0.02),
            //};

            //Map.AddPolygon("polygon1", mapPoints);

            // Ellipse
            //Map.AddEllipse("ellipse1", p, 4000, 2000);

            // Image
            AddFormation(p);
        }

        private void AddFormation(GeoLocation p)
        {
            var imageSource = new Uri(Path.Combine(Directory.GetCurrentDirectory(), @"Resources/Images/aircraft_green.png"));

            var formation = new List<MapEntity>()
            {
                new MapEntity
                {
                    Id = "charlie",
                    IsParent= true,
                    IsSelectable= true,
                    Source = imageSource,
                    Location = p,
                    Angle = 45,
                    Width = 50,
                    Height = 50,
                },
                new MapEntity
                {
                    Id = "david",
                    IsSelectable= false,
                    Source = imageSource,
                    //Location = new GeoLocation(p.Latitude - 0.01, p.Longitude - 0.01),
                    Angle = 180,
                    Width = 50,
                    Height = 50,
                    OffsetX = 100,
                    OffsetY = -50
                },
                new MapEntity
                {
                    Id = "moshe",
                    IsSelectable= false,
                    Source = imageSource,
                    //Location = new GeoLocation(p.Latitude - 0.01, p.Longitude + 0.01),
                    Angle = 350,
                    Width = 50,
                    Height = 50,
                    OffsetX = -100,
                    OffsetY = -50
                }
            };

            Map.AddEntityGroup(new MapEntityGroup("charlie2080", formation));
        }

        private void OnStartSketch()
        {
            Map.StartSketch(SketchMode.Polyline);
        }

        private void OnEditSketch()
        {
            Map.EditSketch();
        }

        private void OnCompleteSketch()
        {
            Map.CompleteSketch();
        }

        private void OnCancelSketch()
        {
            Map.CancelSketch();
        }

        private void OnRemoveSelectedEntities()
        {
            Map.RemoveSelectedEntities();
        }

        public void OnGeoViewTapped(object arg)
        {
            Map.OnGeoViewTapped(arg);
        }
    }
}
