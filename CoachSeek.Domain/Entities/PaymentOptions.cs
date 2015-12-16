using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Factories;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class PaymentOptions
    {
        private Currency _currency;
        private PaymentProviderBase _paymentProvider;

        public string CurrencyCode { get { return _currency.Code; } }
        public string CurrencySymbol { get { return _currency.Symbol; } }
        public bool IsOnlinePaymentEnabled { get; protected set; }
        public bool ForceOnlinePayment { get; protected set; }
        public string PaymentProvider { get { return _paymentProvider.ProviderName; } }
        public string MerchantAccountIdentifier { get { return _paymentProvider.MerchantAccountIdentifier; } }
        public bool UseProRataPricing { get; protected set; }


        public PaymentOptions(BusinessAddCommand command,
                              ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            var validation = new ValidationException();

            SetCurrencyForAdd(command.Currency, supportedCurrencyRepository, validation);
            IsOnlinePaymentEnabled = false;
            ForceOnlinePayment = false;
            _paymentProvider = PaymentProviderFactory.CreateDefaultPaymentProvider();
            UseProRataPricing = true;

            validation.ThrowIfErrors();
        }

        public PaymentOptions(BusinessUpdateCommand command, 
                              ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            var validation = new ValidationException();

            SetCurrencyForUpdate(command.Payment.Currency, supportedCurrencyRepository, validation);
            IsOnlinePaymentEnabled = command.Payment.IsOnlinePaymentEnabled;
            ForceOnlinePayment = command.Payment.ForceOnlinePayment;
            SetPaymentProvider(command.Payment, validation);
            UseProRataPricing = command.Payment.UseProRataPricing;

            validation.ThrowIfErrors();
        }

        public PaymentOptions(string currencyCode,
                              string currencySymbol,
                              bool isOnlinePaymentEnabled, 
                              bool forceOnlinePayment, 
                              string paymentProvider, 
                              string merchantAccountIdentifier,
                              bool useProRataPricing = true)
        {
            // Testing constructor

            _currency = new Currency(currencyCode, currencySymbol);
            IsOnlinePaymentEnabled = isOnlinePaymentEnabled;
            ForceOnlinePayment = forceOnlinePayment;
            _paymentProvider = PaymentProviderFactory.CreatePaymentProvider(paymentProvider,
                                                                            merchantAccountIdentifier);
            UseProRataPricing = useProRataPricing;
        }

        public PaymentOptions(BusinessPaymentData payment, ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            _currency = new Currency(supportedCurrencyRepository.GetByCode(payment.Currency));
            IsOnlinePaymentEnabled = payment.IsOnlinePaymentEnabled;
            ForceOnlinePayment = payment.ForceOnlinePayment;
            _paymentProvider = PaymentProviderFactory.CreatePaymentProvider(payment.PaymentProvider,
                                                                            payment.MerchantAccountIdentifier);
            UseProRataPricing = payment.UseProRataPricing;
        }


        private void SetCurrencyForAdd(string currency,
                                       ISupportedCurrencyRepository supportedCurrencyRepository,
                                       ValidationException errors)
        {
            SetCurrency(currency, supportedCurrencyRepository, errors);
        }

        private void SetCurrencyForUpdate(string currency,
                                          ISupportedCurrencyRepository supportedCurrencyRepository,
                                          ValidationException errors)
        {
            SetCurrency(currency, supportedCurrencyRepository, errors);
        }

        private void SetCurrency(string currency,
                                 ISupportedCurrencyRepository supportedCurrencyRepository,
                                 ValidationException errors)
        {
            try
            {
                _currency = new Currency(currency, supportedCurrencyRepository);
            }
            catch (SingleErrorException ex)
            {
                errors.Add(ex);
            }
        }

        private void SetPaymentProvider(BusinessPaymentCommand payment, ValidationException errors)
        {
            try
            {
                _paymentProvider = PaymentProviderFactory.CreatePaymentProvider(payment.PaymentProvider,
                                                                                payment.MerchantAccountIdentifier);

                if (IsOnlinePaymentEnabled && _paymentProvider is NullPaymentProvider)
                        throw new PaymentProviderRequiredWhenOnlineBookingIsEnabled();
            }
            catch (SingleErrorException ex)
            {
                errors.Add(ex);
            }
        }
    }
}
