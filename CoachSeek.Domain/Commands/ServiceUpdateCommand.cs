﻿using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class ServiceUpdateCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public ServiceData ToData()
        {
            return Mapper.Map<ServiceUpdateCommand, ServiceData>(this);
        }
    }
}
