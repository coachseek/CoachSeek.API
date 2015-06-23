using System;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Payments.PaymentsProcessor;
using Coachseek.Integration.Tests.Unit.Fakes;
using Coachseek.Logging.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Coachseek.Integration.Tests.Unit.Payments
{
    [TestFixture]
    public class PaymentMessageProcessorTests
    {
        private const string PROVIDER_TEST = "Test";
        private const string BUSINESS_ID = "6A857C90-2446-4197-B251-F1189A7BB39F";
        private const string BOOKING_ID = "3386F697-1DB0-4F3C-A1EC-450533A43079";
        private const string EXISTING_MSG_ID = "1";
        private const string NEW_MSG_ID = "2";
        private const string TEST_MESSAGE =
            "status={0}&isTesting=true&date=2020-01-01&payerFirstName=Mike&payerLastName=Smith&payerEmail=mike@smith.com&businessId={1}&businessName=TestBusiness&businessEmail=test@business.com&itemId={2}&itemName=Session1&currency=NZD&grossAmount=15";


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

        private StubLogRepository LogRepository
        {
            get { return (StubLogRepository)DataAccessFactory.LogRepository; }
        }


        [SetUp]
        public void Setup()
        {
            DbAutoMapperConfigurator.Configure();

            SetupProcessor();
        }

        private void SetupProcessor()
        {
            InMemoryBusinessRepository.Clear();

            var transaction = CreateDbTransaction(EXISTING_MSG_ID, "Completed", true, BUSINESS_ID, BOOKING_ID);
            var dataAccessFactory = new StubDataAccessFactory
            {
                BusinessRepository = new InMemoryBusinessRepository(),
                TransactionRepository = new InMemoryTransactionRepository(new[] { transaction }),
                LogRepository = new StubLogRepository()
            };
            var config = new StubPaymentProcessorConfiguration(true);
            var apiFactory = new StubPaymentProviderApiFactory
            {
                PaymentProviderApi = new StubPaymentProviderApi(true)
            };

            Processor = new PaymentMessageProcessor(config, dataAccessFactory, apiFactory);
        }

        private DbTransaction CreateDbTransaction(string id, string status, bool? isVerified, string businessId, string bookingId)
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
                ItemId = bookingId,
                ItemName = "Session1",
                ItemCurrency = "NZD",
                ItemAmount = 15,
            };
        }


        [Test]
        public void GivenSpoofPaymentMessage_WhenTryProcessMessage_ThenFailsVerification()
        {
            var message = GivenSpoofPaymentMessage();
            WhenTryProcessMessage(message);
            ThenFailsVerification();
        }

        [Test]
        public void GivenIsPendingPaymentMessage_WhenTryProcessMessage_ThenStopProcessingWithPendingPaymentMessageError()
        {
            var message = GivenIsPendingPaymentMessage();
            WhenTryProcessMessage(message);
            ThenStopProcessingWithPendingPaymentMessageError();
        }

        [Test]
        public void GivenIsNonExistentBusiness_WhenTryProcessMessage_ThenStopProcessingWithInvalidBusinessError()
        {
            var message = GivenIsNonExistentBusiness();
            WhenTryProcessMessage(message);
            ThenStopProcessingWithInvalidBusinessError();
        }

        [Test]
        public void GivenIsOnlinePaymentDisabled_WhenTryProcessMessage_ThenStopProcessingWithIsOnlinePaymentDisabledError()
        {
            var message = GivenIsOnlinePaymentDisabled();
            WhenTryProcessMessage(message);
            ThenStopProcessingWithIsOnlinePaymentDisabledError();
        }

        [Test]
        public void GivenMerchantAccountIdentifierMismatch_WhenTryProcessMessage_ThenStopProcessingWithMerchantAccountIdentifierMismatchError()
        {
            var message = GivenMerchantAccountIdentifierMismatch();
            WhenTryProcessMessage(message);
            ThenStopProcessingWithMerchantAccountIdentifierMismatchError();
        }

        [Test]
        public void GivenIsNonExistentBooking_WhenTryProcessMessage_ThenStopProcessingWithInvalidBookingError()
        {
            var message = GivenIsNonExistentBooking();
            WhenTryProcessMessage(message);
            ThenStopProcessingWithInvalidBookingError();
        }

        [Test]
        public void GivenPaymentCurrencyMismatch_WhenTryProcessMessage_ThenStopProcessingWithPaymentCurrencyMismatchError()
        {
            var message = GivenPaymentCurrencyMismatch();
            WhenTryProcessMessage(message);
            ThenStopProcessingWithPaymentCurrencyMismatchError();
        }

        [Test, Ignore]
        public void GivenPaymentAmountMismatch_WhenTryProcessMessage_ThenStopProcessingWithPaymentAmountMismatchError()
        {
            var message = GivenPaymentAmountMismatch();
            WhenTryProcessMessage(message);
            ThenStopProcessingWithPaymentCurrencyMismatchError();
        }



        [Test, Ignore]
        public void GivenIsExistingCompletedPayment_WhenTryProcessMessage_ThenLookupMessageAndReturn()
        {
            var message = GivenIsExistingCompletedPayment();
            WhenTryProcessMessage(message);
            ThenLookupMessageAndReturn();
        }

        [Test, Ignore]
        public void GivenIsDeniedPaymentButCanPayLater_WhenTryProcessMessage_ThenSavePaymentAndSetBookingToPendingPayment()
        {
            var message = GivenIsDeniedPaymentButCanPayLater();
            WhenTryProcessMessage(message);
            ThenSavePaymentAndSetBookingToPendingPayment();
        }

        // This scenario is not as important.
        //public void GivenIsDeniedPaymentAndMustPayNow_WhenTryProcessMessage_ThenSavePaymentAndDeleteBooking() ?

        [Test, Ignore]
        public void GivenIsNewCompletedPayment_WhenTryProcessMessage_ThenSavePaymentAndSetBookingToPaid()
        {
            var message = GivenIsNewCompletedPayment();
            WhenTryProcessMessage(message);
            ThenSavePaymentAndSetBookingToPaid();
        }


        private PaymentProcessingMessage GivenSpoofPaymentMessage()
        {
            PaymentProviderApiFactory.PaymentProviderApi = new StubPaymentProviderApi(false);
            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID, BOOKING_ID));
        }

        private PaymentProcessingMessage GivenIsPendingPaymentMessage()
        {
            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Pending", BUSINESS_ID, BOOKING_ID));
        }

        private PaymentProcessingMessage GivenIsNonExistentBusiness()
        {
            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", Guid.NewGuid(), BOOKING_ID));
        }

        private PaymentProcessingMessage GivenIsOnlinePaymentDisabled()
        {
            const bool isOnlinePaymentEnabled = false;
            var business = new Business(new Guid(BUSINESS_ID), "TestBusiness", "testbusiness", "NZD", isOnlinePaymentEnabled);
            BusinessRepository.AddBusiness(business);

            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID, BOOKING_ID));
        }

        private PaymentProcessingMessage GivenMerchantAccountIdentifierMismatch()
        {
            var business = new Business(new Guid(BUSINESS_ID), "TestBusiness", "testbusiness", "NZD", true, false, "PayPal", "mickey@mouse.com");
            BusinessRepository.AddBusiness(business);

            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID, BOOKING_ID));
        }

        private PaymentProcessingMessage GivenIsNonExistentBooking()
        {
            var business = new Business(new Guid(BUSINESS_ID), "TestBusiness", "testbusiness", "NZD", true, false, "PayPal", "test@business.com");
            BusinessRepository.AddBusiness(business);

            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID, Guid.NewGuid()));
        }

        private PaymentProcessingMessage GivenPaymentCurrencyMismatch()
        {
            var business = new Business(new Guid(BUSINESS_ID), "TestBusiness", "testbusiness", "USD", true, false, "PayPal", "test@business.com");
            BusinessRepository.AddBusiness(business);
            var booking = new SingleSessionBooking(new Guid(BOOKING_ID),
                new SessionKeyData {Id = Guid.NewGuid()},
                new CustomerKeyData {Id = Guid.NewGuid()});
            BusinessRepository.AddSessionBooking(business.Id, booking);

            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID, BOOKING_ID));
        }

        private PaymentProcessingMessage GivenPaymentAmountMismatch()
        {
            var business = new Business(new Guid(BUSINESS_ID), "TestBusiness", "testbusiness", "NZD", true, false, "PayPal", "test@business.com");
            BusinessRepository.AddBusiness(business);
            var booking = new SingleSessionBooking(new Guid(BOOKING_ID),
                new SessionKeyData { Id = Guid.NewGuid() },
                new CustomerKeyData { Id = Guid.NewGuid() });
            BusinessRepository.AddSessionBooking(business.Id, booking);

            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID, BOOKING_ID));
        }


        private PaymentProcessingMessage GivenIsExistingCompletedPayment()
        {
            return new PaymentProcessingMessage(EXISTING_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID, BOOKING_ID));
        }

        private PaymentProcessingMessage GivenIsDeniedPaymentButCanPayLater()
        {
            const bool forceOnlinePayment = false;
            var business = new Business(new Guid(BUSINESS_ID), "TestBusiness", "testbusiness", "NZD", true, forceOnlinePayment);
            BusinessRepository.AddBusiness(business);

            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Denied", BUSINESS_ID, BOOKING_ID));
        }

        private PaymentProcessingMessage GivenIsNewCompletedPayment()
        {
            return new PaymentProcessingMessage(NEW_MSG_ID, PROVIDER_TEST, string.Format(TEST_MESSAGE, "Completed", BUSINESS_ID, BOOKING_ID));
        }


        private void WhenTryProcessMessage(PaymentProcessingMessage message)
        {
            Processor.ProcessMessage(message);
        }


        private void ThenFailsVerification()
        {
            Assert.That(PaymentProviderApiFactory.WasGetPaymentProviderApiCalled, Is.True);
            Assert.That(PaymentProviderApiFactory.PaymentProviderApi.WasVerifyPaymentCalled, Is.True);
            Assert.That(PaymentProviderApiFactory.PaymentProviderApi.VerifyPaymentResponse, Is.False);

            Assert.That(LogRepository.WasLogErrorCalled, Is.True);
            Assert.That(LogRepository.PassedInMessage, Is.EqualTo("Invalid message."));
        }

        private void ThenStopProcessingWithPendingPaymentMessageError()
        {
            AssertVerificationPassed();

            Assert.That(BusinessRepository.WasGetBusinessByIdCalled, Is.False);

            Assert.That(LogRepository.WasLogErrorCalled, Is.True);
            Assert.That(LogRepository.PassedInMessage, Is.EqualTo("Pending payment."));
        }

        private void ThenStopProcessingWithInvalidBusinessError()
        {
            AssertVerificationPassed();

            Assert.That(BusinessRepository.WasGetBusinessByIdCalled, Is.True);

            Assert.That(LogRepository.WasLogErrorCalled, Is.True);
            Assert.That(LogRepository.PassedInMessage, Is.EqualTo("Invalid business."));
        }

        private void ThenStopProcessingWithIsOnlinePaymentDisabledError()
        {
            AssertVerificationPassed();

            Assert.That(BusinessRepository.WasGetBusinessByIdCalled, Is.True);

            Assert.That(LogRepository.WasLogErrorCalled, Is.True);
            Assert.That(LogRepository.PassedInMessage, Is.EqualTo("Online payment is not enabled."));
        }

        private void ThenStopProcessingWithMerchantAccountIdentifierMismatchError()
        {
            AssertVerificationPassed();

            Assert.That(BusinessRepository.WasGetBusinessByIdCalled, Is.True);

            Assert.That(LogRepository.WasLogErrorCalled, Is.True);
            Assert.That(LogRepository.PassedInMessage, Is.EqualTo("Merchant account identifiers don't match."));
        }

        private void ThenStopProcessingWithInvalidBookingError()
        {
            AssertVerificationPassed();

            Assert.That(BusinessRepository.WasGetBusinessByIdCalled, Is.True);
            Assert.That(BusinessRepository.WasGetAllCustomerBookingsCalled, Is.True);

            Assert.That(LogRepository.WasLogErrorCalled, Is.True);
            Assert.That(LogRepository.PassedInMessage, Is.EqualTo("Invalid booking."));
        }

        private void ThenStopProcessingWithPaymentCurrencyMismatchError()
        {
            AssertVerificationPassed();

            Assert.That(BusinessRepository.WasGetBusinessByIdCalled, Is.True);
            Assert.That(BusinessRepository.WasGetAllCustomerBookingsCalled, Is.True);

            Assert.That(LogRepository.WasLogErrorCalled, Is.True);
            Assert.That(LogRepository.PassedInMessage, Is.EqualTo("The payment currency does not match the business currency."));
        }



        private void ThenLookupMessageAndReturn()
        {
            AssertVerificationPassed();

            AssertLookupPayment();
            Assert.That(DataAccessFactory.WasCreateDataAccessCalled, Is.True);
            Assert.That(BusinessRepository.WasGetBusinessByIdCalled, Is.False);
            Assert.That(LogRepository.WasLogErrorCalled, Is.True);
            Assert.That(LogRepository.PassedInMessage, Is.EqualTo("Pending payment."));
        }

        private void ThenSavePaymentAndSetBookingToPendingPayment()
        {
            AssertVerificationPassed();
            AssertLookupAndSavePayment();
            // TODO: Assert Set Booking To 'pending-payment'
        }

        private void ThenSavePaymentAndSetBookingToPaid()
        {
            AssertVerificationPassed();
            AssertLookupAndSavePayment();
            // TODO: Assert Set Booking To 'paid'
        }


        private void AssertVerificationPassed()
        {
            Assert.That(PaymentProviderApiFactory.WasGetPaymentProviderApiCalled, Is.True);
            Assert.That(PaymentProviderApiFactory.PaymentProviderApi.WasVerifyPaymentCalled, Is.True);
            Assert.That(PaymentProviderApiFactory.PaymentProviderApi.VerifyPaymentResponse, Is.True);
        }

        private void AssertLookupPayment()
        {
            Assert.That(TransactionRepository.WasGetPaymentCalled, Is.True);
            Assert.That(TransactionRepository.WasAddPaymentCalled, Is.False);
        }

        private void AssertLookupAndSavePayment()
        {
            Assert.That(TransactionRepository.WasGetPaymentCalled, Is.True);
            Assert.That(TransactionRepository.WasAddPaymentCalled, Is.True);
        }
    }
}
