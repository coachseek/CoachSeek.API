using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class ServiceAddCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }
        public string ServiceName { get; set; }


        public NewServiceData ToData()
        {
            return Mapper.Map<ServiceAddCommand, NewServiceData>(this);
        }
    }
}
