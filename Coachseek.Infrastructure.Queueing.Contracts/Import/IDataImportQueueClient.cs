using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coachseek.Infrastructure.Queueing.Contracts.Import
{
    public interface IDataImportQueueClient
    {
        Task PushAsync(DataImportMessage message);
        Task<IList<DataImportMessage>> PeekAsync();
        Task PopAsync(DataImportMessage message);
    }
}
