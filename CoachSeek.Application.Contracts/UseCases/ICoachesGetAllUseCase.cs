using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachesGetAllUseCase
    {
        Guid BusinessId { get; set; }

        IList<CoachData> GetCoaches();
    }
}
