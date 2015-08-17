using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases.Admin;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases.Admin
{
    public class EmailUnsubscribeUseCase : BaseUseCase, IEmailUnsubscribeUseCase
    {
        public IResponse Unsubscribe(string emailAddress)
        {
            try
            {
                ValidateEmailAddress(emailAddress);
                UnsubscribedEmailAddressRepository.Save(emailAddress);
                return new Response();
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private void ValidateEmailAddress(string emailAddress)
        {
            var email = new EmailAddress(emailAddress);
        }
    }
}
