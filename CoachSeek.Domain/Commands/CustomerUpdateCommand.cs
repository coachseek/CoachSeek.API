using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class CustomerUpdateCommand : CustomerAddCommand, IIdentifiable
    {
        public Guid Id { get; set; }


        public new CustomerData ToData()
        {
            return Mapper.Map<CustomerUpdateCommand, CustomerData>(this);
        }
    }
}