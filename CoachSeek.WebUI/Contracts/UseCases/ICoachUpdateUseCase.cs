﻿using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.Contracts.UseCases
{
    public interface ICoachUpdateUseCase
    {
        CoachUpdateResponse UpdateCoach(CoachUpdateRequest request);
    }
}
