using Coachseek.Infrastructure.Queueing.Contracts.Import;

namespace Coachseek.Integration.DataImport
{
    public interface IDataImportMessageProcessor
    {
        void ProcessMessage(DataImportMessage message);
    }
}
