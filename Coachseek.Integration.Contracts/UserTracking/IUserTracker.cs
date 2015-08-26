using CoachSeek.Data.Model;

namespace Coachseek.Integration.Contracts.UserTracking
{
    public interface IUserTracker
    {
        void CreateTrackingUser(UserData user);
    }
}
