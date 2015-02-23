using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseUseCase
    {
        protected IBusinessRepository BusinessRepository { get; set; }

        protected BaseUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        protected Business GetBusiness(Guid businessId)
        {
            var business = BusinessRepository.Get(businessId);
            if (business == null)
                throw new InvalidBusiness();
            return business;
        }

        protected Business GetBusiness(IBusinessIdable command)
        {
            return GetBusiness(command.BusinessId);
        }
    }
}
