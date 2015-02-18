using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Services;

namespace CoachSeek.Domain.Entities
{
    public class RepeatedSession : Session
    {
        private RepeatedSessionPricing _pricing;
        private SessionRepetition _repetition;

        public RepeatedSessionPricingData Pricing { get { return _pricing.ToData(); } }
        public RepetitionData Repetition { get { return _repetition.ToData(); } }

        public IList<SingleSession> Sessions { get; private set; }


        private SingleSession FirstSession
        {
            get { return CalculateFirstSession(); }
        }


        public RepeatedSession(SessionAddCommand command, LocationData location, CoachData coach, ServiceData service)
            : base(command, location, coach, service)
        {
            CalculateSingleSessions();
        }

        public RepeatedSession(RepeatedSessionData data, LocationData location, CoachData coach, ServiceData service)
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
                       RepeatedSessionPricingData pricing)
            : base(id, location, coach, service, timing, booking, presentation)
        {
            _repetition = new SessionRepetition(repetition);
            _pricing = new RepeatedSessionPricing(pricing.SessionPrice, pricing.CoursePrice);

            CalculateSingleSessions();
        }


        public RepeatedSessionData ToData()
        {
            return Mapper.Map<RepeatedSession, RepeatedSessionData>(this);
        }

        public SingleSessionData ToSingleSessionData()
        {
            return Mapper.Map<RepeatedSession, SingleSessionData>(this);
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


        protected override void ValidateAdditional(ValidationException errors,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionAddCommand command)
        {
            ValidateAndCreateSessionRepetition(command.Repetition, service.Repetition, errors);
            ValidateAndCreateSessionPricing(command.Pricing, service.Pricing, errors);
        }

        //private void Validate(LocationData location, CoachData coach, ServiceData service, SessionTimingData timing,
        //    SessionBookingData booking, PresentationData presentation, RepetitionData repetition, PricingData pricing)
        //{
        //    var errors = new ValidationException();

        //    ValidateAndCreateLocation(location, errors);
        //    ValidateAndCreateCoach(coach, errors);
        //    ValidateAndCreateService(service, errors);
        //    ValidateAndCreateSessionTiming(timing, service.Timing, errors);
        //    ValidateAndCreateSessionBooking(booking, service.Booking, errors);
        //    ValidateAndCreateSessionPresentation(presentation, service.Presentation, errors);

        //    ValidateAndCreateSessionRepetition(repetition, service.Repetition, errors);
        //    ValidateAndCreateSessionPricing(pricing, service.Pricing, errors);

        //    errors.ThrowIfErrors();
        //}

        private void ValidateAndCreateSessionRepetition(RepetitionCommand sessionRepetition, RepetitionData serviceRepetition, ValidationException errors)
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

        private void ValidateAndCreateSessionPricing(PricingCommand sessionPricing, RepeatedSessionPricingData servicePricing, ValidationException errors)
        {
            try
            {
                _pricing = new RepeatedSessionPricing(sessionPricing, servicePricing);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private SingleSession CalculateFirstSession()
        {
            var data = ToSingleSessionData();

            return new SingleSession(data, _location.ToData(), _coach.ToData(), _service.ToData());
        }

        private void CalculateSingleSessions()
        {
            var calculator = SingleSessionListCalculatorSelector.SelectCalculator(Repetition.RepeatFrequency);
            Sessions = calculator.Calculate(FirstSession, Repetition.SessionCount);
        }
    }
}
