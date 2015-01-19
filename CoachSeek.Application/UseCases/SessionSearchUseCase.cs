using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.Models.Responses;
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


        public SessionSearchUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<IEnumerable<SessionData>> SearchForSessions(string startDate, string endDate)
        {
            try
            {
                Validate(startDate, endDate);
                var searchStartDate = new Date(startDate);
                var searchEndDate = new Date(endDate);

                var business = GetBusiness(BusinessId);

                var sessions = business.Sessions.Where(x => new Date(x.Timing.StartDate).IsOnOrAfter(searchStartDate))
                                                .Where(x => new Date(x.Timing.StartDate).IsOnOrBefore(searchEndDate))
                                                .OrderBy(x => x.Timing.StartDate).ToList();

                return new Response<IEnumerable<SessionData>>(sessions);
            }
            catch (Exception ex)
            {
                if (ex is InvalidStartDate)
                    return new ErrorResponse<IEnumerable<SessionData>>(new ErrorData("Invalid start date."));
                if (ex is InvalidEndDate)
                    return new ErrorResponse<IEnumerable<SessionData>>(new ErrorData("Invalid end date."));

                return null;
            }
        }

        private void Validate(string searchStartDate, string searchEndDate)
        {
            var startDate = ValidateStartDate(searchStartDate);
            var endDate = ValidateEndDate(searchEndDate);

            //if (startDate.IsAfter() > endDate)


        }

        private static Date ValidateStartDate(string startDate)
        {
            if (string.IsNullOrEmpty(startDate))
                throw new InvalidStartDate();

            try
            {
                return new Date(startDate);
            }
            catch (InvalidDate)
            {
                throw new InvalidStartDate();
            }
        }

        private static Date ValidateEndDate(string endDate)
        {
            if (string.IsNullOrEmpty(endDate))
                throw new InvalidEndDate();
            
            try
            {
                return new Date(endDate);
            }
            catch (InvalidDate)
            {
                throw new InvalidEndDate();
            }
        }
    }
}
