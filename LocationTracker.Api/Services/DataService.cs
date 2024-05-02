using LocationTracker.Api.Services.Interfaces;
using LocationTracker.Context;
using LocationTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace LocationTracker.Api.Services
{
	// <inheritdoc />
	public class DataService : IDataService
	{
		private readonly LocationTrackerContext _context;

        public DataService(LocationTrackerContext context)
        {
            _context = context;
        }

        // <inheritdoc />
        public async Task AddLocationAsync(WayPoint waypoint)
		{
			await _context.WayPoints.AddAsync(waypoint);
			await _context.SaveChangesAsync();
		}

		// <inheritdoc />
		public async Task<List<WayPoint>> GetAllLocationsForUserAsync(Guid userId)
		{
			return await _context.WayPoints
				.Where(wp => wp.UserId == userId).ToListAsync();
		}

		// <inheritdoc />
		public async Task<WayPoint> GetLastLocationForUserAsync(Guid userId)
		{
			var userTrips = 
				await _context.WayPoints
				.Where(wp => wp.UserId == userId)
				.OrderBy(wp => wp.StopTime).ToListAsync();

			return userTrips.Last();
		}

		// <inheritdoc />
		public async Task<List<WayPoint>> GetRecentLocationsForAllUsersAsync(DateTime stopsAfter)
		{
			return await _context.WayPoints
				.Where(wp => wp.StopTime > stopsAfter).ToListAsync();
		}
	}
}
