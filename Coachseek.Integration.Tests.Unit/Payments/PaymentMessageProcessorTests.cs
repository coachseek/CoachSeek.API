using System;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Payments.PaymentsProcessor;
using Coachseek.Integration.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Coachseek.Integration.Tests.Unit.Payments
{
    [TestFixture]
    public class PaymentMessageProcessorTests
    {
        private const string PROVIDER_TEST = "Test";
        private const string BUSINESS_ID = "6A857C90-2446-4197-B251-F1189A7BB39F";
        private const string EXISTING_MSG_ID = "1";
        private const string NEW_MSG_ID = "2";
        private const string TEST_MESSAGE =
            "status={0}&isTesting=true&date=2020-01-01&payerFirstName=Mike&payerLastName=Smith&payerEmail=mike@smith.com&businessId={1}&businessName=TestBusiness&businessEmail=test@business.com&itemId=3&itemName=Session1&currency=NZD&grossAmount=15";


        private PaymentMessageProcessor Processor { get; set; }

        private StubPaymentProcessorConfiguration Configuration
        {
            get { return (StubPaymentProcessorConfiguration)Processor.PaymentProcessorConfiguration; }
        }

        private StubDataAccessFactory DataAccessFactory
        {
            get { return (StubDataAccessFactory)Processor.DataAccessFactory; }
        }

        private StubPaymentProviderApiFactory PaymentProviderApiFactory
        {
            get { return (StubPaymentProviderApiFactory)Processor.PaymentProviderApiFactory; }
        }

        private InMemoryTransactionRepository TransactionRepository
        {
            get { return (InMemoryTransactionRepository)DataAccessFactory.TransactionRepository; }
        }

        private InMemoryBusinessRepository BusinessRepository
        {
            get { return (InMemoryBusinessRepository)DataAccessFactory.BusinessRepository; }
        }


        [SetUp]
        public void Setup()
        {
            DbAutoMapperConfigurator.Configure();

            SetupProcessor();
        }

        private void SetupProcessor()
        {
            var transaction = CreateDbTransaction(EXISTING_MSG_ID, "Completed", true, BUSINESS_ID);
            var dataAccessFactory = new StubDataAccessFactory
            {
                BusinessRepository = new InMemoryBusinessRepository(),
                TransactionRepository = new InMemoryTransactionRepository(new[] { transaction })
            };
            var config = new StubPaymentProcessorConfiguration(true);
            var apiFactory = new StubPaymentProviderApiFactory
            {
                PaymentProviderApi = new StubPaymentProviderApi(true)
            };

            Processor = new PaymentMessageProcessor(config, dataAccessFactory, apiFactory);
        }

        private DbTransaction CreateDbTransaction(string id, string status, bool? isVerified, string businessId)
        {
            return new DbTransaction
            {
                Id = id,
                Type = "Payment",
                PaymentProvider = PROVIDER_TEST,
                TransactionDate = new DateTime(2020, 1, 1),
                IsVerified = isVerified,
                Status = status,
                PayerFirstName = "Mike",
                PayerLastName = "Smith",
                PayerEmail = "mike@smith.com",
                MerchantId = businessId,
                MerchantName = "TestBusiness",
                MerchantEmail = "test@business.com",
                ItemId = "3",
                ItemName = "Session1",
                ItemCurrency = "NZD",
                ItemAmount = 15,
            };
        }


        [Test]
        public void GivenSpoofPaymentMessage_WhenTryProcessMessage_ThenFailsVerificationAndReturn()
        {
            var message = GivenSpoofPaymentMessage();
            WhenTryProcessMessage(message);
            ThenFailsVerificationAndReturn();
        }

        [Test]
        public void GivenIsPendingPaymentMessage_WhenTryProcessMessage_ThenPassVerificationAndReturn()
        {
            var message = GivenIsPendingPaymentMessage();
            WhenTryProcessMessage(message);
            ThenPassVerificationAndReturn();
        }

        [Test]
        public void GivenIsDeniedPaymentButCanPayLater_WhenTryProcessMessage_ThenSavePaymentAndSetBookingToAwaitingPayment()
        {
            var message = GivenIsDeniedPaymentButCanPayLater();
            WhenTryProcessMessage(message);
            ThenSavePaymentAndSetBookingToAwaitingPayment();
        }

        // This scenario is not as important.
        //public void GivenIsDeniedPaymentAndMustPayNow_WhenTryProcessMessage_ThenSavePaymentAndDeleteBooking() ?

        [Test]
        public void GivenIsExistingCompletedPayment_WhenTryProcessMessage_ThenLookupMessageAndReturn()
        {
            var message = GivenIsExistingCompletedPayment();
            WhenTryProcessMessage(message);
            ThenLookupMessageAndReturn();
        }


        private PaymentProcessingMessage GivenSpoofPaymentMessage()
        {
            PaymentProviderApiFactory.PaymentProviderApi = new StubPaymentProviderApi(false);
            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID));
        }

        private PaymentProcessingMessage GivenIsPendingPaymentMessage()
        {
            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Pending", BUSINESS_ID));
        }

        private PaymentProcessingMessage GivenIsDeniedPaymentButCanPayLater()
        {
            const bool forceOnlinePayment = false;
            var business = new Business(new Guid(BUSINESS_ID), "TestBusiness", "testbusiness", "NZD", true, forceOnlinePayment);
            BusinessRepository.AddBusiness(business);

            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Denied", BUSINESS_ID));
        }

        private PaymentProcessingMessage GivenIsExistingCompletedPayment()
        {
            return new PaymentProcessingMessage(EXISTING_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID));
        }


        private void WhenTryProcessMessage(PaymentProcessingMessage message)
        {
            Processor.ProcessMessage(message);
        }


        private void ThenFailsVerificationAndReturn()
        {
            AssertVerificationFailed();

            Assert.That(DataAccessFactory.WasCreateDataAccessCalled, Is.False);
        }

        private void ThenPassVerificationAndReturn()
        {
            AssertVerificationPassed();

            Assert.That(DataAccessFactory.WasCreateDataAccessCalled, Is.False);
        }

        private void ThenLookupMessageAndReturn()
        {
            AssertVerificationPassed();

            Assert.That(DataAccessFactory.WasCreateDataAccessCalled, Is.True);

            // Lookup and saving payment
            Assert.That(TransactionRepository.WasGetPaymentCalled, Is.True);
            Assert.That(TransactionRepository.WasAddPaymentCalled, Is.False);
        }

        private void ThenSavePaymentAndSetBookingToAwaitingPayment()
        {
            AssertVerificationPassed();

            Assert.That(DataAccessFactory.WasCreateDataAccessCalled, Is.True);

            // Lookup and saving payment
            Assert.That(TransactionRepository.WasGetPaymentCalled, Is.True);
            Assert.That(TransactionRepository.WasAddPaymentCalled, Is.True);
        }

        private void AssertVerificationFailed()
        {
            Assert.That(PaymentProviderApiFactory.WasGetPaymentProviderApiCalled, Is.True);
            Assert.That(PaymentProviderApiFactory.PaymentProviderApi.WasVerifyPaymentCalled, Is.True);
            Assert.That(PaymentProviderApiFactory.PaymentProviderApi.VerifyPaymentResponse, Is.False);
        }

        private void AssertVerificationPassed()
        {
            Assert.That(PaymentProviderApiFactory.WasGetPaymentProviderApiCalled, Is.True);
            Assert.That(PaymentProviderApiFactory.PaymentProviderApi.WasVerifyPaymentCalled, Is.True);
            Assert.That(PaymentProviderApiFactory.PaymentProviderApi.VerifyPaymentResponse, Is.True);
        }
    }
}
