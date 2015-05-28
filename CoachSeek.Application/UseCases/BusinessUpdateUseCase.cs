using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class BusinessUpdateUseCase : BaseUseCase, IBusinessUpdateUseCase
    {
        private ISupportedCurrencyRepository SupportedCurrencyRepository { get; set; }

        public BusinessUpdateUseCase(ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            SupportedCurrencyRepository = supportedCurrencyRepository;
        }

        public Response UpdateBusiness(BusinessUpdateCommand command)
        {
            try
            {
                var business = new Business(BusinessId, command, SupportedCurrencyRepository);
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
            var existingBusiness = BusinessRepository.GetBusiness(BusinessId);
            if (existingBusiness.IsNotFound())
                throw new InvalidBusiness();
        }
    }
}