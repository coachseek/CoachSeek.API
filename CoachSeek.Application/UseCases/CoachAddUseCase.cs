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
    public class CoachAddUseCase : ICoachAddUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public CoachAddUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        public Response<CoachData> AddCoach(CoachAddCommand command)
        {
            if (command == null)
                return new NoCoachAddDataResponse();

            try
            {
                var business = GetBusiness(command);
                var coach = business.AddCoach(command, BusinessRepository);
                return new Response<CoachData>(coach);
            }
            catch (Exception ex)
            {
                return HandleAddCoachException(ex);
            }
        }

        private Business GetBusiness(CoachAddCommand command)
        {
            var business = BusinessRepository.Get(command.BusinessId);
            if (business == null)
                throw new InvalidBusiness();
            return business;
        }

        private Response<CoachData> HandleAddCoachException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();
            if (ex is DuplicateCoach)
                return HandleDuplicateCoach();
            if (ex is ValidationException)
                return new Response<CoachData>((ValidationException)ex);

            return null;
        }

        private Response<CoachData> HandleInvalidBusiness()
        {
            return new InvalidBusinessCoachAddResponse();
        }

        private Response<CoachData> HandleDuplicateCoach()
        {
            return new DuplicateCoachAddResponse();
        }
    }
}