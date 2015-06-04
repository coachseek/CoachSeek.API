namespace BouncedEmailProcessor
{
    public interface IBouncedEmailQueueClient : IQueueClient<BouncedEmailMessage>
    {
        Queue GetBouncedEmailQueue();
    }
}
