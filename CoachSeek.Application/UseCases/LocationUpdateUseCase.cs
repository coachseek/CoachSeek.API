using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationUpdateUseCase : UpdateUseCase<LocationData>, ILocationUpdateUseCase
    {
        public LocationUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<LocationData> UpdateLocation(LocationUpdateCommand command)
        {
            return Update(command);
        }

        protected override LocationData UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateLocation((LocationUpdateCommand)command, BusinessRepository);
        }

        protected override Response<LocationData> HandleSpecificException(Exception ex)
        {
            if (ex is InvalidLocation)
                return new InvalidLocationUpdateResponse();
            if (ex is DuplicateLocation)
                return new DuplicateLocationUpdateResponse();

            return null;
        }
    }
}