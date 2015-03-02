using System;

namespace CoachSeek.Data.Model
{
    public class CoachData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Email { get; set; }
        public string Phone { get; set; }
        public WeeklyWorkingHoursData WorkingHours { get; set; }
    }
}