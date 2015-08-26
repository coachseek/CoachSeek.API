namespace Coachseek.Integration.Contracts.UserTracking
{
    public interface IUserTrackerFactory
    {
        string Credentials { set; }

        IUserTracker GetUserTracker(bool isUserTrackingEnabled, bool isTesting);
    }
}
