﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class CoachAddUseCase : BaseUseCase, ICoachAddUseCase
    {
        public Response AddCoach(CoachAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var newCoach = new Coach(command);
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

        private void ValidateAdd(Coach newCoach)
        {
            var coaches = BusinessRepository.GetAllCoaches(BusinessId);
            var isExistingCoach = coaches.Any(x => x.FirstName.ToLower() == newCoach.FirstName.ToLower()
                                                && x.LastName.ToLower() == newCoach.LastName.ToLower());
            if (isExistingCoach)
                throw new DuplicateCoach();
        }
    }
}