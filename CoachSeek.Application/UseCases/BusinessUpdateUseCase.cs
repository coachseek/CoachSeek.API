using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BusinessUpdateUseCase : BaseUseCase, IBusinessUpdateUseCase
    {
        public Response UpdateBusiness(BusinessUpdateCommand command)
        {
            try
            {
                var business = new Business(Business.Id, command, SupportedCurrencyRepository);
                ValidateUpdate(business);
                var data = BusinessRepository.UpdateBusiness(business);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is CurrencyNotSupported)
                    return new CurrencyNotSupportedErrorResponse("business.currency");
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateUpdate(Business business)
        {
            var existingBusiness = BusinessRepository.GetBusiness(Business.Id);
            if (existingBusiness.IsNotFound())
                throw new InvalidBusiness();
        }
    }
}