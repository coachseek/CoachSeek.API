using CoachSeek.Application.Contracts.UseCases.Admin;

namespace CoachSeek.Application.UseCases.Admin
{
    public class EmailIsUnsubscribedUseCase : BaseUseCase, IEmailIsUnsubscribedUseCase
    {
        public bool IsUnsubscribed(string emailAddress)
        {
            return UnsubscribedEmailAddressRepository.Get(emailAddress);
        }
    }
}
