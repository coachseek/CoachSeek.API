using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using Coachseek.Infrastructure.Queueing.Contracts.Import;

namespace CoachSeek.Application.UseCases.DataImport
{
    public class CustomerReceiveDataImportMessageUseCase : BaseUseCase, ICustomerReceiveDataImportMessageUseCase
    {
        private const string ENTITY_TYPE = "Customer";
        private const string DATA_FORMAT = "CSV";

        private IDataImportQueueClient DataImportQueueClient { get; set; }

        public CustomerReceiveDataImportMessageUseCase(IDataImportQueueClient dataImportQueueClient)
        {
            DataImportQueueClient = dataImportQueueClient;
        }

        public async Task ReceiveAsync(Guid businessId, string importData)
        {
            var message = DataImportMessage.Create(businessId, importData, ENTITY_TYPE, DATA_FORMAT);
            await DataImportQueueClient.PushAsync(message);
        }
    }
}
