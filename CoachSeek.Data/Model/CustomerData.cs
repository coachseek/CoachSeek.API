using System;
using AutoMapper;

namespace CoachSeek.Data.Model
{
    public class CustomerData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }


        public CustomerKeyData ToKeyData()
        {
            return Mapper.Map<CustomerData, CustomerKeyData>(this);
        }
    }
}