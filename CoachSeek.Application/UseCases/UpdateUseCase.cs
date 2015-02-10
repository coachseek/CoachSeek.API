using CoachSeek.Application.Contracts.Models;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public abstract class UpdateUseCase : BaseUseCase
    {
        protected UpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        protected Response Update(IBusinessIdable command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var business = GetBusiness(command);
                var data = UpdateInBusiness(business, command);
                return new Response(data);
            }
            catch (Exception ex)
            {
                return HandleUpdateException(ex);
            }
        }

        protected abstract object UpdateInBusiness(Business business, IBusinessIdable command);

        private Response HandleUpdateException(Exception ex)
        {
            //if (ex is InvalidBusiness)
            //    return HandleInvalidBusiness();

            var response = HandleSpecificException(ex);
            if (response != null)
                return response;

            if (ex is ValidationException)
                return new ErrorResponse((ValidationException)ex);

            return null;
        }

        protected abstract ErrorResponse HandleSpecificException(Exception ex);
    }
}
