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
    public class CoachAddUseCase : AddUseCase, ICoachAddUseCase
    {
        public Guid BusinessId { get; set; }

        
        public CoachAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddCoach(CoachAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var newCoach = new NewCoach(command);
                ValidateAdd(newCoach);
                var data = BusinessRepository.AddCoach(BusinessId, newCoach);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is DuplicateCoach)
                    return new DuplicateCoachErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateAdd(NewCoach newCoach)
        {
            var coaches = BusinessRepository.GetAllCoaches(BusinessId);
            var isExistingCoach = coaches.Any(x => x.FirstName.ToLower() == newCoach.FirstName.ToLower()
                                                && x.LastName.ToLower() == newCoach.LastName.ToLower());
            if (isExistingCoach)
                throw new DuplicateCoach();
        }


        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddCoach((CoachAddCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is DuplicateCoach)
                return new DuplicateCoachErrorResponse();

            return null;
        }
    }
}