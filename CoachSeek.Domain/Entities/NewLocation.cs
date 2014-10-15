using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewLocation : Location
    {
        public NewLocation(string name)
            : base(Guid.NewGuid(), name)
        { }

        public NewLocation(NewLocationData data)
            : this(data.Name)
        { }
    }
}