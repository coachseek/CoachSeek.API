using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class NewCoach : Coach
    {
        public NewCoach(string firstName, string lastName, string email, string phone, WeeklyWorkingHoursCommand workingHours)
            : base(Guid.NewGuid(), firstName, lastName, email, phone, workingHours)
        { }

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

        public NewCoach(CoachAddCommand command)
            : this(command.FirstName,
                   command.LastName,
                   command.Email,
                   command.Phone,
                   command.WorkingHours)
        { }
    }
}