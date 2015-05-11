using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases.Factories
{
    public interface IBookingAddUseCaseFactory : IApplicationContextSetter
    {
        IBookingAddUseCase CreateBookingUseCase(BookingAddCommand command);
        IBookingAddUseCase CreateOnlineBookingUseCase(BookingAddCommand command);
    }
}
