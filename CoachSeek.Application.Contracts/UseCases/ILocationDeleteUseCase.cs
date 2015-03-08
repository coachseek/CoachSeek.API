using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationDeleteUseCase : IBusinessRepositorySetter
    {
        Response DeleteLocation(Guid id);
    }
}
