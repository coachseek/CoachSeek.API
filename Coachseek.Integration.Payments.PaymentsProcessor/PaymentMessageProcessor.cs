using System.Threading.Tasks;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Services;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Payments.Exceptions;
using Coachseek.Integration.Contracts.Payments.Interfaces;
using Environment = CoachSeek.Common.Environment;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class PaymentMessageProcessor : IPaymentMessageProcessor
    {
        public IPaymentProcessorConfiguration PaymentProcessorConfiguration { get; private set; }
        public IDataAccessFactory DataAccessFactory { get; private set; }
        public IPaymentProviderApiFactory PaymentProviderApiFactory { get; private set; }

        private Environment Environment
        {
            get { return PaymentProcessorConfiguration.Environment; }
        }


        public PaymentMessageProcessor(IPaymentProcessorConfiguration paymentProcessorConfiguration,
                                       IDataAccessFactory dataAccessFactory,
                                       IPaymentProviderApiFactory paymentProviderApiFactory)
        {
            PaymentProcessorConfiguration = paymentProcessorConfiguration;
            DataAccessFactory = dataAccessFactory;
            PaymentProviderApiFactory = paymentProviderApiFactory;
        }

        public async Task ProcessMessageAsync(PaymentProcessingMessage message)
        {
            var dataAccess = GetDataAccess();
            try
            {
                var newPayment = NewPaymentConverter.Convert(message);
                await VerifyMessageAsync(message, newPayment.IsTesting);
                await ProcessPaymentAsync(newPayment, dataAccess);
                await dataAccess.LogRepository.LogInfoAsync("Message successfully processed.", message.ToString());
            }
            catch (PaymentProcessingException ex)
            {
                dataAccess.LogRepository.LogErrorAsync(ex, message.ToString()).Wait();
            }
        }


        private Contracts.Payments.Models.DataRepositories GetDataAccess()
        {
            var isTesting = PaymentProcessorConfiguration.Environment != Environment.Production;
            return DataAccessFactory.CreateDataAccess(isTesting);
        }

        private async Task VerifyMessageAsync(PaymentProcessingMessage message, bool isTestMessage)
        {
            var paymentApi = PaymentProviderApiFactory.GetPaymentProviderApi(message, isTestMessage);
            if (!await paymentApi.VerifyPaymentAsync(message))
                throw new InvalidPaymentMessage();
        }

        private async Task ProcessPaymentAsync(NewPayment newPayment, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            await ValidatePaymentAsync(newPayment, dataAccess);
            newPayment = await ModifyPaymentAsync(newPayment, dataAccess);
            var payment = await SaveIfNewPaymentAsync(newPayment, dataAccess);
            if (payment.IsCompleted)
                await SetBookingAsPaidAsync(payment, dataAccess);
        }

        private async Task ValidatePaymentAsync(NewPayment newPayment, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            var parameters = new PaymentParameters(newPayment);
            ValidatePaymentStatus(parameters);
            parameters.Business = await GetAndValidateBusinessAsync(parameters, dataAccess);
            parameters.CustomerBooking = await GetAndValidateCustomerBookingAsync(parameters, dataAccess);
            await ValidateBookingPaymentAmountAsync(parameters, dataAccess);
        }

        private async Task<Business> GetAndValidateBusinessAsync(PaymentParameters parameters, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            var businessData = await dataAccess.BusinessRepository.GetBusinessAsync(parameters.Payment.MerchantId);
            if (businessData.IsNotFound())
                throw new InvalidBusiness();
            var business = new Business(businessData, dataAccess.SupportedCurrencyRepository);
            ValidateBusiness(business, parameters.Payment);
            return business;
        }

        private async Task<NewPayment> ModifyPaymentAsync(NewPayment newPayment, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            if (newPayment.PaymentProvider == Constants.PAYPAL)
                return await ModifyPaymentForPaypalAsync(newPayment, dataAccess);
            return newPayment;
        }

        private async Task<NewPayment> ModifyPaymentForPaypalAsync(NewPayment newPayment, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            var business = await dataAccess.BusinessRepository.GetBusinessAsync(newPayment.MerchantId);
            return new NewPayment(newPayment, business.Name);
        }

        private void ValidatePaymentStatus(PaymentParameters parameters)
        {
            if (Environment != Environment.Production && !parameters.Payment.IsTesting)
                throw new ProductionMessageForNonProductionEnvironment();
            if (parameters.Payment.IsPending)
                throw new PendingPayment();
        }

        private void ValidateBusiness(Business business, NewPayment newPayment)
        {
            if (!business.IsOnlinePaymentEnabled)
                throw new OnlinePaymentNotEnabled();
            if (newPayment.PaymentProvider != business.PaymentProvider)
                throw new PaymentProviderMismatch();
            if (newPayment.MerchantEmail != business.MerchantAccountIdentifier)
                throw new MerchantAccountIdentifierMismatch();
            if (newPayment.ItemCurrency != business.CurrencyCode)
                throw new PaymentCurrencyMismatch();
        }

        private async Task<CustomerBookingData> GetAndValidateCustomerBookingAsync(PaymentParameters parameters, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            var customerBooking = await dataAccess.BusinessRepository.GetCustomerBookingAsync(parameters.Business.Id, parameters.Payment.ItemId);
            ValidateCustomerBooking(customerBooking, parameters.Payment);
            return customerBooking;
        }

        private void ValidateCustomerBooking(CustomerBookingData customerBooking, NewPayment newPayment)
        {
            if (customerBooking.IsNotFound())
                throw new InvalidBooking();
        }

        private async Task ValidateBookingPaymentAmountAsync(PaymentParameters parameters, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            var booking = await GetSessionOrCourseBookingAsync(parameters, dataAccess);
            if (booking is SingleSessionBookingData)
            {
                var session = await dataAccess.BusinessRepository.GetSessionAsync(parameters.Business.Id, parameters.CustomerBooking.SessionId);
                ValidateSingleSessionPaymentAmount(session, (SingleSessionBookingData)booking, parameters.Payment);
            }
            else
            {
                var course = await dataAccess.BusinessRepository.GetCourseAsync(parameters.Business.Id, parameters.CustomerBooking.SessionId);
                ValidateCoursePaymentAmount(course, (CourseBookingData)booking, parameters.Payment, parameters.Business.UseProRataPricing);
            }
        }

        private static void ValidateSingleSessionPaymentAmount(SingleSessionData session, SingleSessionBookingData booking, NewPayment newPayment)
        {
            var expectedPrice = session.Pricing.SessionPrice.GetValueOrDefault().ApplyDiscount(booking.DiscountPercent);
            if (newPayment.ItemAmount != expectedPrice)
                throw new PaymentAmountMismatch(newPayment.ItemAmount, session.Pricing.SessionPrice.GetValueOrDefault());
        }

        private static void ValidateCoursePaymentAmount(RepeatedSessionData course, CourseBookingData booking, NewPayment newPayment, bool useProRataPricing)
        {
            var expectedPrice = CourseBookingPriceCalculator.CalculatePrice(booking, course, useProRataPricing, booking.DiscountPercent);
            if (newPayment.ItemAmount != expectedPrice)
                throw new PaymentAmountMismatch(newPayment.ItemAmount, expectedPrice);
        }

        private async Task<BookingData> GetSessionOrCourseBookingAsync(PaymentParameters parameters, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            var sessionBooking = await dataAccess.BusinessRepository.GetSessionBookingAsync(parameters.Business.Id, parameters.CustomerBooking.Id);
            if (sessionBooking.IsFound())
                return sessionBooking;
            var courseBooking = await dataAccess.BusinessRepository.GetCourseBookingAsync(parameters.Business.Id, parameters.CustomerBooking.Id);
            return courseBooking;
        }

        private async Task<Payment> SaveIfNewPaymentAsync(NewPayment newPayment, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            var repository = dataAccess.TransactionRepository;
            var payment = await repository.GetPaymentAsync(newPayment.PaymentProvider, newPayment.Id);
            if (payment.IsNotFound())
            {
                await repository.AddPaymentAsync(newPayment);
                payment = newPayment;
            }
            return payment;
        }

        private async Task SetBookingAsPaidAsync(Payment payment, Contracts.Payments.Models.DataRepositories dataAccess)
        {
            await dataAccess.BusinessRepository.SetBookingPaymentStatusAsync(payment.MerchantId, payment.ItemId, Constants.PAYMENT_STATUS_PAID);
        }


        private class PaymentParameters
        {
            public NewPayment Payment { get; private set; }
            public Business Business { get; set; }
            public CustomerBookingData CustomerBooking { get; set; }

            public PaymentParameters(NewPayment payment)
            {
                Payment = payment;
            }
        }
    }
}
