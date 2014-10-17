using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewCoach : Coach
    {
        public NewCoach(string firstName, string lastName, string email, string phone, WeeklyWorkingHoursData workingHours)
            : base(Guid.NewGuid(), firstName, lastName, email, phone, workingHours)
        { }

        public NewCoach(NewCoachData data)
            : this(data.FirstName, 
                   data.LastName, 
                   data.Email, 
                   data.Phone,
                   data.WorkingHours)
        { }
    }
}