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
                var business = new Business(Business.Id, command, SupportedCurrencyRepository);
                var data = BusinessRepository.UpdateBusiness(business);
                return new Response(data);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }
    }
}