using LocationTracker.Api.Models;
using LocationTracker.Domain;

namespace LocationTracker.Api.Services.Interfaces
{
	/// <summary>
	/// CRUD endpoints for the application.
	/// </summary>
	public interface IDataService
	{
		/// <summary>
		/// Adds a location for a specific User.
		/// </summary>
		/// <param name="waypoint">The <see cref="WayPoint"/> to add.</param>
		/// <returns>Completed Task.</returns>
		Task AddLocationAsync(WayPoint waypoint);

		/// <summary>
		/// Gets the last visited location for the specified User.
		/// </summary>
		/// <param name="userId">The unique ID of the <see cref="User"/>.</param>
		/// <returns>A <see cref="WayPoint"/>.</returns>
		Task<WayPoint> GetLastLocationForUserAsync(Guid userId);

		/// <summary>
		/// Gets all visited locations for the specified User.
		/// </summary>
		/// <param name="userId">The unique ID of the <see cref="User"/>.</param>
		/// <param name="page">The page of records to return.</param>
		/// <param name="pageSize">The size of a page of records. If 0 all records are returned.</param>
		/// <returns>A list of <see cref="WayPoint"/> objects.</returns>
		Task<List<WayPoint>> GetAllLocationsForUserAsync(Guid userId, int page = 0, int pageSize = 0);

		/// <summary>
		/// Gets locations visited by all users sice the specified date and time.
		/// </summary>
		/// <param name="stopsAfter">The earliest date to return.</param>
		/// <returns>A list of <see cref="WayPoint"/> objects.</returns>
		Task<List<WayPoint>> GetRecentLocationsForAllUsersAsync(DateTime stopsAfter);

        /// <summary>
        /// Gets locations visited by all users sice the specified date and time and within a specified boundary.
        /// </summary>
        /// <param name="stopsAfter">The earliest date to return.</param>
		/// <param name="northEast">The North Eastern boundary of the query.</param>
		/// <param name="southWest">The South estern boundary of the query.</param>
        /// <returns>A list of <see cref="WayPoint"/> objects.</returns>
        Task<List<WayPoint>> GetRecentLocationsForAllUsersInBoundsAsync(DateTime stopsAfter, GeoCoordinate southWest, GeoCoordinate northEast);
    }

}
