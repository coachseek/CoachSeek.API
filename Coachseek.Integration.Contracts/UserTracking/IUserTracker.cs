using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace Coachseek.Integration.Contracts.UserTracking
{
    public interface IUserTracker
    {
        Task CreateTrackingUserAsync(UserData user, BusinessData business);
    }
}
