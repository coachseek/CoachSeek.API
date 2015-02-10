using System;
using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    /// <summary>
    /// A single session is used as the individual sessions inside a RepeatedSession, or
    /// as an single standalone session. 
    /// </summary>
    public class SingleSession : Session
    {
        protected SingleSessionPricing _pricing;

        public PricingData Pricing { get { return _pricing.ToData(); } }

        public PointInTime Start { get { return _timing.Start; } }
        public PointInTime Finish { get { return _timing.Finish; } }


        //public SingleSession(SessionData data)
        //{
        //    Id = data.Id;
        //    _location = new Location(data.Location);
        //    _coach = new Coach(data.Coach);
        //}

        public SingleSession(SessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation, data.Pricing)
        { }

        public SingleSession(Guid id,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       PricingData pricing)
        {
            Id = id;

            Validate(location, coach, service, timing, booking, presentation, pricing);
        }

        private void Validate(LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       PricingData pricing)
        {
            var errors = new ValidationException();

            ValidateAndCreateLocation(location, errors);
            ValidateAndCreateCoach(coach, errors);
            ValidateAndCreateService(service, errors);
            ValidateAndCreateSessionTiming(timing, service.Timing, errors);
            ValidateAndCreateSessionBooking(booking, service.Booking, errors);
            ValidateAndCreateSessionPresentation(presentation, service.Presentation, errors);
            ValidateAndCreateSessionPricing(pricing, service.Pricing, errors);

            ValidateAdditional(errors, location, coach, service, timing, booking, presentation, pricing);

            errors.ThrowIfErrors();
        }

        protected virtual void ValidateAdditional(ValidationException errors, 
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       PricingData pricing)
        { }


        public override bool IsOverlapping(Session otherSession)
        {
            if (otherSession is SingleSession)
                return IsOverlapping((SingleSession)otherSession);

            if (otherSession is RepeatedSession)
                return IsOverlapping((RepeatedSession)otherSession);

            return false;
        }

        public bool Contains(PointInTime pointInTime)
        {
            return pointInTime.IsAfter(Start) && Finish.IsAfter(pointInTime);
        }

        public override SessionData ToData()
        {
            return Mapper.Map<SingleSession, SessionData>(this);
        }

        public SingleSession Clone()
        {
            var sessionData = ToData();
            sessionData.Id = Guid.NewGuid();

            return new SingleSession(sessionData, _location.ToData(), _coach.ToData(), _service.ToData());
        }

        public SingleSession Clone(Date startDate)
        {
            var sessionData = ToData();
            sessionData.Id = Guid.NewGuid();

            var timing = sessionData.Timing;
            sessionData.Timing = new SessionTimingData(startDate.ToData(), timing.StartTime, timing.Duration);

            return new SingleSession(sessionData, _location.ToData(), _coach.ToData(), _service.ToData());
        }


        private bool IsOverlapping(SingleSession otherSession)
        {
            if (IsNullOrSameSession(otherSession))
                return false;

            return (Contains(otherSession.Start) ||
                    Contains(otherSession.Finish) ||
                    otherSession.Contains(Start) ||
                    otherSession.Contains(Finish));
        }

        private bool IsOverlapping(RepeatedSession repeatedSession)
        {
            if (IsNullOrHasNoSessions(repeatedSession))
                return false;

            return repeatedSession.Sessions.Any(IsOverlapping);
        }


        private void ValidateAndCreateSessionPricing(PricingData sessionPricing, PricingData servicePricing, ValidationException errors)
        {
            try
            {
                _pricing = new SingleSessionPricing(sessionPricing, servicePricing);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private bool IsNullOrHasNoSessions(RepeatedSession repeatedSession)
        {
            return repeatedSession == null || repeatedSession.Sessions.Count == 0;
        }
    }
}
