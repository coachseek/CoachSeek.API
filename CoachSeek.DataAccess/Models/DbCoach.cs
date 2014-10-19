using System;

namespace CoachSeek.DataAccess.Models
{
    public class DbCoach
    {
        public Guid Id { get; set; }
        //public Guid BusinessId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public DbWeeklyWorkingHours WorkingHours { get; set; }
    }
}