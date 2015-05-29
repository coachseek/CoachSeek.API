using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class BusinessGetUseCase : BaseUseCase, IBusinessGetUseCase
    {
        public BusinessData GetBusiness()
        {
            return BusinessRepository.GetBusiness(Business.Id);
        }
    }
}
