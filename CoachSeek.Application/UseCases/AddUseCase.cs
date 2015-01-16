using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public abstract class AddUseCase<TData> : BaseUseCase<TData> where TData : class, IData, new()
    {
        protected AddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        protected Response<TData> Add(IBusinessIdable command)
        {
            if (command == null)
                return new NoDataResponse<TData>();

            try
            {
                var business = GetBusiness(command);
                var data = AddToBusiness(business, command);
                return new Response<TData>(data);
            }
            catch (Exception ex)
            {
                return HandleAddException(ex);
            }
        }

        protected abstract TData AddToBusiness(Business business, IBusinessIdable command);

        private Response<TData> HandleAddException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();

            var response = HandleSpecificException(ex);
            if (response != null)
                return response;

            if (ex is ValidationException)
                return new Response<TData>((ValidationException)ex);

            throw new InvalidOperationException();
        }

        protected abstract Response<TData> HandleSpecificException(Exception ex);
    }
}
