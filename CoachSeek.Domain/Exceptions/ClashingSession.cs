using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class ClashingSession : SingleErrorException
    {
        public ClashingSession(Session clashingSession)
            : base("This session clashes with one or more sessions.",
                   ErrorCodes.SessionClashing,
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
