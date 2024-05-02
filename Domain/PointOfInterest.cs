namespace LocationTracker.Domain
{
	/// <summary>
	/// Describes a location.
	/// </summary>
	public class PointOfInterest
	{
		/// <summary>
		/// The unique ID of the Point of Interest.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// The name of the Point of Interest.
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// The latitude of the Point of Interest.
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// The longitude of the Point of Interest.
		/// </summary>
		public double Longitude { get; set; }

	}
}
