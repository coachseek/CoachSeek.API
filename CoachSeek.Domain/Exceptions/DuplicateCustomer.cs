using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class DuplicateCustomer : SingleErrorException
    {
        public DuplicateCustomer(Customer customer)
            : base(string.Format("Customer '{0}' with email '{1}' already exists.", customer.Name, customer.Email),
                   ErrorCodes.CustomerDuplicate,
                   string.Format("{0}, {1}", customer.Name, customer.Email))
        { }
    }
}