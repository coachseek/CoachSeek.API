namespace Coachseek.Infrastructure.Queueing.Contracts
{
    public interface IBouncedEmailQueueClient : IQueueClient<BouncedEmailMessage>
    {
        Queue GetBouncedEmailQueue();
    }
}
