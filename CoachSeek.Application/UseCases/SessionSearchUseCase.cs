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
    public class SessionSearchUseCase : BaseUseCase<SessionData>, ISessionSearchUseCase
    {
        public Guid BusinessId { get; set; }

        private ICoachGetUseCase CoachGetUseCase { get; set; }
        private ILocationGetByIdUseCase LocationGetUseCase { get; set; }

        public SessionSearchUseCase(IBusinessRepository businessRepository,
                                    ICoachGetUseCase coachGetUseCase,
                                    ILocationGetByIdUseCase locationGetUseCase)
            : base(businessRepository)
        {
            CoachGetUseCase = coachGetUseCase;
            LocationGetUseCase = locationGetUseCase;
        }


        public IList<SessionData> SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null)
        {
            Validate(startDate, endDate, coachId, locationId);

            var business = GetBusiness(BusinessId);
            
            var query = business.Sessions.Where(x => new Date(x.Timing.StartDate).IsOnOrAfter(new Date(startDate)))
                                         .Where(x => new Date(x.Timing.StartDate).IsOnOrBefore(new Date(endDate)));

            if (coachId.HasValue)
                query = query.Where(x => x.Coach.Id == coachId);

            if (locationId.HasValue)
                query = query.Where(x => x.Location.Id == locationId);

            return query.OrderBy(x => x.Timing.StartDate).ToList();
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
            CoachGetUseCase.BusinessId = BusinessId;
            var coach = CoachGetUseCase.GetCoach(coachId.Value);
            if (coach == null)
                throw new ValidationException("Not a valid coachId.", "coachId");
        }

        private void ValidateLocation(Guid? locationId)
        {
            if (!locationId.HasValue)
                return;
            LocationGetUseCase.BusinessId = BusinessId;
            var coach = LocationGetUseCase.GetLocation(locationId.Value);
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
