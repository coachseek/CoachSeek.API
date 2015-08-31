using CoachSeek.Application.Contracts.UseCases.Admin;

namespace CoachSeek.Application.UseCases.Admin
{
    public class EmailIsUnsubscribedUseCase : BaseAdminUseCase, IEmailIsUnsubscribedUseCase
    {
        public bool IsUnsubscribed(string emailAddress)
        {
            return UnsubscribedEmailAddressRepository.Get(emailAddress);
        }
    }
}
