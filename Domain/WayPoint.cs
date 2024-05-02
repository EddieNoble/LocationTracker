using System.ComponentModel.DataAnnotations.Schema;

namespace LocationTracker.Domain
{
	/// <summary>
	/// Describes a recorded point on a User's journey. 
	/// </summary>
	public class WayPoint
	{
		/// <summary>
		/// The unique ID of the Waypoint.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// The unique ID of the user.
		/// </summary>
		[ForeignKey("User")]
		public Guid UserId { get; set; }

		/// <summary>
		/// The latitude of the Waypoint.
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// The longitude of the Waypoint.
		/// </summary>
		public double Longitude { get; set; }

		/// <summary>
		/// The time the User was recorded at the location.
		/// </summary>
		public DateTime StopTime { get; set; }

		/// <summary>
		/// The User.
		/// </summary>
		public virtual User User { get; set; }

	}
}
