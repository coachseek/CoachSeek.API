using System.Collections.Generic;

namespace Coachseek.Infrastructure.Queueing.Contracts.Import
{
    public interface IDataImportQueueClient
    {
        void Push(DataImportMessage message);
        IList<DataImportMessage> Peek();
        void Pop(DataImportMessage message);
    }
}
