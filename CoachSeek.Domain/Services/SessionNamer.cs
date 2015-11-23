using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Services
{
    public static class SessionNamer
    {
        public static string Name(SingleSessionData session)
        {
            return string.Format("{0} at {1} with {2} on {3} at {4}", 
                                 session.Service.Name, 
                                 session.Location.Name, 
                                 session.Coach.Name,
                                 session.Timing.StartDate,
                                 session.Timing.StartTime);
        }
    }
}
