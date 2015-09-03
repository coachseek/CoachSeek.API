using System;
using CoachSeek.Application.Contracts.UseCases.Import;
using Coachseek.Infrastructure.Queueing.Contracts.Import;

namespace CoachSeek.Application.UseCases.Import
{
    public class CustomerReceiveDataImportMessageUseCase : ICustomerReceiveDataImportMessageUseCase
    {
        private const string ENTITY_TYPE = "Customer";
        private const string DATA_FORMAT = "CSV";

        private IDataImportQueueClient DataImportQueueClient { get; set; }

        public CustomerReceiveDataImportMessageUseCase(IDataImportQueueClient dataImportQueueClient)
        {
            DataImportQueueClient = dataImportQueueClient;
        }

        public void Receive(Guid businessId, string importData)
        {
            var message = DataImportMessage.Create(businessId, importData, ENTITY_TYPE, DATA_FORMAT);
            DataImportQueueClient.Push(message);
        }
    }
}
