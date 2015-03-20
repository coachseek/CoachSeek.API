using System;
using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
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

        public Guid? ParentId { get; set; }
        public SingleSessionPricingData Pricing { get { return _pricing.ToData(); } }

        public PointInTime Start { get { return _timing.Start; } }
        public PointInTime Finish { get { return _timing.Finish; } }


        protected SingleSession(SessionAddCommand command, CoreData coreData)
            : base(command, coreData)
        { }

        protected SingleSession(SingleSession existingSession, SessionUpdateCommand command, CoreData coreData)
            : base(existingSession, command, coreData)
        { }

        public SingleSession(SingleSessionData data, CoreData coreData)
            : this(data, coreData.Location, coreData.Coach, coreData.Service)
        { }

        public SingleSession(SingleSessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation, data.Pricing, data.ParentId)
        { }

        protected SingleSession(Guid id,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       SingleSessionPricingData pricing, 
                       Guid? parentId = null)
            : base(id, location, coach, service, timing, booking, presentation)
        {
            ParentId = parentId;
            _pricing = new SingleSessionPricing(pricing.SessionPrice);
        }


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

        public SingleSessionData ToData()
        {
            return Mapper.Map<SingleSession, SingleSessionData>(this);
        }


        private bool IsOverlapping(SingleSession otherSession)
        {
            if (IsNullOrSameSession(otherSession))
                return false;

            if (IsSameCoach(otherSession))
                return (IsFullyOverlapping(otherSession) || IsPartiallyOverlapping(otherSession));

            return false;
        }

        private bool IsSameCoach(SingleSession otherSession)
        {
            return Coach.Id == otherSession.Coach.Id;
        }

        private bool IsFullyOverlapping(SingleSession otherSession)
        {
            return Start.IsSameAs(otherSession.Start) 
                && Finish.IsSameAs(otherSession.Finish);
        }

        private bool IsPartiallyOverlapping(SingleSession otherSession)
        {
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

        protected override void ValidateAdditional(SessionAddCommand command,
                                                   CoreData coreData,
                                                   ValidationException errors)                       
        {
            ValidateAndCreateSessionPricing(command.Pricing, errors);
        }

        private void ValidateAndCreateSessionPricing(PricingCommand sessionPricing, ValidationException errors)
        {
            try
            {
                _pricing = new SingleSessionPricing(sessionPricing);
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
