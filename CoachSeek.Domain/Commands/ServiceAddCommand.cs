using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class ServiceAddCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ServiceDefaultsCommand Defaults { get; set; }

        public ServiceBookingCommand Booking { get; set; }
        public PricingCommand Pricing { get; set; }
        public RepetitionCommand Repetition { get; set; }

        public NewServiceData ToData()
        {
            return Mapper.Map<ServiceAddCommand, NewServiceData>(this);
        }
    }
}
