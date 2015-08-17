using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionClashing : SingleErrorException
    {
        public SessionClashing(Session clashingSession)
            : base(ErrorCodes.SessionClashing, 
                   "This session clashes with one or more sessions.",
                   FormatClashingSessionMessage(clashingSession))
        { }


        private static string FormatClashingSessionMessage(Session clashingSession)
        {
            return string.Format("Clashing session: {0}; SessionId = {{{1}}}",
                                 clashingSession.Name,
                                 clashingSession.Id);
        }
    }
}
