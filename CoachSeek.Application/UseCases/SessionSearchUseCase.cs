using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class SessionSearchUseCase : BaseUseCase, ISessionSearchUseCase
    {
        private ICoachGetByIdUseCase CoachGetByIdUseCase { get; set; }
        private ILocationGetByIdUseCase LocationGetByIdUseCase { get; set; }
        private IServiceGetByIdUseCase ServiceGetByIdUseCase { get; set; }


        public SessionSearchUseCase(ICoachGetByIdUseCase coachGetByIdUseCase,
                                    ILocationGetByIdUseCase locationGetByIdUseCase,
                                    IServiceGetByIdUseCase serviceGetByIdUseCase)
        {
            CoachGetByIdUseCase = coachGetByIdUseCase;
            LocationGetByIdUseCase = locationGetByIdUseCase;
            ServiceGetByIdUseCase = serviceGetByIdUseCase;
        }


        public SessionSearchData SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null)
        {
            var parameters = PackageUpParameters(startDate, endDate, coachId, locationId, serviceId);
            Validate(parameters);
            var matchingStandaloneSessions = FindMatchingStandaloneSessions(parameters);

            var bookings = BusinessRepository.GetAllCustomerBookings(Business.Id);
            AddBookingsToSessions(matchingStandaloneSessions, bookings);

            var matchingCourseSessions = FindMatchingCourseSessions(parameters);
            var matchingCourses = FindMatchingCourses(matchingCourseSessions);
            AddBookingsToCourses(matchingCourses, bookings);

            return new SessionSearchData(matchingStandaloneSessions, matchingCourses);
        }

        public SessionSearchData SearchForOnlineBookableSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null)
        {
            var parameters = PackageUpParameters(startDate, endDate, coachId, locationId, serviceId);
            Validate(parameters);
            var matchingStandaloneSessions = FindMatchingOnlineBookableStandaloneSessions(parameters);

            var bookings = BusinessRepository.GetAllCustomerBookings(Business.Id);
            AddBookingsToSessions(matchingStandaloneSessions, bookings);

            var matchingCourseSessions = FindMatchingCourseSessions(parameters);
            var matchingCourses = FindMatchingOnlineBookableCourses(matchingCourseSessions);
            AddBookingsToCourses(matchingCourses, bookings);

            return new SessionSearchData(matchingStandaloneSessions, matchingCourses);
        }

        // Deprecated. TODO: Remove
        public IList<SingleSessionData> SearchForSessionsOld(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null)
        {
            var parameters = PackageUpParameters(startDate, endDate, coachId, locationId, serviceId);
            Validate(parameters);

            var matchingSessions = FindMatchingSessions(parameters);
            var bookings = BusinessRepository.GetAllCustomerBookings(Business.Id);
            AddBookingsToSessions(matchingSessions, bookings);

            return matchingSessions;
        }

        private SearchParameters PackageUpParameters(string startDate, string endDate, Guid? coachId, Guid? locationId, Guid? serviceId)
        {
            return new SearchParameters
            {
                StartDate = startDate,
                EndDate = endDate,
                CoachId = coachId,
                LocationId = locationId,
                ServiceId = serviceId
            };
        }

        private IList<SingleSessionData> FindMatchingSessions(SearchParameters parameters)
        {
            var sessions = BusinessRepository.GetAllSessions(Business.Id);
            sessions = FilterSessionsByDate(sessions, parameters.StartDate, parameters.EndDate);
            sessions = FilterSessionsByCoachLocationService(sessions, parameters.CoachId, parameters.LocationId, parameters.ServiceId);

            return OrderSessionsByStartDateAndTime(sessions);
        }

        private IList<SingleSessionData> FindMatchingStandaloneSessions(SearchParameters parameters)
        {
            var sessions = FindMatchingSessions(parameters);
            return sessions.Where(x => x.ParentId == null).ToList();
        }

        private IList<SingleSessionData> FindMatchingOnlineBookableStandaloneSessions(SearchParameters parameters)
        {
            var sessions = FindMatchingStandaloneSessions(parameters);
            return sessions.Where(x => x.Booking.IsOnlineBookable).ToList();
        }

        private IList<SingleSessionData> FindMatchingCourseSessions(SearchParameters parameters)
        {
            var sessions = FindMatchingSessions(parameters);
            return sessions.Where(x => x.ParentId != null).ToList();
        }

        private void AddBookingsToSessions(IList<SingleSessionData> sessions, IList<CustomerBookingData> bookings)
        {
            foreach (var session in sessions)
                AddBookingsToSession(session, bookings);
        }

        private void AddBookingsToSession(SingleSessionData session, IList<CustomerBookingData> bookings)
        {
            session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
            session.Booking.BookingCount = session.Booking.Bookings.Count;
        }

        private IList<SingleSessionData> FilterSessionsByDate(IList<SingleSessionData> sessions, string startDate, string endDate)
        {
            return sessions.Where(x => new Date(x.Timing.StartDate).IsOnOrAfter(new Date(startDate)))
                           .Where(x => new Date(x.Timing.StartDate).IsOnOrBefore(new Date(endDate))).ToList();
        }

        private IList<SingleSessionData> FilterSessionsByCoachLocationService(IEnumerable<SingleSessionData> sessions, Guid? coachId, Guid? locationId, Guid? serviceId)
        {
            if (coachId.HasValue)
                sessions = sessions.Where(x => x.Coach.Id == coachId);
            if (locationId.HasValue)
                sessions = sessions.Where(x => x.Location.Id == locationId);
            if (serviceId.HasValue)
                sessions = sessions.Where(x => x.Service.Id == serviceId);

            return sessions.ToList();
        }

        private IList<SingleSessionData> OrderSessionsByStartDateAndTime(IList<SingleSessionData> sessions)
        {
            return sessions.OrderBy(x => x.Timing.StartDate)
                           .ThenBy(x => CreateOrderableStartTime(x.Timing.StartTime)).ToList();
        }

        private IList<RepeatedSessionData> FindMatchingCourses(IList<SingleSessionData> matchingSessions)
        {
            var courses = BusinessRepository.GetAllCourses(Business.Id);
            var matchingCourses = new List<RepeatedSessionData>();
            foreach (var matchingSession in matchingSessions)
            {
                if (matchingSession.ParentId == null)
                    continue;
                var matchingCourse = courses.Single(x => x.Id == matchingSession.ParentId);
                if (!matchingCourses.Contains(matchingCourse))
                    matchingCourses.Add(matchingCourse);
            }

            return matchingCourses;
        }

        private IList<RepeatedSessionData> FindMatchingOnlineBookableCourses(IList<SingleSessionData> matchingSessions)
        {
            var courses = FindMatchingCourses(matchingSessions);

            return courses.Where(x => x.Booking.IsOnlineBookable).ToList();
        }

        private void AddBookingsToCourses(IList<RepeatedSessionData> courses, IList<CustomerBookingData> bookings)
        {
            foreach (var course in courses)
            {
                course.Booking.Bookings = bookings.Where(x => x.SessionId == course.Id).ToList();
                course.Booking.BookingCount = course.Booking.Bookings.Count;

                foreach (var session in course.Sessions)
                {
                    session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
                    session.Booking.BookingCount = session.Booking.Bookings.Count;
                }
            }
        }

        private static string CreateOrderableStartTime(string startTime)
        {
            return startTime.Length == 4 ? string.Format("0{0}", startTime) : startTime;
        }

        private void Validate(SearchParameters parameters)
        {
            ValidateDates(parameters.StartDate, parameters.EndDate);
            ValidateCoach(parameters.CoachId);
            ValidateLocation(parameters.LocationId);
            ValidateService(parameters.ServiceId);
        }

        private void ValidateCoach(Guid? coachId)
        {
            if (!coachId.HasValue)
                return;
            CoachGetByIdUseCase.Initialise(Context);
            var coach = CoachGetByIdUseCase.GetCoach(coachId.Value);
            if (coach == null)
                throw new ValidationException("Not a valid coachId.", "coachId");
        }

        private void ValidateLocation(Guid? locationId)
        {
            if (!locationId.HasValue)
                return;
            LocationGetByIdUseCase.Initialise(Context);
            var coach = LocationGetByIdUseCase.GetLocation(locationId.Value);
            if (coach == null)
                throw new ValidationException("Not a valid locationId.", "locationId");
        }

        private void ValidateService(Guid? serviceId)
        {
            if (!serviceId.HasValue)
                return;
            ServiceGetByIdUseCase.Initialise(Context);
            var service = ServiceGetByIdUseCase.GetService(serviceId.Value);
            if (service == null)
                throw new ValidationException("Not a valid serviceId.", "serviceId");
        }

        private void ValidateDates(string searchStartDate, string searchEndDate)
        {
            var errors = new ValidationException();
            ValidateStartDate(searchStartDate, errors);
            ValidateEndDate(searchEndDate, errors);
            errors.ThrowIfErrors();

            var startDate = new Date(searchStartDate);
            var endDate = new Date(searchEndDate);

            if (startDate.IsAfter(endDate))
                throw new ValidationException("The startDate is after the endDate.", "startDate");
        }

        private static void ValidateStartDate(string startDate, ValidationException errors)
        {
            if (string.IsNullOrEmpty(startDate))
            {
                errors.Add("The startDate is missing.", "startDate");
                return;
            }

            try
            {
                var start = new Date(startDate);
            }
            catch (InvalidDate)
            {
                errors.Add("The startDate is not a valid date.", "startDate");
            }
        }

        private static void ValidateEndDate(string endDate, ValidationException errors)
        {
            if (string.IsNullOrEmpty(endDate))
            {
                errors.Add("The endDate is missing.", "endDate");
                return;
            }

            try
            {
                var end = new Date(endDate);
            }
            catch (InvalidDate)
            {
                errors.Add("The endDate is not a valid date.", "endDate");
            }
        }


        private struct SearchParameters
        {
            public string StartDate;
            public string EndDate;
            public Guid? CoachId;
            public Guid? LocationId;
            public Guid? ServiceId;
        }
    }
}
