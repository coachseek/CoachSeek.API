namespace CoachSeek.Application.Contracts.UseCases.Admin
{
    public interface IEmailUnsubscribeUseCase : IApplicationContextSetter
    {
        void Unsubscribe(string emailAddress);
    }
}
