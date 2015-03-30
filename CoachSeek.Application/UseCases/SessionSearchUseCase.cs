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


        public IList<SingleSessionData> SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null)
        {
            Validate(startDate, endDate, coachId, locationId, serviceId);

            var sessions = BusinessRepository.GetAllSessions(BusinessId);

            var sessionQuery = sessions.Where(x => new Date(x.Timing.StartDate).IsOnOrAfter(new Date(startDate)))
                                       .Where(x => new Date(x.Timing.StartDate).IsOnOrBefore(new Date(endDate)));

            if (coachId.HasValue)
                sessionQuery = sessionQuery.Where(x => x.Coach.Id == coachId);

            if (locationId.HasValue)
                sessionQuery = sessionQuery.Where(x => x.Location.Id == locationId);

            if (serviceId.HasValue)
                sessionQuery = sessionQuery.Where(x => x.Service.Id == serviceId);

            var searchResults = sessionQuery.OrderBy(x => x.Timing.StartDate).ThenBy(x => CreateOrderableStartTime(x.Timing.StartTime)).ToList();

            foreach (var session in searchResults)
                session.Booking.Bookings = BusinessRepository.GetCustomerBookingsBySessionId(BusinessId, session.Id);

            return searchResults;
        }

        private static string CreateOrderableStartTime(string startTime)
        {
            return startTime.Length == 4 ? string.Format("0{0}", startTime) : startTime;
        }

        private void Validate(string searchStartDate, string searchEndDate, Guid? coachId, Guid? locationId, Guid? serviceId)
        {
            ValidateDates(searchStartDate, searchEndDate);
            ValidateCoach(coachId);
            ValidateLocation(locationId);
            ValidateService(serviceId);
        }

        private void ValidateCoach(Guid? coachId)
        {
            if (!coachId.HasValue)
                return;
            CoachGetByIdUseCase.Initialise(BusinessRepository, BusinessId);
            var coach = CoachGetByIdUseCase.GetCoach(coachId.Value);
            if (coach == null)
                throw new ValidationException("Not a valid coachId.", "coachId");
        }

        private void ValidateLocation(Guid? locationId)
        {
            if (!locationId.HasValue)
                return;
            LocationGetByIdUseCase.Initialise(BusinessRepository, BusinessId);
            var coach = LocationGetByIdUseCase.GetLocation(locationId.Value);
            if (coach == null)
                throw new ValidationException("Not a valid locationId.", "locationId");
        }

        private void ValidateService(Guid? serviceId)
        {
            if (!serviceId.HasValue)
                return;
            ServiceGetByIdUseCase.Initialise(BusinessRepository, BusinessId);
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
    }
}
