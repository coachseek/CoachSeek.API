using System;
using System.Collections.Generic;
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

                return new Response<IEnumerable<SessionData>>(new List<SessionData>());
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

        private void Validate(string startDate, string endDate)
        {
            ValidateStartDate(startDate);
            ValidateEndDate(endDate);
        }

        private static void ValidateStartDate(string startDate)
        {
            if (string.IsNullOrEmpty(startDate))
                throw new InvalidStartDate();
            try
            {
                var start = new Date(startDate);
            }
            catch (InvalidDate ex)
            {
                throw new InvalidStartDate();
            }
        }

        private static void ValidateEndDate(string endDate)
        {
            if (string.IsNullOrEmpty(endDate))
                throw new InvalidEndDate();
            try
            {
                var end = new Date(endDate);
            }
            catch (InvalidDate ex)
            {
                throw new InvalidEndDate();
            }
        }
    }
}
