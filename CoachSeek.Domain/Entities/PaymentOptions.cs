using System;
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
        public bool IsOnlinePaymentEnabled { get; protected set; }
        public bool ForceOnlinePayment { get; protected set; }
        public string PaymentProvider { get { return _paymentProvider.ProviderName; } }
        public string MerchantAccountIdentifier { get { return _paymentProvider.MerchantAccountIdentifier; } }


        public PaymentOptions(BusinessAddCommand command,
                              ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            var validation = new ValidationException();

            SetCurrencyForAdd(command.Currency, supportedCurrencyRepository, validation);
            IsOnlinePaymentEnabled = false;
            ForceOnlinePayment = false;
            _paymentProvider = PaymentProviderFactory.CreateDefaultPaymentProvider();

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

            validation.ThrowIfErrors();

            Validate();
        }

        public PaymentOptions(string currency, 
                              bool isOnlinePaymentEnabled, 
                              bool forceOnlinePayment, 
                              string paymentProvider, 
                              string merchantAccountIdentifier)
        {
            // Testing constructor

            _currency = new Currency(currency);
            IsOnlinePaymentEnabled = isOnlinePaymentEnabled;
            ForceOnlinePayment = forceOnlinePayment;
            _paymentProvider = PaymentProviderFactory.CreatePaymentProvider(paymentProvider,
                                                                            merchantAccountIdentifier);
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
            catch (CurrencyNotSupported currencyNotSupported)
            {
                errors.Add(currencyNotSupported);
            }
        }

        private void SetPaymentProvider(BusinessPaymentCommand payment, ValidationException errors)
        {
            try
            {
                _paymentProvider = PaymentProviderFactory.CreatePaymentProvider(payment.PaymentProvider,
                                                                                payment.MerchantAccountIdentifier);
            }
            catch (Exception ex)
            {
                if (ex is SingleErrorException)
                    errors.Add(ex as SingleErrorException);
            }
        }

        private void Validate()
        {
            var validation = new ValidationException();

            if (IsOnlinePaymentEnabled)
                if (_paymentProvider is NullPaymentProvider)
                    validation.Add(new PaymentProviderRequiredWhenOnlineBookingIsEnabled());

            validation.ThrowIfErrors();
        }
    }
}
