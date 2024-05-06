﻿using LocationTracker.Api.Models;
using LocationTracker.Api.Services.Interfaces;
using LocationTracker.Context;
using LocationTracker.Domain;
using LocationTracker.Domain.EventArguments;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text;

namespace LocationTracker.Api.Services
{
	// <inheritdoc />
	public class DataService : IDataService
	{
        /// <summary>
        /// Represents the method that will handle an event when a <see cref="WayPoint"/> is added. />
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">A <see cref="WayPointAddedEventArgs" containing the new WayPoint. /></param>
        public delegate void WayPointAddedEventHandler(object sender, WayPointAddedEventArgs e);

		/// <summary>
		/// Raised when a new WayPoint is added to the data store.
		/// </summary>
		public event WayPointAddedEventHandler WayPointAdded;	

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
			if (!_context.Users.Any(u => u.Id == waypoint.UserId)) 
			{
				throw new KeyNotFoundException($"No user found with ID {waypoint.UserId}");
			}

			await _context.WayPoints.AddAsync(waypoint);
			await _context.SaveChangesAsync();
			OnWayPointAdded(waypoint);
		}

		// <inheritdoc />
		public async Task<List<WayPoint>> GetAllLocationsForUserAsync(Guid userId, int page = 0, int pageSize = 0)
		{
            var usersWayPoints = 
				_context.WayPoints.Where(wp => wp.UserId == userId).OrderBy(wp => wp.StopTime);

			if (pageSize > 0) 
			{
				var skip = page * pageSize;

				usersWayPoints = (IOrderedQueryable<WayPoint>)usersWayPoints.Skip(skip).Take(pageSize);

			}

			return await usersWayPoints.ToListAsync();
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

        // <inheritdoc />
        public async Task<List<WayPoint>> GetRecentLocationsForAllUsersInBoundsAsync(DateTime stopsAfter, GeoCoordinate southWest, GeoCoordinate northEast)
        {
			var pointsInTime = await _context.WayPoints.Where(wp => wp.StopTime > stopsAfter).ToListAsync();

            return 
				pointsInTime.Where(p => 
				IsWithinBounds( 
					new GeoCoordinate { Latitude = p.Latitude, Longitude = p.Longitude}, 
					southWest, 
					northEast)).ToList();
        }

		private static bool IsWithinBounds(GeoCoordinate pointToCheck, GeoCoordinate southWestBound, GeoCoordinate northEastBound)
        {

			bool isLatitudeInRange =
				pointToCheck.Latitude >= southWestBound.Latitude
				&& pointToCheck.Latitude <= northEastBound.Latitude;

            bool isLongitudeInRange =
				pointToCheck.Longitude >= southWestBound.Longitude
				&& pointToCheck.Longitude <= northEastBound.Longitude;

            return isLatitudeInRange && isLongitudeInRange;		
		}

        private void OnWayPointAdded(WayPoint newWayPoint)
        {
			var handler = WayPointAdded;
			var args = new WayPointAddedEventArgs { NewWayPoint = newWayPoint };
			handler?.Invoke(this, args);
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
