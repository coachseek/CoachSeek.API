using System;
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        private void ValidateEmailAddress(string emailAddress)
        {
            var email = new EmailAddress(emailAddress);
        }

        private ErrorResponse HandleException(Exception ex)
        {
            if (ex is MissingEmailAddress)
                return new ErrorResponse(new ValidationException("Missing email address."));
            if (ex is InvalidEmailAddressFormat)
                return new ErrorResponse(new ValidationException("Invalid email address format."));

            return null;
        }
    }
}
