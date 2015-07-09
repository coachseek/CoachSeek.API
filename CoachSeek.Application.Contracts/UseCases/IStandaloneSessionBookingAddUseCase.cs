using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IStandaloneSessionBookingAddUseCase : IBookingAddUseCase
    {
        SingleSessionData Session { set; }
    }
}
