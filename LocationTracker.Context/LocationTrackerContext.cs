using LocationTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace LocationTracker.Context
{
    public class LocationTrackerContext: DbContext
	{
        public LocationTrackerContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; }

		public DbSet<WayPoint> WayPoints { get; set; }

		public DbSet<PointOfInterest> PointsOfInterest { get; set; }


    }
}
