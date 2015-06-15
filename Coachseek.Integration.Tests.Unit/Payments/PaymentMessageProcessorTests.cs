using CoachSeek.DataAccess.Main.Memory.Configuration;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Payments.PaymentsProcessor;
using Coachseek.Integration.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Coachseek.Integration.Tests.Unit.Payments
{
    [TestFixture]
    public class PaymentMessageProcessorTests
    {
        [SetUp]
        public void Setup()
        {
            DbAutoMapperConfigurator.Configure();
        }


        [Test, Ignore("not ready yet")]
        public void GivenIsExistingVerifiedPayment_WhenTryProcessMessage_ThenLookupMessageAndReturn()
        {
            var processor = GivenIsExistingVerifiedPayment();
            WhenTryProcessMessage(processor);
            ThenLookupMessageAndReturn(processor);
        }


        private PaymentMessageProcessor GivenIsExistingVerifiedPayment()
        {
            var config = new StubPaymentProcessorConfiguration(true);
            var repository = new InMemoryTransactionRepository(new[] { new DbTransaction { Id = "1", Type = "Payment", IsVerified = true } });

            return new PaymentMessageProcessor(config, repository);
        }

        private void WhenTryProcessMessage(PaymentMessageProcessor processor)
        {
            var message = new PaymentProcessingMessage("1", "Test", "world!");
            processor.ProcessMessage(message);
        }

        private void ThenLookupMessageAndReturn(PaymentMessageProcessor processor)
        {
            var configuration = (StubPaymentProcessorConfiguration)processor.PaymentProcessorConfiguration;
            var repository = (InMemoryTransactionRepository)processor.TransactionRepository;

            Assert.That(repository.WasAddPaymentCalled, Is.False);
        }
    }
}
