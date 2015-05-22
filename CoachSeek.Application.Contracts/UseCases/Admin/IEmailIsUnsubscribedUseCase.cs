namespace CoachSeek.Application.Contracts.UseCases.Admin
{
    public interface IEmailIsUnsubscribedUseCase : IApplicationContextSetter
    {
        bool IsUnsubscribed(string emailAddress);
    }
}
