using System;

namespace Pangea.Core.Model
{
    public class MapEntity
    {
        public string Id { get; set; }
        public Uri Source { get; set; }
        public GeoLocation Location { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double Angle { get; set; }
        public bool IsSelectable { get; set; }
        public bool IsParent { get; set; }
        public MapEntity Parent { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
    }
}
