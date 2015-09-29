using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace Coachseek.Integration.Contracts.UserTracking
{
    public interface IUserTracker
    {
        void CreateTrackingUser(UserData user, BusinessData business);
        Task CreateTrackingUserAsync(UserData user, BusinessData business);
    }
}
