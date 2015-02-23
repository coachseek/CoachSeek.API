using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationAddUseCase : AddUseCase, ILocationAddUseCase
    {
        public Guid BusinessId { get; set; }

        
        public LocationAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddLocation(LocationAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var newLocation = new NewLocation(command);
                ValidateAdd(newLocation);
                var data = BusinessRepository.AddLocation(BusinessId, newLocation);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is DuplicateLocation)
                    return new DuplicateLocationErrorResponse();
                throw;
            }
        }

        private void ValidateAdd(NewLocation newLocation)
        {
            var locations = BusinessRepository.GetAllLocations(BusinessId);
            var isExistingLocation = locations.Any(x => x.Name.ToLower() == newLocation.Name.ToLower());
            if (isExistingLocation)
                throw new DuplicateLocation();
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