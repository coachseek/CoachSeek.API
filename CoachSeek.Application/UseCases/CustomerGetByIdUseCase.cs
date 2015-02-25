using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class CustomerGetByIdUseCase : BaseUseCase, ICustomerGetByIdUseCase
    {
        public Guid BusinessId { get; set; }

        public CustomerGetByIdUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public CustomerData GetCustomer(Guid id)
        {
            return BusinessRepository.GetCustomer(BusinessId, id);
        }
    }
}
