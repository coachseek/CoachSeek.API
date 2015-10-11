using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public class SessionGetByIdUseCase : SessionBaseUseCase, ISessionGetByIdUseCase
    {
        public async Task<SessionData> GetSessionAsync(Guid id)
       { 
            var sessionOrCourse = await GetExistingSessionOrCourseAsync(id);
            if (sessionOrCourse.IsNotFound())
                return null;
            return await AppendCustomerBookingsAsync(sessionOrCourse);
        }


        private async Task<SessionData> AppendCustomerBookingsAsync(Session sessionOrCourse)
        {
            if (sessionOrCourse is SingleSession)
                return await AppendCustomerBookingsToSessionAsync((SingleSession)sessionOrCourse);

            if (sessionOrCourse is RepeatedSession)
                return await AppendCustomerBookingsToCourseAsync((RepeatedSession)sessionOrCourse);

            throw new InvalidOperationException("Unexpected session type!");
        }

        private async Task<SessionData> AppendCustomerBookingsToSessionAsync(SingleSession session)
        {
            var sessionData = session.ToData();
            sessionData.Booking.Bookings = await BusinessRepository.GetCustomerBookingsBySessionIdAsync(Business.Id, session.Id);
            return sessionData;
        }

        private async Task<SessionData> AppendCustomerBookingsToCourseAsync(RepeatedSession course)
        {
            var courseData = course.ToData();
            var courseAndSessionBookings = await BusinessRepository.GetCustomerBookingsByCourseIdAsync(Business.Id, courseData.Id);

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
