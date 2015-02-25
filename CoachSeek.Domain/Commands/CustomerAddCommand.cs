using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class CustomerAddCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


        public NewCustomerData ToData()
        {
            return Mapper.Map<CustomerAddCommand, NewCustomerData>(this);
        }
    }
}