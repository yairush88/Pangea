namespace Pangea.Core.Model
{
    public class GeoLocation
    {
        public GeoLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public GeoLocation(double latitude, double longitude, double altitude) : this(latitude, longitude)
        {
            Altitude = altitude;
        }

        public GeoLocation(GeoLocation geoLocation)
        {
            Latitude = geoLocation.Latitude;
            Longitude = geoLocation.Longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}
