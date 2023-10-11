using NpgsqlTypes;

namespace Backand.FrontendEntities
{
    public class Spot
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public static class NpgSQLPointExtension
    {
        public static Spot ToSpot(this NpgsqlPoint point)
        {
            return new Spot { Latitude = point.X, Longitude = point.Y };
        }
    }
}
