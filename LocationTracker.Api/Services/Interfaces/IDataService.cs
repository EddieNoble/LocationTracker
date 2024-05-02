using LocationTracker.Domain;

namespace LocationTracker.Api.Services.Interfaces
{
	/// <summary>
	/// CRUD endpoints for the application.
	/// </summary>
	public interface IDataService
	{
		/// <summary>
		/// Adds a locatin for a specific User.
		/// </summary>
		/// <param name="waypoint">The <see cref="WayPoint"/> to add.</param>
		/// <returns>Completed Task.</returns>
		Task AddLocationAsync(WayPoint waypoint);

		/// <summary>
		/// Gets the last visited location for the specified User.
		/// </summary>
		/// <param name="userId">The unique ID of the <see cref="User"/> of interest.</param>
		/// <returns>A <see cref="WayPoint"/>.</returns>
		Task<WayPoint> GetLastLocationForUserAsync(Guid userId);

		/// <summary>
		/// Gets all visited locations for the specified User.
		/// </summary>
		/// <param name="userId">The unique ID of the <see cref="User"/> of interest.</param>
		/// <returns>A list of <see cref="WayPoint"/> objects.</returns>
		Task<List<WayPoint>> GetAllLocationsForUserAsync(Guid userId);

		/// <summary>
		/// Gets locations visited by all users sice the specified date and time.
		/// </summary>
		/// <param name="stopsAfter">The earliest date to return.</param>
		/// <returns>A list of <see cref="WayPoint"/> objects.</returns>
		Task<List<WayPoint>> GetRecentLocationsForAllUsersAsync(DateTime stopsAfter);
	}
}
