﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessAddUseCase
    {
        Response AddBusiness(BusinessAddCommand command);
    }
}
