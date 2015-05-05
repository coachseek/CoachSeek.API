using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessGetUseCase : IBusinessRepositorySetter
    {
        BusinessData GetBusiness();
    }
}
