namespace Coachseek.Infrastructure.Queueing.Contracts
{
    public interface IMessage
    {
        string ReceiptId { get; }
    }
}
