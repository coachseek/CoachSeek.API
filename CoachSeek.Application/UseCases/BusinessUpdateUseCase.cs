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
        public IResponse UpdateBusiness(BusinessUpdateCommand command)
        {
            try
            {
                var existingBusiness = BusinessRepository.GetBusiness(Business.Id);
                var business = new Business(existingBusiness, command, SupportedCurrencyRepository);
                BusinessRepository.UpdateBusiness(business);
                return new Response(business.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }
    }
}