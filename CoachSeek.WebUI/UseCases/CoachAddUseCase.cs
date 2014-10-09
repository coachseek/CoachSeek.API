﻿using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Contracts.UseCases;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;
using System;

namespace CoachSeek.WebUI.UseCases
{
    public class CoachAddUseCase : ICoachAddUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public CoachAddUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }

        
        public CoachAddResponse AddCoach(CoachAddRequest request)
        {
            if (request == null)
                return new NoCoachAddDataResponse();

            try
            {
                var business = GetBusiness(request);
                business.AddCoach(request, BusinessRepository);
                return new CoachAddResponse(business);
            }
            catch (Exception ex)
            {
                return HandleAddCoachException(ex);
            }
        }

        private Business GetBusiness(CoachAddRequest request)
        {
            var business = BusinessRepository.Get(request.BusinessId);
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