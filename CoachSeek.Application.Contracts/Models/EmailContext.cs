using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class EmailContext
    {
        public bool IsEmailingEnabled { get; private set; }
        public bool ForceEmail { get; private set; }
        public string EmailSender { get; private set; }
        public IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { get; private set; }


        public EmailContext(bool isEmailingEnabled, bool forceEmail, string emailSender, IUnsubscribedEmailAddressRepository unsubscribedEmailAddressRepository)
        {
            IsEmailingEnabled = isEmailingEnabled;
            ForceEmail = forceEmail;
            EmailSender = emailSender;
            UnsubscribedEmailAddressRepository = unsubscribedEmailAddressRepository;
        }
    }
}
