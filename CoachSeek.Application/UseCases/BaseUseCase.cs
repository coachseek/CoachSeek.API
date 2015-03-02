using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseUseCase
    {
        protected IBusinessRepository BusinessRepository { get; set; }

        protected BaseUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }
    }
}
