using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomerGetByIdUseCase : BaseUseCase, ICustomerGetByIdUseCase
    {
        public CustomerData GetCustomer(Guid id)
        {
            return BusinessRepository.GetCustomer(BusinessId, id);
        }
    }
}
