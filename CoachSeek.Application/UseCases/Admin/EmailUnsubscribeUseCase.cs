using CoachSeek.Application.Contracts.UseCases.Admin;

namespace CoachSeek.Application.UseCases.Admin
{
    public class EmailUnsubscribeUseCase : BaseUseCase, IEmailUnsubscribeUseCase
    {
        public void Unsubscribe(string emailAddress)
        {
            UnsubscribedEmailAddressRepository.Save(emailAddress);
        }
    }
}
