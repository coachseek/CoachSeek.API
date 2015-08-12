namespace CoachSeek.Domain.Commands
{
    public class BusinessRegistrationCommand
    {
        public BusinessAddCommand Business { get; set; }
        public UserAddCommand Admin { get; set; }
    }
}
