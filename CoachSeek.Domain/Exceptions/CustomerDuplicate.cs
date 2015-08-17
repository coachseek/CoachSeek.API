using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomerDuplicate : SingleErrorException
    {
        public CustomerDuplicate(Customer customer)
            : base(ErrorCodes.CustomerDuplicate, 
                   string.Format("Customer '{0}' with email '{1}' already exists.", customer.Name, customer.Email),
                   string.Format("{0}, {1}", customer.Name, customer.Email))
        { }
    }
}