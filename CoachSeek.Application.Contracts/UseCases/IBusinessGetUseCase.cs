using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessGetUseCase : IApplicationContextSetter
    {
        BusinessData GetBusiness();
    }
}
