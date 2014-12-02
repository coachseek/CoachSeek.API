using System;

namespace CoachSeek.Data.Model
{
    public class SessionData : NewSessionData
    {
        public Guid Id { get; set; }


        public SessionData()
        { }

        public SessionData(Guid id, NewSessionData newSession)
            : base(newSession)
        {
            Id = id;
        }
    }
}
