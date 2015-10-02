using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
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


        public async Task<SessionSearchData> SearchForSessionsAsync(string startDate, 
                                                                    string endDate, 
                                                                    Guid? coachId = null, 
                                                                    Guid? locationId = null, 
                                                                    Guid? serviceId = null)
        {
            var parameters = PackageUpParameters(startDate, endDate, coachId, locationId, serviceId);
            Validate(parameters);

            var searchData = await GetSessionsCoursesAndBookingsAsync();

            var standaloneSessions = FindMatchingStandaloneSessions(searchData.Sessions, parameters);
            AddBookingsToSessions(standaloneSessions, searchData.Bookings);

            var courseSessions = FindMatchingCourseSessions(searchData.Sessions, parameters);
            var courses = FindMatchingCourses(searchData.Courses, courseSessions);
            AddBookingsToCourses(courses, searchData.Bookings);

            return new SessionSearchData(standaloneSessions, courses);
        }

        public async Task<SessionSearchData> SearchForOnlineBookableSessionsAsync(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null)
        {
            var parameters = PackageUpParameters(startDate, endDate, coachId, locationId, serviceId);
            Validate(parameters);

            var searchData = await GetSessionsCoursesAndBookingsAsync();

            var standaloneSessions = FindMatchingOnlineBookableStandaloneSessions(searchData.Sessions, parameters);
            AddBookingsToSessions(standaloneSessions, searchData.Bookings);

            var courseSessions = FindMatchingCourseSessions(searchData.Sessions, parameters);
            var courses = FindMatchingOnlineBookableCourses(searchData.Courses, courseSessions);
            AddBookingsToCourses(courses, searchData.Bookings);

            return new SessionSearchData(standaloneSessions, courses);
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

        private async Task<SessionsCoursesAndBookings> GetSessionsCoursesAndBookingsAsync()
        {
            var sessionsTask = BusinessRepository.GetAllSessionsAsync(Business.Id);
            var coursesTask = BusinessRepository.GetAllCoursesAsync(Business.Id);
            var bookingsTask = BusinessRepository.GetAllCustomerBookingsAsync(Business.Id);
            await Task.WhenAll(bookingsTask, sessionsTask, coursesTask);
            return new SessionsCoursesAndBookings(sessionsTask.Result, coursesTask.Result, bookingsTask.Result);
        }

        private IList<SingleSessionData> FindMatchingSessions(IList<SingleSessionData> sessions, SearchParameters parameters)
        {
            var filteredSessions = FilterSessionsByDate(sessions, parameters.StartDate, parameters.EndDate);
            filteredSessions = FilterSessionsByCoachLocationService(filteredSessions, parameters.CoachId, parameters.LocationId, parameters.ServiceId);

            return OrderSessionsByStartDateAndTime(filteredSessions);
        }

        private IList<SingleSessionData> FindMatchingStandaloneSessions(IList<SingleSessionData> sessions, SearchParameters parameters)
        {
            var matchingSessions = FindMatchingSessions(sessions, parameters);
            return matchingSessions.Where(x => x.ParentId == null).ToList();
        }

        private IList<SingleSessionData> FindMatchingOnlineBookableStandaloneSessions(IList<SingleSessionData> sessions, SearchParameters parameters)
        {
            var matchingSessions = FindMatchingStandaloneSessions(sessions, parameters);
            return matchingSessions.Where(x => x.Booking.IsOnlineBookable).ToList();
        }

        private IList<SingleSessionData> FindMatchingCourseSessions(IList<SingleSessionData> sessions, SearchParameters parameters)
        {
            var matchingSessions = FindMatchingSessions(sessions, parameters);
            return matchingSessions.Where(x => x.ParentId != null).ToList();
        }

        private void AddBookingsToSessions(IList<SingleSessionData> sessions, IList<CustomerBookingData> bookings)
        {
            foreach (var session in sessions)
                AddBookingsToSession(session, bookings);
        }

        private void AddBookingsToSession(SingleSessionData session, IList<CustomerBookingData> bookings)
        {
            session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
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

        private IList<RepeatedSessionData> FindMatchingCourses(IList<RepeatedSessionData> courses, IList<SingleSessionData> courseSessions)
        {
            var matchingCourses = new List<RepeatedSessionData>();
            foreach (var courseSession in courseSessions)
            {
                if (courseSession.ParentId == null)
                    continue;
                var matchingCourse = courses.Single(x => x.Id == courseSession.ParentId);
                if (!matchingCourses.Contains(matchingCourse))
                    matchingCourses.Add(matchingCourse);
            }

            return matchingCourses;
        }

        private IList<RepeatedSessionData> FindMatchingOnlineBookableCourses(IList<RepeatedSessionData> courses, IList<SingleSessionData> courseSessions)
        {
            var matchingCourses = FindMatchingCourses(courses, courseSessions);

            return matchingCourses.Where(x => x.Booking.IsOnlineBookable).ToList();
        }

        private void AddBookingsToCourses(IList<RepeatedSessionData> courses, IList<CustomerBookingData> bookings)
        {
            foreach (var course in courses)
            {
                course.Booking.Bookings = bookings.Where(x => x.SessionId == course.Id).ToList();

                foreach (var session in course.Sessions)
                    session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
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
            if (coach.IsNotFound())
                throw new CoachInvalid(coachId.Value);
        }

        private void ValidateLocation(Guid? locationId)
        {
            if (!locationId.HasValue)
                return;
            LocationGetByIdUseCase.Initialise(Context);
            var location = LocationGetByIdUseCase.GetLocation(locationId.Value);
            if (location.IsNotFound())
                throw new LocationInvalid(locationId.Value);
        }

        private void ValidateService(Guid? serviceId)
        {
            if (!serviceId.HasValue)
                return;
            ServiceGetByIdUseCase.Initialise(Context);
            var service = ServiceGetByIdUseCase.GetService(serviceId.Value);
            if (service.IsNotFound())
                throw new ServiceInvalid(serviceId.Value);
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
                throw new StartDateAfterEndDate(startDate, endDate);
        }

        private static void ValidateStartDate(string startDate, ValidationException errors)
        {
            if (string.IsNullOrEmpty(startDate))
            {
                errors.Add(new StartDateRequired());
                return;
            }

            try
            {
                var start = new Date(startDate);
            }
            catch (DateInvalid ex)
            {
                errors.Add(new StartDateInvalid(ex));
            }
        }

        private static void ValidateEndDate(string endDate, ValidationException errors)
        {
            if (string.IsNullOrEmpty(endDate))
            {
                errors.Add(new EndDateRequired());
                return;
            }

            try
            {
                var end = new Date(endDate);
            }
            catch (DateInvalid ex)
            {
                errors.Add(new EndDateInvalid(ex));
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

        private class SessionsCoursesAndBookings
        {
            public IList<SingleSessionData> Sessions { get; set; }
            public IList<RepeatedSessionData> Courses { get; set; }
            public IList<CustomerBookingData> Bookings { get; set; }

            public SessionsCoursesAndBookings(IList<SingleSessionData> sessions, 
                                              IList<RepeatedSessionData> courses, 
                                              IList<CustomerBookingData> bookings)
            {
                Sessions = sessions;
                Courses = courses;
                Bookings = bookings;
            }
        }
    }
}
