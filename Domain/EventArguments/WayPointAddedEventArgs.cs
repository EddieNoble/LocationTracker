namespace LocationTracker.Domain.EventArguments
{
    public class WayPointAddedEventArgs: EventArgs
    {
        public WayPoint NewWayPoint { get; set; }
    }
}
