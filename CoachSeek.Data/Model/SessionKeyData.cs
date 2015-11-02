using System;

namespace CoachSeek.Data.Model
{
    public class SessionKeyData : KeyData
    {
        public SessionKeyData(Guid id)
        {
            Id = id;
        }

        public SessionKeyData(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
