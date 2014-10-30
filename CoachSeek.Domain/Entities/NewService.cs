using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewService : Service
    {
        public NewService(string name, string description)
            : base(Guid.NewGuid(), name, description)
        { }

        public NewService(NewServiceData data)
            : this(data.Name, data.Description)
        { }
    }
}