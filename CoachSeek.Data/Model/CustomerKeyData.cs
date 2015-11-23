using System;

namespace CoachSeek.Data.Model
{
    public class CustomerKeyData : KeyData
    {
        public CustomerKeyData(Guid id)
        {
            Id = id;
        }

        public CustomerKeyData(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
