using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using CoachSeek.Domain.Commands;
using Coachseek.Infrastructure.Queueing.Contracts.Import;
using Coachseek.Integration.DataImport;

namespace CoachSeek.Application.UseCases.DataImport
{
    public class ProcessDataImportUseCase : IProcessDataImportUseCase
    {
        public IDataImportQueueClient DataImportQueueClient { get; private set; }
        private IDataImportMessageProcessor DataImportMessageProcessor { get; set; }


        public ProcessDataImportUseCase(IDataImportQueueClient dataImportQueueClient,
                                        IDataImportMessageProcessor dataImportMessageProcessor)
        {
            DataImportQueueClient = dataImportQueueClient;
            DataImportMessageProcessor = dataImportMessageProcessor;
        }

        public async Task ProcessAsync()
        {
            // Read import data from queue
            var messages = await DataImportQueueClient.PeekAsync();

            foreach (var message in messages)
            {
                try
                {
                    // Go to blob storage and get import data ...


                    await DataImportMessageProcessor.ProcessMessageAsync(message);
                    //await DataImportQueueClient.PopAsync(message);
                }
                catch (Exception)
                {
                    // Log error
                }
            }
        }
    }
}
