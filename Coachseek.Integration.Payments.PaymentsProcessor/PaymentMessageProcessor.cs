using System;
using System.Linq;
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
            var dataAccess = DataAccessFactory.CreateProductionDataAccess();

            try
            {
                VerifyMessage(message);
                var newPayment = NewPaymentConverter.Convert(message);
                dataAccess = DataAccessFactory.CreateDataAccess(newPayment.IsTesting);
                ProcessPayment(newPayment, dataAccess);
            }
            catch (PaymentProcessingException ex)
            {
                dataAccess.LogRepository.LogError(ex);
            }
        }

        public void ProcessPayment(NewPayment newPayment, DataRepositories dataAccess)
        {
            ValidatePaymentStatus(newPayment);
            var business = dataAccess.BusinessRepository.GetBusiness(newPayment.MerchantId);
            ValidateBusiness(business, newPayment);
            var booking = GetBooking(dataAccess, business.Id, newPayment.ItemId);
            if (booking.IsNotFound())
                throw new InvalidBooking();
            if (newPayment.ItemCurrency != business.Payment.Currency)
                throw new PaymentCurrencyMismatch();
            var sessionOrCourse = GetSessionOrCourse(business.Id, booking.SessionId, dataAccess);
            if (sessionOrCourse is SingleSession)
                if (newPayment.ItemAmount != ((SingleSession)sessionOrCourse).Pricing.SessionPrice.Value)
                    throw new PaymentAmountMismatch();
            if (sessionOrCourse is RepeatedSession)
                if (newPayment.ItemAmount != ((RepeatedSession)sessionOrCourse).Pricing.CoursePrice.Value)
                    throw new PaymentAmountMismatch();
            var payment = SaveIfNewPayment(newPayment, dataAccess);
            //if (payment.IsCompleted)
                //dataAccess.BusinessRepository.PayBooking();
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
            if (newPayment.MerchantEmail != business.Payment.MerchantAccountIdentifier)
                throw new MerchantAccountIdentifierMismatch();
        }

        private CustomerBookingData GetBooking(DataRepositories dataAccess, Guid businessId, Guid itemId)
        {
            var bookings = dataAccess.BusinessRepository.GetAllCustomerBookings(businessId);
            return bookings.SingleOrDefault(x => x.Id == itemId);
        }

        //private void ValidateBooking(Session sessionOrCourse, NewPayment payment)
        //{
        //}

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
                return new RepeatedSession(course, LookupCoreData(businessId, session, dataAccess));

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
            var payment = repository.GetPayment(newPayment.Id);
            if (payment.IsNotFound())
            {
                repository.AddPayment(newPayment);
                payment = newPayment;
            }
            return payment;
        }

        private void VerifyMessage(PaymentProcessingMessage message)
        {
            var isPaymentEnabled = PaymentProcessorConfiguration.IsPaymentEnabled;
            var paymentApi = PaymentProviderApiFactory.GetPaymentProviderApi(message, isPaymentEnabled);
            if (!paymentApi.VerifyPayment(message))
                throw new InvalidPaymentMessage();
        }
    }
}
