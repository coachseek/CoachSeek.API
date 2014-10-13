using System;
using CoachSeek.Domain.Data;

namespace CoachSeek.Domain.Entities
{
    public class NewCoach : Coach
    {
        public NewCoach(string firstName, string lastName, string email, string phone)
            : base(Guid.NewGuid(), firstName, lastName, email, phone)
        { }

        public NewCoach(NewCoachData data)
            : this(data.FirstName, 
                   data.LastName, 
                   data.Email, 
                   data.Phone)
        { }
    }
}