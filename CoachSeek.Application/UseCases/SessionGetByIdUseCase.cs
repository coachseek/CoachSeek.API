using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public class SessionGetByIdUseCase : SessionBaseUseCase, ISessionGetByIdUseCase
    {
        public SessionData GetSession(Guid id)
       { 
            var sessionOrCourse = GetExistingSessionOrCourse(id);
            if (sessionOrCourse.IsNotFound())
                return null;
            return AppendCustomerBookings(sessionOrCourse);
        }


        private SessionData AppendCustomerBookings(Session sessionOrCourse)
        {
            if (sessionOrCourse is SingleSession)
                return AppendCustomerBookingsToSession((SingleSession)sessionOrCourse);

            if (sessionOrCourse is RepeatedSession)
                return AppendCustomerBookingsToCourse((RepeatedSession)sessionOrCourse);

            throw new InvalidOperationException("Unexpected session type!");
        }

        private SessionData AppendCustomerBookingsToSession(SingleSession session)
        {
            var sessionData = session.ToData();
            sessionData.Booking.Bookings = BusinessRepository.GetCustomerBookingsBySessionId(BusinessId, session.Id);
            return sessionData;
        }

        private SessionData AppendCustomerBookingsToCourse(RepeatedSession course)
        {
            var courseData = course.ToData();
            var courseAndSessionBookings = BusinessRepository.GetCustomerBookingsByCourseId(BusinessId, courseData.Id);

            courseData.Booking.Bookings = ExtractCourseBookings(courseAndSessionBookings).ToList();

            foreach (var session in courseData.Sessions)
            {
                var sessionBookings = ExtractSessionBookings(courseAndSessionBookings, session.Id).ToList();
                session.Booking.Bookings = sessionBookings;
            }

            return courseData;
        }

        private IEnumerable<CustomerBookingData> ExtractCourseBookings(IList<CustomerBookingData> courseAndSessionBookings)
        {
            var parentlessBookings = courseAndSessionBookings.Where(x => x.ParentId == null).ToList();
            var sessionInCourseBookings = courseAndSessionBookings.Where(x => x.ParentId != null).ToList();

            var courseBookings = new List<CustomerBookingData>();

            foreach (var parentlessBooking in parentlessBookings)
                if (sessionInCourseBookings.Any(x => x.ParentId == parentlessBooking.Id))
                    courseBookings.Add(parentlessBooking);

            return courseBookings;
        }

        private IEnumerable<CustomerBookingData> ExtractSessionBookings(IList<CustomerBookingData> courseAndSessionBookings, Guid sessionId)
        {
            var parentlessBookings = courseAndSessionBookings.Where(x => x.ParentId == null).ToList();
            var sessionInCourseBookings = courseAndSessionBookings.Where(x => x.ParentId != null).ToList();

            var sessionBookings = new List<CustomerBookingData>(sessionInCourseBookings);

            foreach (var parentlessBooking in parentlessBookings)
                if (!sessionInCourseBookings.Any(x => x.ParentId == parentlessBooking.Id))
                    sessionBookings.Add(parentlessBooking);

            return sessionBookings.Where(x => x.SessionId == sessionId);
        }
    }
}
