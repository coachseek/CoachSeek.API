using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Commands
{
    public class BusinessSetCustomerOnlineBookingEmailCustomisationCommand : ICommand
    {
        public string CustomerOnlineBookingEmailCustomisation { get; set; }
    }
}
