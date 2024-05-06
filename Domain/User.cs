using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationTracker.Domain
{
	/// <summary>
	/// Describes a system user.
	/// </summary>
	public class User
	{
		/// <summary>
		/// The unique ID of the User.
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// The name of the User.
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// The User's WayPoints.
		/// </summary>
		public virtual ICollection<WayPoint> WayPoints { get; set; } = new List<WayPoint>();
	}
}
