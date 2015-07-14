using System;
using System.Linq;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Exceptions;
using Coachseek.Integration.Contracts.Interfaces;
using InvalidBooking = Coachseek.Integration.Contracts.Exceptions.InvalidBooking;
using InvalidBusiness = Coachseek.Integration.Contracts.Exceptions.InvalidBusiness;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class PaymentMessageProcessor : IPaymentMessageProcessor
    {
        public IPaymentProcessorConfiguration PaymentProcessorConfiguration { get; private set; }
        public IDataAccessFactory DataAccessFactory { get; private set; }
        public IPaymentProviderApiFactory PaymentProviderApiFactory { get; private set; }


        public PaymentMessageProcessor(IPaymentProcessorConfiguration paymentProcessorConfiguration,
                                       IDataAccessFactory dataAccessFactory,
                                       IPaymentProviderApiFactory paymentProviderApiFactory)
        {
            PaymentProcessorConfiguration = paymentProcessorConfiguration;
            DataAccessFactory = dataAccessFactory;
            PaymentProviderApiFactory = paymentProviderApiFactory;
        }


        public void ProcessMessage(PaymentProcessingMessage message)
        {
            var isTesting = !PaymentProcessorConfiguration.IsPaymentEnabled;
            var dataAccess = DataAccessFactory.CreateDataAccess(isTesting);

            try
            {
                VerifyMessage(message);
                var newPayment = NewPaymentConverter.Convert(message);
                if (newPayment.IsTesting != isTesting)
                    throw new IsTestingStatusMismatch();
                ProcessPayment(newPayment, dataAccess);
                dataAccess.LogRepository.LogInfo("Message successfully processed.", message.ToString());
            }
            catch (PaymentProcessingException ex)
            {
                dataAccess.LogRepository.LogError(ex, message.ToString());
            }
        }


        private void VerifyMessage(PaymentProcessingMessage message)
        {
            var isPaymentEnabled = PaymentProcessorConfiguration.IsPaymentEnabled;
            var paymentApi = PaymentProviderApiFactory.GetPaymentProviderApi(message, isPaymentEnabled);
            if (!paymentApi.VerifyPayment(message))
                throw new InvalidPaymentMessage();
        }

        private void ProcessPayment(NewPayment newPayment, DataRepositories dataAccess)
        {
            ValidatePayment(newPayment, dataAccess);
            var payment = SaveIfNewPayment(newPayment, dataAccess);
            if (payment.IsCompleted)
                SetBookingAsPaid(payment, dataAccess);
        }

        private void ValidatePayment(NewPayment newPayment, DataRepositories dataAccess)
        {
            ValidatePaymentStatus(newPayment);
            var business = dataAccess.BusinessRepository.GetBusiness(newPayment.MerchantId);
            ValidateBusiness(business, newPayment);
            var booking = GetBooking(dataAccess, business.Id, newPayment.ItemId);
            ValidateBooking(booking, newPayment);
            var sessionOrCourse = GetSessionOrCourse(business.Id, booking.SessionId, dataAccess);
            ValidateSessionOrCourse(sessionOrCourse, newPayment);
        }

        private void ValidatePaymentStatus(NewPayment newPayment)
        {
            if (newPayment.IsPending)
                throw new PendingPayment();
        }

        private void ValidateBusiness(BusinessData business, NewPayment newPayment)
        {
            if (business.IsNotFound())
                throw new InvalidBusiness();
            if (!business.Payment.IsOnlinePaymentEnabled)
                throw new OnlinePaymentNotEnabled();
            if (newPayment.PaymentProvider != business.Payment.PaymentProvider)
                throw new PaymentProviderMismatch();
            if (newPayment.MerchantEmail != business.Payment.MerchantAccountIdentifier)
                throw new MerchantAccountIdentifierMismatch();
            if (newPayment.ItemCurrency != business.Payment.Currency)
                throw new PaymentCurrencyMismatch();
        }

        private CustomerBookingData GetBooking(DataRepositories dataAccess, Guid businessId, Guid itemId)
        {
            var bookings = dataAccess.BusinessRepository.GetAllCustomerBookings(businessId);
            return bookings.SingleOrDefault(x => x.Id == itemId);
        }

        private void ValidateBooking(CustomerBookingData booking, NewPayment newPayment)
        {
            if (booking.IsNotFound())
                throw new InvalidBooking();
        }

        private void ValidateSessionOrCourse(Session sessionOrCourse, NewPayment newPayment)
        {
            if (sessionOrCourse is SingleSession)
                if (newPayment.ItemAmount != ((SingleSession)sessionOrCourse).Pricing.SessionPrice.Value)
                    throw new PaymentAmountMismatch();
            if (sessionOrCourse is RepeatedSession)
                if (newPayment.ItemAmount != ((RepeatedSession)sessionOrCourse).Pricing.CoursePrice.Value)
                    throw new PaymentAmountMismatch();
        }

        protected Session GetSessionOrCourse(Guid businessId, Guid sessionId, DataRepositories dataAccess)
        {
            // Is it a Session or a Course?
            var session = dataAccess.BusinessRepository.GetSession(businessId, sessionId);
            if (session.IsExisting())
            {
                if (session.ParentId == null)
                    return new StandaloneSession(session, LookupCoreData(businessId, session, dataAccess));
                return new SessionInCourse(session, LookupCoreData(businessId, session, dataAccess));
            }

            var course = dataAccess.BusinessRepository.GetCourse(businessId, sessionId);
            if (course.IsExisting())
                return new RepeatedSession(course,
                                           dataAccess.BusinessRepository.GetAllLocations(businessId),
                                           dataAccess.BusinessRepository.GetAllCoaches(businessId),
                                           dataAccess.BusinessRepository.GetAllServices(businessId));

            return null;
        }

        private CoreData LookupCoreData(Guid businessId, SessionData data, DataRepositories dataAccess)
        {
            var location = dataAccess.BusinessRepository.GetLocation(businessId, data.Location.Id);
            var coach = dataAccess.BusinessRepository.GetCoach(businessId, data.Coach.Id);
            var service = dataAccess.BusinessRepository.GetService(businessId, data.Service.Id);

            return new CoreData(location, coach, service);
        }

        private Payment SaveIfNewPayment(NewPayment newPayment, DataRepositories dataAccess)
        {
            var repository = dataAccess.TransactionRepository;
            var payment = repository.GetPayment(newPayment.PaymentProvider, newPayment.Id);
            if (payment.IsNotFound())
            {
                repository.AddPayment(newPayment);
                payment = newPayment;
            }
            return payment;
        }

        private void SetBookingAsPaid(Payment payment, DataRepositories dataAccess)
        {
            dataAccess.BusinessRepository.SetBookingPaymentStatus(payment.MerchantId, payment.ItemId, Constants.PAYMENT_STATUS_PAID);
        }
    }
}
