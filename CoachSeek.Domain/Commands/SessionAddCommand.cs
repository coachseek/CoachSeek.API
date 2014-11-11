using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class SessionAddCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }

        public ServiceKeyCommand Service { get; set; }
        public LocationKeyCommand Location { get; set; }
        public CoachKeyCommand Coach { get; set; }

        public SessionTimingCommand Timing { get; set; }
        public SessionBookingCommand Booking { get; set; }
        public PricingCommand Pricing { get; set; }
        public RepetitionCommand Repetition { get; set; }
        public PresentationCommand Presentation { get; set; }


        public NewSessionData ToData()
        {
            return Mapper.Map<SessionAddCommand, NewSessionData>(this);
        }
    }
}
