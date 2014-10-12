using System;
using CoachSeek.Domain.Data;

namespace CoachSeek.Domain
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


        // Internal since for unit testing only!
        public NewCoach(CoachData coachData)
            : base(coachData)
        { }

        internal NewCoach(Guid id, string firstName, string lastName, string email, string phone)
            : base(id, firstName, lastName, email, phone)
        { }
    }
}