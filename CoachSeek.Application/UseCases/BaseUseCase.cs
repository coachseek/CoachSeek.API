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


        protected Response<TData> HandleInvalidBusiness()
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
