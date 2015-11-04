namespace CoachSeek.Application.Contracts.Services.Emailing
{
    public interface IDataImportProcessEmailer : IApplicationContextSetter
    {
        void SendProcessingSuccessfulEmail();
        void SendProcessingWithErrorsEmail();
    }
}
