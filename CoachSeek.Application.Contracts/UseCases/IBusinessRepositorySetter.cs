using System;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessRepositorySetter
    {
        void Initialise(IBusinessRepository businessRepository, Guid? businessId = null);
    }
}
