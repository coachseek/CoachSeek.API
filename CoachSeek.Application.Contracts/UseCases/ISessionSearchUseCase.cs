﻿using System.Collections.Generic;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionSearchUseCase
    {
        Response<IEnumerable<SessionData>> SearchForSessions(string startDate, string endDate);
    }
}
