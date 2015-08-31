namespace CoachSeek.Application.Contracts.UseCases.Admin
{
    public interface IEmailIsUnsubscribedUseCase : IAdminApplicationContextSetter
    {
        bool IsUnsubscribed(string emailAddress);
    }
}
