using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public abstract class AddUseCase : BaseUseCase
    {
        protected AddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        protected Response Add(IBusinessIdable command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var business = GetBusiness(command);
                var data = AddToBusiness(business, command);
                return new Response(data);
            }
            catch (Exception ex)
            {
                return HandleAddException(ex);
            }
        }

        protected abstract object AddToBusiness(Business business, IBusinessIdable command);

        private ErrorResponse HandleAddException(Exception ex)
        {
            var response = HandleSpecificException(ex);
            if (response != null)
                return response;

            if (ex is ValidationException)
                return new ErrorResponse((ValidationException)ex);

            throw (ex);
        }

        protected abstract ErrorResponse HandleSpecificException(Exception ex);
    }
}
