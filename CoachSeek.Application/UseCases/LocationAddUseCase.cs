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
    public class LocationAddUseCase : AddUseCase<LocationData>, ILocationAddUseCase
    {
        public LocationAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<LocationData> AddLocation(LocationAddCommand command)
        {
            return Add(command);
        }

        protected override LocationData AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddLocation((LocationAddCommand)command, BusinessRepository);
        }

        protected override Response<LocationData> HandleSpecificException(Exception ex)
        {
            if (ex is DuplicateLocation)
                return new DuplicateLocationAddResponse();

            return null;
        }
    }
}