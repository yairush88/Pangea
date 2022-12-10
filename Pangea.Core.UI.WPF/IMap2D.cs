using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Pangea.Core.Model;

namespace Pangea.Core.UI
{
    public interface IMap2D
   {
        Task LoadMap(Uri source);
        void SetMapView(object mapView);
        Task SetViewpoint(double latitude, double longitude, double scale);
        void AddPolyline(string id, IEnumerable<GeoLocation> mapPoints);
        void AddPolygon(string id, IEnumerable<GeoLocation> mapPoints);
        void AddEllipse(string id, GeoLocation centerPoint, double width, double height);
        void AddEntity(MapEntity entity);
        void AddEntityGroup(MapEntityGroup entityGroup);
        Task StartSketch(SketchMode sketchMode, bool drawAndEdit = true);
        Task EditSketch();
        void CompleteSketch();
        void CancelSketch();
        bool RemoveEntity(string id);
        Task MoveEntity();
        void RemoveSelectedEntities();
        void OnGeoViewTapped(object arguments);
    }
}
