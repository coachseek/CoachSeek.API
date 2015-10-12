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

        public IList<SessionInCourse> Sessions { get; private set; }


        private SessionInCourse FirstSession
        {
            get { return CalculateFirstSession(); }
        }


        public RepeatedSession(SessionAddCommand command, CoreData coreData)
            : base(command, coreData)
        {
            CalculateSingleSessions();
        }

        public RepeatedSession(RepeatedSession existingCourse, SessionUpdateCommand command, CoreData coreData)
            : base(existingCourse, command, coreData)
        {
            // Calculate the child sessions out for this update command.
            // Compare those sessions to the ones that already exist for the course.
            // Copy the updated session properties onto the existing sessions.
            // If the number of sessions has changed then create new or delete existing sessions. 
            // Ouch!

            UpdateSessions(existingCourse, existingCourse.Sessions, command, coreData);
        }

        public RepeatedSession(RepeatedSessionData data, 
                               IList<LocationData> locations,
                               IList<CoachData> coaches,
                               IList<ServiceData> services)

            : this(data.Id,
                   locations.Single(x => x.Id == data.Location.Id),
                   coaches.Single(x => x.Id == data.Coach.Id),
                   services.Single(x => x.Id == data.Service.Id),
                   data.Timing, 
                   data.Booking, 
                   data.Presentation, 
                   data.Repetition, 
                   data.Pricing, 
                   data.Sessions,
                   locations,
                   coaches,
                   services)
        { }


        public RepeatedSession(Guid id, 
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       RepetitionData repetition,
                       RepeatedSessionPricingData pricing,
                       IEnumerable<SingleSessionData> sessions,
                       IList<LocationData> locations,
                       IList<CoachData> coaches,
                       IList<ServiceData> services)
            : base(id, location, coach, service, timing, booking, presentation)
        {
            _repetition = new SessionRepetition(repetition);
            _pricing = new RepeatedSessionPricing(pricing.SessionPrice, pricing.CoursePrice);

            // Recreated the Session collection
            var courseSessions = new List<SessionInCourse>();
            foreach (var session in sessions)
            {
                var sessionLocation = locations.First(x => x.Id == session.Location.Id);
                var sessionCoach = coaches.First(x => x.Id == session.Coach.Id);
                var sessionService = services.First(x => x.Id == session.Service.Id);

                var courseSession = new SessionInCourse(session, sessionLocation, sessionCoach, sessionService);
                courseSessions.Add(courseSession);
            }

            Sessions = courseSessions.ToList();
        }


        public override SessionData ToData()
        {
            return Mapper.Map<RepeatedSession, RepeatedSessionData>(this);
        }

        public SingleSessionData ToChildSessionData()
        {
            var singleSession = Mapper.Map<RepeatedSession, SingleSessionData>(this);

            singleSession.ParentId = singleSession.Id;
            singleSession.Id = Guid.NewGuid();

            return singleSession;
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


        protected override void ValidateAdditional(SessionAddCommand command,
                                                   CoreData coreData,
                                                   ValidationException errors)
        {
            try
            {
                _repetition = new SessionRepetition(command.Repetition);
                _pricing = new RepeatedSessionPricing(command.Pricing, _repetition.SessionCount);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private SessionInCourse CalculateFirstSession()
        {
            var childSession = ToChildSessionData();

            return new SessionInCourse(childSession, _location.ToData(), _coach.ToData(), _service.ToData());
        }

        private void CalculateSingleSessions()
        {
            var calculator = SingleSessionListCalculatorSelector.SelectCalculator(Repetition.RepeatFrequency);
            Sessions = calculator.Calculate(FirstSession, Repetition.SessionCount);
        }

        private void UpdateSessions(RepeatedSession existingCourse, IList<SessionInCourse> existingSessions, SessionUpdateCommand command, CoreData coreData)
        {
            var updateSessions = new List<SessionInCourse>();

            foreach(var existingSession in existingSessions)
                updateSessions.Add(new SessionInCourse(existingCourse, existingSession, command, coreData));

            Sessions = updateSessions;
        }


        protected override string FormatSessionName()
        {
            var name = string.Format("{0} at {1} with {2} on {3} at {4}",
                                 Service.Name,
                                 Location.Name,
                                 Coach.Name,
                                 Timing.StartDate,
                                 Timing.StartTime);

            if (_repetition.IsRepeatedEveryDay)
                return string.Format("{0} repeated for {1} days", name, _repetition.SessionCount);
            if (_repetition.IsRepeatedEveryWeek)
                return string.Format("{0} repeated for {1} weeks", name, _repetition.SessionCount);
            
            throw new InvalidOperationException("Invalid repetition.");
        }
    }
}
