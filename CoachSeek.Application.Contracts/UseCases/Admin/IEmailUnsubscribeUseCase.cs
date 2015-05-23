using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases.Admin
{
    public interface IEmailUnsubscribeUseCase : IApplicationContextSetter
    {
        Response Unsubscribe(string emailAddress);
    }
}
