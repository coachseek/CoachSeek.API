using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases.Factories
{
    public interface IBookingAddUseCaseFactory : IBusinessRepositorySetter
    {
        IBookingAddUseCase CreateUseCase(BookingAddCommand command);
    }
}
