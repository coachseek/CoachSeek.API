using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases.Payments;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Payments.Interfaces;
using Coachseek.Integration.Contracts.Payments.Models;

namespace CoachSeek.Application.UseCases.Payments
{
    public class ProcessOnlinePaymentsUseCase : IProcessOnlinePaymentsUseCase
    {
        public IOnlinePaymentProcessingQueueClient PaymentProcessingQueueClient { get; private set; }
        public IPaymentMessageProcessor PaymentMessageProcessor { get; private set; }
        public IDataAccessFactory DataAccessFactory { get; private set; }
        public IPaymentProcessorConfiguration PaymentProcessorConfiguration { get; private set; }


        public ProcessOnlinePaymentsUseCase(IOnlinePaymentProcessingQueueClient paymentProcessingQueueClient,
                                            IPaymentMessageProcessor paymentMessageProcessor,
                                            IDataAccessFactory dataAccessFactory,
                                            IPaymentProcessorConfiguration paymentProcessorConfiguration)
        {
            PaymentProcessingQueueClient = paymentProcessingQueueClient;
            PaymentMessageProcessor = paymentMessageProcessor;
            DataAccessFactory = dataAccessFactory;
            PaymentProcessorConfiguration = paymentProcessorConfiguration;
        }


        public async Task ProcessAsync()
        {
            var messages = await PaymentProcessingQueueClient.PeekAsync();

            foreach (var message in messages)
            {
                try
                {
                    await PaymentMessageProcessor.ProcessMessageAsync(message);
                    await PaymentProcessingQueueClient.PopAsync(message);
                }
                catch (Exception ex)
                {
                    GetDataAccess().LogRepository.LogError(ex.Message);
                }
            }
        }

        private DataRepositories GetDataAccess()
        {
            var isTesting = PaymentProcessorConfiguration.Environment != Common.Environment.Production;
            return DataAccessFactory.CreateDataAccess(isTesting);
        }
    }
}
