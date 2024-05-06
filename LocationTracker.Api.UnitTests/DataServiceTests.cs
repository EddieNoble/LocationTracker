using LocationTracker.Api.Services;
using LocationTracker.Context;
using LocationTracker.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LocationTracker.Api.UnitTests
{
	[TestClass]
	public class DataServiceTests
	{
		SqliteConnection _connection;
		const double SampleLat = 53.118755;
		const double SampleLong = -1.448822;

		[TestMethod]
		public async Task AddLocationAsyncWhenPassedValidObjectCreatesARecord() 
		{
			// Arrange
			var userId = Guid.NewGuid();
			var stopTime = DateTime.Now;
			var lat = SampleLat;
			var lng = SampleLong;
			var user = new User { Id = userId, Name = "User1" };
			var context = CreateContext();
			context.Users.Add(user);
			await context.SaveChangesAsync();

			var location = new WayPoint { UserId = userId, Latitude = lat, Longitude = lng, StopTime = stopTime };

			var sut = new DataService(context);

			// Act
			await sut.AddLocationAsync(location);
			var newWaypoint = context.WayPoints.Single();

			// Assert
			Assert.AreEqual(userId, newWaypoint.UserId);
			Assert.AreEqual(stopTime, newWaypoint.StopTime);
			Assert.AreEqual(lat, newWaypoint.Latitude);
			Assert.AreEqual(lng, newWaypoint.Longitude);
			await context.DisposeAsync();
			await _connection.CloseAsync();
		}

		[TestMethod]
		public async Task GetAllLocationsForUserAsyncReturnsOnlyRecordsForTheSpecifiedUser()
		{
			// Arrange
			var userId = Guid.NewGuid();
			var userId2 = Guid.NewGuid();
			var stopTime = DateTime.Now;
			var lat = SampleLat;
			var lng = SampleLong;
			var user = new User { Id = userId, Name = "User1" };
			var user2 = new User { Id = userId2, Name = "User2" };
			var waypoint1 = new WayPoint { UserId = userId, Latitude = lat, Longitude = lng, StopTime = stopTime };
			var waypoint2 = new WayPoint { UserId = userId2, Latitude = lat, Longitude = lng, StopTime = stopTime };
			var context = CreateContext();
			context.Users.Add(user);
			context.Users.Add(user2);
			context.WayPoints.Add(waypoint1);
			context.WayPoints.Add(waypoint2);
			await context.SaveChangesAsync();

			var sut = new DataService(context);

			// Act
			var result = await sut.GetAllLocationsForUserAsync(userId);

			// Assert
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(userId, result.Single().UserId);
            await context.DisposeAsync();
            await _connection.CloseAsync();
        }

        [TestMethod]
		public async Task GetLastLocationForUserAsyncRetreivesTheLastRecord()
		{
			// Arrange
			var userId = Guid.NewGuid();
			var stopTime = DateTime.Now;
			var earlierStopTime = DateTime.Now.AddDays(-1);
			var lat = SampleLat;
			var lng = SampleLong;
			var user = new User { Id = userId, Name = "User1" };
			var waypoint1 = new WayPoint { UserId = userId, Latitude = lat, Longitude = lng, StopTime = stopTime };
			var waypoint2 = new WayPoint { UserId = userId, Latitude = lat, Longitude = lng, StopTime = earlierStopTime };
			var context = CreateContext();
			context.Users.Add(user);
			context.WayPoints.Add(waypoint1);
			context.WayPoints.Add(waypoint2);
			await context.SaveChangesAsync();

			var sut = new DataService(context);

			// Act
			var result = await sut.GetLastLocationForUserAsync(userId);

			// Assert
			Assert.AreEqual(stopTime, result.StopTime);
            await context.DisposeAsync();
            await _connection.CloseAsync();
        }

        [TestMethod]
		public async Task GetRecentLocationsForAllUsersAsyncRetreivesRecordsAterTheSpecifiedDateTime()
		{
			// Arrange
			var userId = Guid.NewGuid();
			var userId2 = Guid.NewGuid();
			var stopTime = DateTime.Now;
			var earliestStopTime = DateTime.Now.AddDays(-2);
			var midStopTime = DateTime.Now.AddDays(-1);
			var queryTime = midStopTime.AddMinutes(-1);
			var lat = SampleLat;
			var lng = SampleLong;
			var user = new User { Id = userId, Name = "User1" };
			var user2 = new User { Id = userId2, Name = "User2" };
			var waypoint1 = new WayPoint { UserId = userId, Latitude = lat, Longitude = lng, StopTime = stopTime };
			var waypoint2 = new WayPoint { UserId = userId2, Latitude = lat, Longitude = lng, StopTime = midStopTime };
			var waypoint3 = new WayPoint { UserId = userId2, Latitude = lat, Longitude = lng, StopTime = earliestStopTime };
			var context = CreateContext();
			context.Users.Add(user);
			context.Users.Add(user2);
			context.WayPoints.Add(waypoint1);
			context.WayPoints.Add(waypoint2);
			context.WayPoints.Add(waypoint3);
			await context.SaveChangesAsync();

			var sut = new DataService(context);

			// Act
			var result = await sut.GetRecentLocationsForAllUsersAsync(queryTime);

			// Assert
			Assert.AreEqual(2, result.Count);
			Assert.AreEqual(0, result.Count(r => r.StopTime == earliestStopTime));
            await context.DisposeAsync();
            await _connection.CloseAsync();
        }

        [TestMethod]
		public async Task AddLocationAsyncShouldTriggerAWayPointAddedEvent() 
		{
            // Arrange
            var userId = Guid.NewGuid();
            var stopTime = DateTime.Now;
            var lat = SampleLat;
            var lng = SampleLong;

			var context = CreateContext();
			var user = new User { Id = userId, Name = "User1" };

			context.Users.Add(user);	

            var waypoint = new WayPoint { UserId = userId, Latitude = lat, Longitude = lng, StopTime = stopTime };

			var sut = new DataService(context);

			WayPoint newWayPoint = new WayPoint();

			sut.WayPointAdded += (sender, args) =>
            {
				newWayPoint = args.NewWayPoint;
            };

			// Act
			await sut.AddLocationAsync(waypoint);

			// Assert
			Assert.AreEqual(userId, newWayPoint.UserId);
            Assert.AreEqual(SampleLat, newWayPoint.Latitude);
            Assert.AreEqual(SampleLong, newWayPoint.Longitude);
			Assert.AreEqual(stopTime, newWayPoint?.StopTime);
            await context.DisposeAsync();
            await _connection.CloseAsync();
        }

        private LocationTrackerContext CreateContext()
		{
			_connection =
				new SqliteConnection("Data Source=TestConnection;Mode=Memory;Cache=Shared");
			_connection.Open();

			var contextOptions = new DbContextOptionsBuilder<LocationTrackerContext>()
				.UseSqlite(_connection)
				.Options;

			var context = new LocationTrackerContext(contextOptions);
			context.Database.EnsureCreated();
			return context;
        }
    }
}
