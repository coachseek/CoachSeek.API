using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationAddUseCase : ILocationAddUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public LocationAddUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        public LocationAddResponse AddLocation(LocationAddCommand command)
        {
            if (command == null)
                return new NoLocationAddDataResponse();

            try
            {
                var business = GetBusiness(command);
                business.AddLocation(command, BusinessRepository);
                return new LocationAddResponse(business);
            }
            catch (Exception ex)
            {
                return HandleAddLocationException(ex);
            }
        }

        private Business GetBusiness(LocationAddCommand command)
        {
            var business = BusinessRepository.Get(command.BusinessId);
            if (business == null)
                throw new InvalidBusiness();
            return business;
        }

        private LocationAddResponse HandleAddLocationException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();
            if (ex is DuplicateLocation)
                return HandleDuplicateLocation();

            return null;
        }

        private LocationAddResponse HandleInvalidBusiness()
        {
            return new InvalidBusinessLocationAddResponse();
        }

        private LocationAddResponse HandleDuplicateLocation()
        {
            return new DuplicateLocationAddResponse();
        }
    }
}