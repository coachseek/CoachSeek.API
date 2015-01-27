using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewCustomer : Customer
    {
        public NewCustomer(string firstName, string lastName, string email, string phone)
            : base(Guid.NewGuid(), firstName, lastName, email, phone)
        { }

        public NewCustomer(NewCustomerData data)
            : this(data.FirstName, 
                   data.LastName, 
                   data.Email, 
                   data.Phone)
        { }
    }
}