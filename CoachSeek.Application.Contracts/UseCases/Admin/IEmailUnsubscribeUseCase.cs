using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases.Admin
{
    public interface IEmailUnsubscribeUseCase : IAdminApplicationContextSetter
    {
        IResponse Unsubscribe(string emailAddress);
    }
}
