using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICourseSessionBookingAddUseCase : IBookingAddUseCase
    {
        RepeatedSessionData Course { set; }
    }
}
