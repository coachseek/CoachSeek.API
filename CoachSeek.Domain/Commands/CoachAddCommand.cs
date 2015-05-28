namespace CoachSeek.Domain.Commands
{
    public class CoachAddCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public WeeklyWorkingHoursCommand WorkingHours { get; set; }
    }
}