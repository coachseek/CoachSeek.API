using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class ServiceUpdateCommand : ServiceAddCommand
    {
        public Guid Id { get; set; }


        public new ServiceData ToData()
        {
            return Mapper.Map<ServiceUpdateCommand, ServiceData>(this);
        }
    }
}
