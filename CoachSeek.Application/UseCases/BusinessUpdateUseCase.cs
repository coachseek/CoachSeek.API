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
                // TODO: Get existing business and then pass it into the Business constructor.
                //var existingBusiness = BusinessRepository.GetBusiness(Business.Id);
                var business = new Business(Business.Id, command, SupportedCurrencyRepository);
                var data = BusinessRepository.UpdateBusiness(business);
                return new Response(data);
                //return new Response(business.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }
    }
}