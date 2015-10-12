using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class SessionInCourse : SingleSession
    {
        public SessionInCourse(SessionAddCommand command, CoreData coreData)
            : base(command, coreData)
        { }

        public SessionInCourse(SingleSession existingSession, SessionUpdateCommand command, CoreData coreData)
            : base(existingSession, command, coreData)
        {
            // Use this constructor for updating just this single session (not the whole course as well).
            ParentId = existingSession.ParentId;
        }

        public SessionInCourse(RepeatedSession existingCourse, SingleSession existingSession, SessionUpdateCommand command, CoreData coreData)
            : base(existingSession, command, coreData)
        {
            // Use this constructor for updating this session as part of an update to the whole course.
            // Override the Id (which will be the one of the Course)
            // and also set the ParentId

            Id = existingSession.Id;
            ParentId = existingSession.ParentId;

            var existingCourseStartDate = new Date(existingCourse.Timing.StartDate);
            var updatedCourseStartDate = new Date(command.Timing.StartDate);
            var dayOffset = updatedCourseStartDate.CalculateDayOffsetTo(existingCourseStartDate);

            var updatedSessionStartDate = new Date(existingSession.Timing.StartDate, dayOffset);

            _timing = new SessionTiming(new SessionTimingData(updatedSessionStartDate.ToString(), command.Timing.StartTime, command.Timing.Duration));
        }

        public SessionInCourse(SingleSessionData data, CoreData coreData)
            : this(data, coreData.Location, coreData.Coach, coreData.Service)
        { }

        public SessionInCourse(SingleSessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation, data.Pricing, data.ParentId)
        { }

        public SessionInCourse(Guid id,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       SingleSessionPricingData pricing, 
                       Guid? parentId = null)
            : base(id, location, coach, service, timing, booking, presentation, pricing, parentId)
        { }


        public SessionInCourse Clone()
        {
            var sessionData = (SingleSessionData)ToData();
            sessionData.Id = Guid.NewGuid();

            return new SessionInCourse(sessionData, _location.ToData(), _coach.ToData(), _service.ToData());
        }

        public SessionInCourse Clone(Date startDate)
        {
            var sessionData = (SingleSessionData)ToData();
            sessionData.ParentId = ParentId;
            sessionData.Id = Guid.NewGuid();

            var timing = sessionData.Timing;
            sessionData.Timing = new SessionTimingData(startDate.ToString(), timing.StartTime, timing.Duration);

            return new SessionInCourse(sessionData, _location.ToData(), _coach.ToData(), _service.ToData());
        }
    }
}
