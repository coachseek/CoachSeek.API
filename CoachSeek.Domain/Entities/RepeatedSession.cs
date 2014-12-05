using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Services;

namespace CoachSeek.Domain.Entities
{
    public class RepeatedSession : Session
    {
        private RepeatedSessionPricing _pricing;
        private SessionRepetition _repetition;

        public PricingData Pricing { get { return _pricing.ToData(); } }
        public RepetitionData Repetition { get { return _repetition.ToData(); } }

        public IList<SingleSession> Sessions { get { return CalculateSingleSessions(); } }


        private SingleSession FirstSession
        {
            get { return CalculateFirstSession(); }
        }


        public RepeatedSession(SessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation, data.Repetition, data.Pricing)
        { }


        public RepeatedSession(Guid id, 
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       RepetitionData repetition,
                       PricingData pricing)
        {
            Id = id;

            var errors = new ValidationException();

            ValidateAndCreateLocation(location, errors);
            ValidateAndCreateCoach(coach, errors);
            ValidateAndCreateService(service, errors);
            ValidateAndCreateSessionTiming(timing, service.Timing, errors);
            ValidateAndCreateSessionBooking(booking, service.Booking, errors);
            ValidateAndCreateSessionPresentation(presentation, service.Presentation, errors);

            ValidateAndCreateSessionRepetition(repetition, service.Repetition, errors);
            ValidateAndCreateSessionPricing(pricing, service.Pricing, errors);

            errors.ThrowIfErrors();
        }


        public override SessionData ToData()
        {
            return Mapper.Map<RepeatedSession, SessionData>(this);
        }

        public override bool IsOverlapping(Session otherSession)
        {
            if (otherSession is SingleSession)
                return IsOverlapping((SingleSession)otherSession);

            if (otherSession is RepeatedSession)
                return IsOverlapping((RepeatedSession)otherSession);

            return false;
        }


        private bool IsOverlapping(SingleSession otherSession)
        {
            return Sessions.Any(session => session.IsOverlapping(otherSession));
        }

        private bool IsOverlapping(RepeatedSession otherSessions)
        {
            return Sessions.Any(otherSessions.IsOverlapping);
        }


        private void ValidateAndCreateSessionRepetition(RepetitionData sessionRepetition, RepetitionData serviceRepetition, ValidationException errors)
        {
            try
            {
                _repetition = new SessionRepetition(sessionRepetition, serviceRepetition);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateSessionPricing(PricingData sessionPricing, PricingData servicePricing, ValidationException errors)
        {
            try
            {
                _pricing = new RepeatedSessionPricing(sessionPricing, servicePricing, _repetition);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private SingleSession CalculateFirstSession()
        {
            var data = ToData();
            data.Repetition = null;

            return new SingleSession(data, _location.ToData(), _coach.ToData(), _service.ToData());
        }

        private IList<SingleSession> CalculateSingleSessions()
        {
            var calculator = SingleSessionListCalculatorSelector.SelectCalculator(Repetition.RepeatFrequency);
            return calculator.Calculate(FirstSession, Repetition.SessionCount);
        }
    }
}
