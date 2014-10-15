﻿using System;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class CoachAddUseCase : ICoachAddUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public CoachAddUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        public CoachAddResponse AddCoach(CoachAddCommand command)
        {
            if (command == null)
                return new NoCoachAddDataResponse();

            try
            {
                var business = GetBusiness(command);
                business.AddCoach(command, BusinessRepository);
                return new CoachAddResponse(business);
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

        private CoachAddResponse HandleAddCoachException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();
            if (ex is DuplicateCoach)
                return HandleDuplicateCoach();

            return null;
        }

        private CoachAddResponse HandleInvalidBusiness()
        {
            return new InvalidBusinessCoachAddResponse();
        }

        private CoachAddResponse HandleDuplicateCoach()
        {
            return new DuplicateCoachAddResponse();
        }
    }
}