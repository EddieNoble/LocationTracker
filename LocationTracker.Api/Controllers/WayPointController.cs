using LocationTracker.Api.Services.Interfaces;
using LocationTracker.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LocationTracker.Api.Controllers
{
	/// <summary>
	/// Provides access to Commands and Queries related to WayPoints.
	/// </summary>
	public class WayPointController : Controller
	{
		private readonly IDataService _dataService;

		/// <summary>
		/// Constructor for the Controller.
		/// </summary>
		/// <param name="dataService">An instnace of <see cref="IDataService"/>.</param>
		public WayPointController(IDataService dataService)
		{
			_dataService = dataService;
		}

		/// <summary>
		/// Adds a WayPoint to the store.
		/// </summary>
		/// <param name="waypoint">A <see cref="WayPoint"/> object.</param>
		/// <returns>OK response.</returns>
		[HttpPost("add")]
		public async Task<IActionResult> AddLocationAsync([FromBody] WayPoint waypoint)
		{
			await _dataService.AddLocationAsync(waypoint);
			return Ok();
		}

		/// <summary>
		/// Gets all WayPoints for the specified user.
		/// </summary>
		/// <param name="userId">The unique ID of the user.</param>
		/// <returns>A list of <see cref="WayPoint"/> objects.</returns>
		[HttpGet("{userId}")]
		public async Task<List<WayPoint>> GetAllLocationsForUserAsync(Guid userId)
		{
			return await _dataService.GetAllLocationsForUserAsync(userId);
		}

		/// <summary>
		/// Gets the last Waypoint for the specified user.
		/// </summary>
		/// <param name="userId">The unique ID of the user.</param>
		/// <returns>A <see cref="WayPoint"/> object.</returns>
		[HttpGet("lastlocation/{userId}")]
		public async Task<WayPoint> GetLastLocationForUserAsync(Guid userId)
		{
			return await _dataService.GetLastLocationForUserAsync(userId);
		}

		/// <summary>
		/// Gets a list of Waypoints for all users after the specified date.
		/// </summary>
		/// <param name="stopsAfter">The starting date.</param>
		/// <returns>A list of <see cref="WayPoints"/>.</returns>
		[HttpGet("lastlocationall/{stopsAfter}")]
		public async Task<List<WayPoint>> GetRecentLocationsForAllUsersAsync(DateTime stopsAfter)
		{
			return await _dataService.GetRecentLocationsForAllUsersAsync(stopsAfter);
		}
	}
}
