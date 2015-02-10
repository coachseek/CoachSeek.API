using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationAddUseCase : AddUseCase, ILocationAddUseCase
    {
        public LocationAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddLocation(LocationAddCommand command)
        {
            return Add(command);
        }

        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddLocation((LocationAddCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is DuplicateLocation)
                return new DuplicateLocationErrorResponse();

            return null;
        }
    }
}