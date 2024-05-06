namespace LocationTracker.Api.Models
{
    /// <summary>
    /// Class to define a Geographical Point.
    /// </summary>
    public class GeoCoordinate
    {
        /// <summary>
        /// The Latitude of the point.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// The Longitude of the point.
        /// </summary>
        public double Longitude { get; set; }
    }
}
