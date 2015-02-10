using System;
using AutoMapper;

namespace CoachSeek.Data.Model
{
    public class CustomerData : NewCustomerData
    {
        public Guid Id { get; set; }


        public CustomerKeyData ToKeyData()
        {
            return Mapper.Map<CustomerData, CustomerKeyData>(this);
        }
    }
}