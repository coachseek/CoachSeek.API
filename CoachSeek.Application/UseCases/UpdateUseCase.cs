using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public abstract class UpdateUseCase<TData> : BaseUseCase<TData> where TData : class, IData, new()
    {
        protected UpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        protected Response<TData> Update(IBusinessIdable command)
        {
            if (command == null)
                return new NoDataResponse<TData>();

            try
            {
                var business = GetBusiness(command);
                var data = UpdateInBusiness(business, command);
                return new Response<TData>(data);
            }
            catch (Exception ex)
            {
                return HandleUpdateException(ex);
            }
        }

        protected abstract TData UpdateInBusiness(Business business, IBusinessIdable command);

        private Response<TData> HandleUpdateException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();

            var response = HandleSpecificException(ex);
            if (response != null)
                return response;

            if (ex is ValidationException)
                return new Response<TData>((ValidationException)ex);

            return null;
        }

        protected abstract Response<TData> HandleSpecificException(Exception ex);
    }
}
