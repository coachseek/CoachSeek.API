using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationUpdateUseCase : UpdateUseCase, ILocationUpdateUseCase
    {
        public LocationUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response UpdateLocation(LocationUpdateCommand command)
        {
            return Update(command);
        }

        protected override object UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateLocation((LocationUpdateCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is InvalidLocation)
                return new InvalidLocationErrorResponse();
            if (ex is DuplicateLocation)
                return new DuplicateLocationErrorResponse();

            return null;
        }
    }
}