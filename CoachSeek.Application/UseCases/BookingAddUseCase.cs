using System;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public abstract class BookingAddUseCase : BaseUseCase
    {
        protected void ValidateCustomer(Guid customerId, ValidationException errors)
        {
            var customer = BusinessRepository.GetCustomer(Business.Id, customerId);
            if (customer.IsNotFound())
                errors.Add(new CustomerInvalid(customerId));
        }
    }
}
