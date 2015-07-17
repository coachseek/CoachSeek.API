using System;
using System.Linq;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Exceptions;
using Coachseek.Integration.Contracts.Interfaces;
using Environment = CoachSeek.Common.Environment;
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

        private Environment Environment
        {
            get { return PaymentProcessorConfiguration.Environment; }
        }

        public void ProcessMessage(PaymentProcessingMessage message)
        {
            var dataAccess = GetDataAccess();
            try
            {
                var newPayment = NewPaymentConverter.Convert(message);
                VerifyMessage(message, newPayment.IsTesting);
                ProcessPayment(newPayment, dataAccess);
                dataAccess.LogRepository.LogInfo("Message successfully processed.", message.ToString());
            }
            catch (PaymentProcessingException ex)
            {
                dataAccess.LogRepository.LogError(ex, message.ToString());
            }
        }

        private DataRepositories GetDataAccess()
        {
            var isTesting = PaymentProcessorConfiguration.Environment != Environment.Production;
            return DataAccessFactory.CreateDataAccess(isTesting);
        }

        private void VerifyMessage(PaymentProcessingMessage message, bool isTestMessage)
        {
            var paymentApi = PaymentProviderApiFactory.GetPaymentProviderApi(message, isTestMessage);
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
            var customerBooking = GetCustomerBooking(dataAccess, business.Id, newPayment.ItemId);
            ValidateCustomerBooking(customerBooking, newPayment);
            var booking = GetSessionOrCourseBooking(business.Id, newPayment.ItemId, dataAccess);
            if (booking is SingleSessionBookingData)
            {
                var session = dataAccess.BusinessRepository.GetSession(business.Id, customerBooking.SessionId);
                ValidateSingleSessionPaymentAmount(session, newPayment);
                return;
            }
            var course = dataAccess.BusinessRepository.GetCourse(business.Id, customerBooking.SessionId);
            ValidateCoursePaymentAmount(course, (CourseBookingData)booking, newPayment);
        }

        private void ValidatePaymentStatus(NewPayment newPayment)
        {
            if (Environment != Environment.Production && !newPayment.IsTesting)
                throw new ProductionMessageForNonProductionEnvironment();
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

        private CustomerBookingData GetCustomerBooking(DataRepositories dataAccess, Guid businessId, Guid itemId)
        {
            var bookings = dataAccess.BusinessRepository.GetAllCustomerBookings(businessId);
            return bookings.SingleOrDefault(x => x.Id == itemId);
        }

        private void ValidateCustomerBooking(CustomerBookingData customerBooking, NewPayment newPayment)
        {
            if (customerBooking.IsNotFound())
                throw new InvalidBooking();
        }

        private static void ValidateSingleSessionPaymentAmount(SingleSessionData session, NewPayment newPayment)
        {
            if (newPayment.ItemAmount != session.Pricing.SessionPrice.Value)
                throw new PaymentAmountMismatch();
        }

        private static void ValidateCoursePaymentAmount(RepeatedSessionData course, CourseBookingData booking, NewPayment newPayment)
        {
            if (IsBookingForWholeCourse(booking, course))
                ValidateWholeCoursePaymentAmount(course, newPayment);
            else
                ValidateMultipleSessionPaymentAmount(course, booking, newPayment);
        }

        private static void ValidateWholeCoursePaymentAmount(RepeatedSessionData course, NewPayment newPayment)
        {
            var coursePrice = course.Pricing.CoursePrice ??
                              course.Pricing.SessionPrice.Value * course.Repetition.SessionCount;

            if (newPayment.ItemAmount != coursePrice)
                throw new PaymentAmountMismatch();
        }

        private static void ValidateMultipleSessionPaymentAmount(RepeatedSessionData course, CourseBookingData booking, NewPayment newPayment)
        {
            var sessionPrice = course.Pricing.SessionPrice ??
                               Math.Round(course.Pricing.CoursePrice.Value / course.Repetition.SessionCount, 2);

            if (newPayment.ItemAmount != sessionPrice * booking.SessionBookings.Count)
                throw new PaymentAmountMismatch();
        }

        private static bool IsBookingForWholeCourse(CourseBookingData booking, RepeatedSessionData course)
        {
            return booking.SessionBookings.Count == course.Sessions.Count;
        }

        protected BookingData GetSessionOrCourseBooking(Guid businessId, Guid bookingId, DataRepositories dataAccess)
        {
            // GetSessionBooking also returns course bookings so call GetCourseBooking first. TODO
            var courseBooking = dataAccess.BusinessRepository.GetCourseBooking(businessId, bookingId);
            if (courseBooking.IsExisting())
                return courseBooking;
            
            return dataAccess.BusinessRepository.GetSessionBooking(businessId, bookingId);
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
