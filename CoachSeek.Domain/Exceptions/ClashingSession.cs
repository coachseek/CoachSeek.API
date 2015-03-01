using System;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class ClashingSession : Exception
    {
        public Session Session { get; set; }


        public ClashingSession()
        { }

        public ClashingSession(Session session)
        {
            Session = session;
        }
    }
}
