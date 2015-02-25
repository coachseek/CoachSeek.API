﻿using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class CoachGetByIdUseCase : BaseUseCase, ICoachGetByIdUseCase
    {
        public Guid BusinessId { get; set; }

        public CoachGetByIdUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public CoachData GetCoach(Guid id)
        {
            return BusinessRepository.GetCoach(BusinessId, id);
        }
    }
}
