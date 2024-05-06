using LocationTracker.Api.Models;
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
			try
			{
				await _dataService.AddLocationAsync(waypoint);
				return Ok();
			}
			catch (KeyNotFoundException knfException)
			{
				return BadRequest(knfException);
			}
			catch (Exception exception) 
			{
				return StatusCode(500, exception.Message);
			}
		}

        /// <summary>
        /// Gets all WayPoints for the specified user.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="page">Optional - page of results to return.</param>
        /// <param name="pageSize">Optional - page size. If set to 0 all records will be returned regardless of any Page value.</param>
        /// <returns>A list of <see cref="WayPoint"/> objects.</returns>
        [HttpGet("{userId}/{page?}/{pageSize?}")]
		public async Task<List<WayPoint>> GetAllLocationsForUserAsync(Guid userId, int page = 0, int pageSize = 0)
		{
			return await _dataService.GetAllLocationsForUserAsync(userId, page, pageSize);
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
		[HttpGet("locationsall/{stopsAfter}")]
		public async Task<List<WayPoint>> GetRecentLocationsForAllUsersAsync(DateTime stopsAfter)
		{
			return await _dataService.GetRecentLocationsForAllUsersAsync(stopsAfter);
		}

        /// <summary>
        /// Gets a list of Waypoints for all users after the specified date wiothin bounds.
        /// </summary>
        /// <param name="stopsAfter">The starting date.</param>
        /// <param name="southWestLat">The south western boundary latitude.</param>
        /// <param name="southWestLong">The south western boundary longitude.</param>
        /// <param name="northEastLat">The north eastern boundary latitude.</param>
		/// <param name="northEastLing">The north eastern boundary longitude.</param>
        /// <returns>A list of <see cref="WayPoints"/>.</returns>
        [HttpGet("locationsallinbounds/{stopsAfter}/{southWestLat}/{southWestLong}/{northEastLat}/{northEastLong}")]
		public async Task<List<WayPoint>> GetRecentLocationsForAllUsersInBoundsAsync
			(DateTime stopsAfter, double southWestLat, double southWestLong, double northEastLat, double northEastLong)
		{
			return await _dataService.GetRecentLocationsForAllUsersInBoundsAsync(
				stopsAfter,
				new GeoCoordinate { Latitude = southWestLat, Longitude = southWestLong },
				new GeoCoordinate { Latitude = northEastLat, Longitude = northEastLong});
        }
    }
}
