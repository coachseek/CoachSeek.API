﻿using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationDeleteUseCase : IApplicationContextSetter
    {
        Response DeleteLocation(Guid id);
    }
}
