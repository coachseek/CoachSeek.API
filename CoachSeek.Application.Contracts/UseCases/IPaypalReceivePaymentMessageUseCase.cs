namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IPaypalReceivePaymentMessageUseCase
    {
        void Receive(string formData);
    }
}
