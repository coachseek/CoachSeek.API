using System.Threading.Tasks;
using Coachseek.Infrastructure.Queueing.Contracts.Import;

namespace Coachseek.Integration.DataImport
{
    public interface IDataImportMessageProcessor
    {
        Task ProcessMessageAsync(DataImportMessage message);
    }
}
