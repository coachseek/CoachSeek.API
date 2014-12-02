﻿using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionUpdateUseCase
    {
        Response<SessionData> UpdateSession(SessionUpdateCommand command);
    }
}