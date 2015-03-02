using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class ServiceUpdateCommand : ServiceAddCommand, IIdentifiable
    {
        public Guid Id { get; set; }


        public ServiceData ToData()
        {
            return Mapper.Map<ServiceUpdateCommand, ServiceData>(this);
        }
    }
}
