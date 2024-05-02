using LocationTracker.Api.Services.Interfaces;
using LocationTracker.Context;
using LocationTracker.Domain;
using Microsoft.Data.Sqlite;
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

		/// <summary>
		/// Overloaded constructor spins up in-memory db for prototype development.
		/// </summary>
		public DataService()
		{
			_context = CreateContext();
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

		private LocationTrackerContext CreateContext()
		{
			var connection =
				new SqliteConnection("Data Source=TestConnection;Mode=Memory;Cache=Shared");
			connection.Open();

			var contextOptions = new DbContextOptionsBuilder<LocationTrackerContext>()
				.UseSqlite(connection)
				.Options;

			var context = new LocationTrackerContext(contextOptions);
			context.Database.EnsureCreated();

			var userId = Guid.NewGuid();
			var userId2 = Guid.NewGuid();
			var user = new User { Id = userId, Name = "User1" };
			var user2 = new User { Id = userId2, Name = "User2" };

			context.Users.Add(user);
			context.Users.Add(user2);

			context.SaveChanges();

			return context;
		}
	}
}
