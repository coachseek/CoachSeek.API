﻿using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationGetByIdUseCase : IApplicationContextSetter
    {
        LocationData GetLocation(Guid id);
    }
}
