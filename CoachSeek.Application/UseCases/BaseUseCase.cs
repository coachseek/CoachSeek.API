using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseUseCase<TData> where TData : class, IData, new()
    {
        protected IBusinessRepository BusinessRepository { get; set; }

        protected BaseUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


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

            var response = HandleSpecificAddException(ex);
            if (response != null)
                return response;

            if (ex is ValidationException)
                return new Response<TData>((ValidationException)ex);

            return null;
        }

        protected abstract Response<TData> HandleSpecificAddException(Exception ex);


        private Response<TData> HandleInvalidBusiness()
        {
            return new InvalidBusinessResponse<TData>();
        }

        protected Business GetBusiness(IBusinessIdable command)
        {
            var business = BusinessRepository.Get(command.BusinessId);
            if (business == null)
                throw new InvalidBusiness();
            return business;
        }
    }
}
