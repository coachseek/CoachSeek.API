using System;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachGetByIdUseCase : IApplicationContextSetter
    {
        Task<CoachData> GetCoachAsync(Guid id);
    }
}
