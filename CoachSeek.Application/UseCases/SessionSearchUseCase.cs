using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class SessionSearchUseCase : BaseUseCase, ISessionSearchUseCase
    {
        public Guid BusinessId { get; set; }

        private ICoachGetByIdUseCase CoachGetByIdUseCase { get; set; }
        private ILocationGetByIdUseCase LocationGetByIdUseCase { get; set; }

        public SessionSearchUseCase(IBusinessRepository businessRepository,
                                    ICoachGetByIdUseCase coachGetByIdUseCase,
                                    ILocationGetByIdUseCase locationGetByIdUseCase)
            : base(businessRepository)
        {
            CoachGetByIdUseCase = coachGetByIdUseCase;
            LocationGetByIdUseCase = locationGetByIdUseCase;
        }


        public IList<SingleSessionData> SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null)
        {
            Validate(startDate, endDate, coachId, locationId);
            var business = GetBusiness(BusinessId);
            var matchingSessions = new List<SingleSessionData>();

            var standaloneSessions = SearchForStandaloneSessions(business, startDate, endDate, coachId, locationId);
            matchingSessions.AddRange(standaloneSessions);

            var courseSessions = SearchForCourseSessions(business, startDate, endDate, coachId, locationId);
            matchingSessions.AddRange(courseSessions);

            return matchingSessions.OrderBy(x => x.Timing.StartDate).ToList();
        }


        private IEnumerable<SingleSessionData> SearchForStandaloneSessions(Business business, string startDate, string endDate, Guid? coachId = null, Guid? locationId = null)
        {
            var sessionQuery = business.Sessions.Where(x => new Date(x.Timing.StartDate).IsOnOrAfter(new Date(startDate)))
                                                .Where(x => new Date(x.Timing.StartDate).IsOnOrBefore(new Date(endDate)));

            if (coachId.HasValue)
                sessionQuery = sessionQuery.Where(x => x.Coach.Id == coachId);

            if (locationId.HasValue)
                sessionQuery = sessionQuery.Where(x => x.Location.Id == locationId);

            return sessionQuery.ToList();
        }

        private IEnumerable<SingleSessionData> SearchForCourseSessions(Business business, string startDate, string endDate, Guid? coachId = null, Guid? locationId = null)
        {
            var courseQuery = business.Courses.AsEnumerable();

            if (coachId.HasValue)
                courseQuery = courseQuery.Where(x => x.Coach.Id == coachId);
            if (locationId.HasValue)
                courseQuery = courseQuery.Where(x => x.Location.Id == locationId);

            var matchingSessions = new List<SingleSessionData>();

            foreach (var course in courseQuery.ToList())
            {
                var sessions = course.Sessions.Where(x => new Date(x.Timing.StartDate).IsOnOrAfter(new Date(startDate)))
                                              .Where(x => new Date(x.Timing.StartDate).IsOnOrBefore(new Date(endDate)));

                matchingSessions.AddRange(sessions);
            }

            return matchingSessions;
        }

        private void Validate(string searchStartDate, string searchEndDate, Guid? coachId, Guid? locationId)
        {
            ValidateDates(searchStartDate, searchEndDate);
            ValidateCoach(coachId);
            ValidateLocation(locationId);
        }

        private void ValidateCoach(Guid? coachId)
        {
            if (!coachId.HasValue)
                return;
            CoachGetByIdUseCase.BusinessId = BusinessId;
            var coach = CoachGetByIdUseCase.GetCoach(coachId.Value);
            if (coach == null)
                throw new ValidationException("Not a valid coachId.", "coachId");
        }

        private void ValidateLocation(Guid? locationId)
        {
            if (!locationId.HasValue)
                return;
            LocationGetByIdUseCase.BusinessId = BusinessId;
            var coach = LocationGetByIdUseCase.GetLocation(locationId.Value);
            if (coach == null)
                throw new ValidationException("Not a valid locationId.", "locationId");
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
    }
}
