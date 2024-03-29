﻿using CoachSeek.Application.UseCases.Payments;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Infrastructure.Queueing.Tests.Unit.Fakes;
using Coachseek.Integration.Tests.Unit.Payments.Fakes;
using Coachseek.Logging.Tests.Unit.Fakes;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class ProcessPaymentsUseCaseTests
    {
        [Test]
        public void GivenNoMessage_WhenTryProcess_ThenDoNothing()
        {
            var useCase = GivenNoMessage();
            WhenTryProcess(useCase);
            ThenDoNothing(useCase);
        }

        [Test]
        public void GivenSingleMessage_WhenTryProcess_ThenProcessAndPopMessage()
        {
            var useCase = GivenSingleMessage();
            WhenTryProcess(useCase);
            ThenProcessAndPopMessage(useCase);
        }

        [Test]
        public void GivenTwoMessages_WhenTryProcess_ThenProcessAndPopBoth()
        {
            var useCase = GivenTwoMessages();
            WhenTryProcess(useCase);
            ThenProcessAndPopBoth(useCase);
        }

        [Test]
        public void GivenTwoMessagesAndOneErrors_WhenTryProcess_ThenStillProcessAndPopTheOtherMessage()
        {
            var useCase = GivenTwoMessagesAndOneErrors();
            WhenTryProcess(useCase);
            ThenStillProcessAndPopTheOtherMessage(useCase);
        }


        private ProcessOnlinePaymentsUseCase GivenNoMessage()
        {
            var queue = new StubPaymentProcessingQueueClient();
            var processor = new StubPaymentMessageProcessor();

            return new ProcessOnlinePaymentsUseCase(queue, processor, null, null);
        }

        private ProcessOnlinePaymentsUseCase GivenSingleMessage()
        {
            var queue = new StubPaymentProcessingQueueClient(new PaymentProcessingMessage("1", "PayPal", "contents"));
            var processor = new StubPaymentMessageProcessor();

            return new ProcessOnlinePaymentsUseCase(queue, processor, null, null);
        }

        private ProcessOnlinePaymentsUseCase GivenTwoMessages()
        {
            var queue = new StubPaymentProcessingQueueClient(new[] { new PaymentProcessingMessage("1", "PayPal", "contents"), 
                                                                     new PaymentProcessingMessage("2", "PayPal", "contents too") });
            var processor = new StubPaymentMessageProcessor();

            return new ProcessOnlinePaymentsUseCase(queue, processor, null, null);
        }

        private ProcessOnlinePaymentsUseCase GivenTwoMessagesAndOneErrors()
        {
            var queue = new StubPaymentProcessingQueueClient(new[] { new PaymentProcessingMessage("1", "Error", "contents"), 
                                                                     new PaymentProcessingMessage("2", "PayPal", "contents too") });
            var processor = new StubPaymentMessageProcessor();
            var config = new StubPaymentProcessorConfiguration();
            var dataAccessFactory = new StubDataAccessFactory { LogRepository = new StubLogRepository() };

            return new ProcessOnlinePaymentsUseCase(queue, processor, dataAccessFactory, config);
        }


        private void WhenTryProcess(ProcessOnlinePaymentsUseCase useCase)
        {
            useCase.ProcessAsync().Wait();
        }


        private void ThenDoNothing(ProcessOnlinePaymentsUseCase useCase)
        {
            var queue = (StubPaymentProcessingQueueClient)useCase.PaymentProcessingQueueClient;
            var processor = (StubPaymentMessageProcessor)useCase.PaymentMessageProcessor;

            Assert.That(processor.WasProcessMessageCalled, Is.False);
            Assert.That(queue.WasPopCalled, Is.False);
        }

        private void ThenProcessAndPopMessage(ProcessOnlinePaymentsUseCase useCase)
        {
            var queue = (StubPaymentProcessingQueueClient)useCase.PaymentProcessingQueueClient;
            var processor = (StubPaymentMessageProcessor)useCase.PaymentMessageProcessor;

            Assert.That(processor.WasProcessMessageCalled, Is.True);
            Assert.That(processor.ProcessMessageCallCount, Is.EqualTo(1));

            Assert.That(queue.WasPopCalled, Is.True);
            Assert.That(queue.PopCallCount, Is.EqualTo(1));
        }

        private void ThenProcessAndPopBoth(ProcessOnlinePaymentsUseCase useCase)
        {
            var queue = (StubPaymentProcessingQueueClient)useCase.PaymentProcessingQueueClient;
            var processor = (StubPaymentMessageProcessor)useCase.PaymentMessageProcessor;

            Assert.That(processor.WasProcessMessageCalled, Is.True);
            Assert.That(processor.ProcessMessageCallCount, Is.EqualTo(2));

            Assert.That(queue.WasPopCalled, Is.True);
            Assert.That(queue.PopCallCount, Is.EqualTo(2));
        }

        private void ThenStillProcessAndPopTheOtherMessage(ProcessOnlinePaymentsUseCase useCase)
        {
            var queue = (StubPaymentProcessingQueueClient)useCase.PaymentProcessingQueueClient;
            var processor = (StubPaymentMessageProcessor)useCase.PaymentMessageProcessor;

            Assert.That(processor.WasProcessMessageCalled, Is.True);
            Assert.That(processor.ProcessMessageCallCount, Is.EqualTo(2));

            Assert.That(queue.WasPopCalled, Is.True);
            Assert.That(queue.PopCallCount, Is.EqualTo(1));

            var message = processor.PassedInMessage;
            Assert.That(message.Id, Is.EqualTo("2"));
            Assert.That(message.PaymentProvider, Is.EqualTo("PayPal"));
            Assert.That(message.Contents, Is.EqualTo("contents too"));
        }
    }
}
