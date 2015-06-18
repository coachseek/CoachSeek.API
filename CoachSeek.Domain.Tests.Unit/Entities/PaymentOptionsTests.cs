using System;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    public class PaymentOptionsTests : Tests
    {
        [TestFixture]
        public class PaymentOptionsAddCommandTests : PaymentOptionsTests
        {
            [Test]
            public void GivenInvalidCurrency_WhenTryConstructPaymentOptions_ThenReturnUnsupportedCurrencyError()
            {
                var parameters = GivenInvalidCurrency();
                var response = WhenTryConstructPaymentOptions(parameters);
                ThenReturnUnsupportedCurrencyError(response);
            }

            [Test]
            public void GivenValidCommand_WhenTryConstructPaymentOptions_ThenCreatesValidPaymentOptions()
            {
                var parameters = GivenValidCommand();
                var response = WhenTryConstructPaymentOptions(parameters);
                ThenCreatesValidPaymentOptions(response);
            }


            private BusinessAddCommand GivenInvalidCurrency()
            {
                return new BusinessAddCommand
                {
                    Name = "Business Invalid",
                    Currency = "XXX"
                };
            }

            private BusinessAddCommand GivenValidCommand()
            {
                return new BusinessAddCommand
                {
                    Name = "Business Valid",
                    Currency = "AUD"
                };
            }


            private object WhenTryConstructPaymentOptions(BusinessAddCommand command)
            {
                try
                {
                    return new PaymentOptions(command, new HardCodedSupportedCurrencyRepository());
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }

            
            protected void ThenReturnUnsupportedCurrencyError(object response)
            {
                AssertSingleError(response, "This currency is not supported.", "business.currency");
            }

            private void ThenCreatesValidPaymentOptions(object response)
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.InstanceOf<PaymentOptions>());

                var payment = (PaymentOptions)response;
                Assert.That(payment.CurrencyCode, Is.EqualTo("AUD"));
                Assert.That(payment.IsOnlinePaymentEnabled, Is.False);
                Assert.That(payment.ForceOnlinePayment, Is.Null);
                Assert.That(payment.PaymentProvider, Is.Null);
                Assert.That(payment.MerchantAccountIdentifier, Is.Null);
            }
        }

        [TestFixture]
        public class PaymentOptionsUpdateCommandTests : PaymentOptionsTests
        {
            [Test]
            public void GivenInvalidCurrency_WhenTryConstructPaymentOptions_ThenReturnUnsupportedCurrencyError()
            {
                var parameters = GivenInvalidCurrency();
                var response = WhenTryConstructPaymentOptions(parameters);
                ThenReturnUnsupportedCurrencyError(response);
            }

            [Test]
            public void GivenInvalidPaymentProvider_WhenTryConstructPaymentOptions_ThenReturnUnsupportedPaymentProviderError()
            {
                var parameters = GivenInvalidPaymentProvider();
                var response = WhenTryConstructPaymentOptions(parameters);
                ThenReturnUnsupportedPaymentProviderError(response);
            }

            [Test]
            public void GivenInvalidCurrencyAndPaymentProvider_WhenTryConstructPaymentOptions_ThenReturnUnsupportedCurrencyAndPaymentProviderErrors()
            {
                var parameters = GivenInvalidCurrencyAndPaymentProvider();
                var response = WhenTryConstructPaymentOptions(parameters);
                ThenReturnUnsupportedCurrencyAndPaymentProviderErrors(response);
            }

            [Test]
            public void GivenOnlinePaymentEnabledButMissingForceOnlinePayment_WhenTryConstructPaymentOptions_ThenReturnMissingForceOnlinePaymentError()
            {
                var parameters = GivenOnlinePaymentEnabledButMissingForceOnlinePayment();
                var response = WhenTryConstructPaymentOptions(parameters);
                ThenReturnMissingForceOnlinePaymentError(response);
            }

            [Test]
            public void GivenOnlinePaymentEnabledButMissingPaymentProvider_WhenTryConstructPaymentOptions_ThenReturnMissingPaymentProviderError()
            {
                var parameters = GivenOnlinePaymentEnabledButMissingPaymentProvider();
                var response = WhenTryConstructPaymentOptions(parameters);
                ThenReturnMissingPaymentProviderError(response);
            }

            [Test]
            public void GivenValidCommand_WhenTryConstructPaymentOptions_ThenCreatesValidPaymentOptions()
            {
                var parameters = GivenValidCommand();
                var response = WhenTryConstructPaymentOptions(parameters);
                ThenCreatesValidPaymentOptions(response);
            }


            private BusinessUpdateCommand GivenInvalidCurrency()
            {
                return new BusinessUpdateCommand
                {
                    Name = "Business Invalid",
                    Payment = new BusinessPaymentCommand
                    {
                        Currency = "ZZZ",
                        IsOnlinePaymentEnabled = false
                    }
                };
            }

            private BusinessUpdateCommand GivenInvalidPaymentProvider()
            {
                return new BusinessUpdateCommand
                {
                    Name = "Business Invalid",
                    Payment = new BusinessPaymentCommand
                    {
                        Currency = "NZD",
                        IsOnlinePaymentEnabled = false,
                        PaymentProvider = "FRED"
                    }
                };
            }

            private BusinessUpdateCommand GivenInvalidCurrencyAndPaymentProvider()
            {
                return new BusinessUpdateCommand
                {
                    Name = "Business Invalid",
                    Payment = new BusinessPaymentCommand
                    {
                        Currency = "WWW",
                        IsOnlinePaymentEnabled = false,
                        PaymentProvider = "OLAF"
                    }
                };
            }

            private BusinessUpdateCommand GivenOnlinePaymentEnabledButMissingForceOnlinePayment()
            {
                return new BusinessUpdateCommand
                {
                    Name = "Business Invalid",
                    Payment = new BusinessPaymentCommand
                    {
                        Currency = "USD",
                        IsOnlinePaymentEnabled = true,
                        ForceOnlinePayment = null,
                        PaymentProvider = "PayPal",
                        MerchantAccountIdentifier = "olaf@coachseek.com"
                    }
                };
            }

            private BusinessUpdateCommand GivenOnlinePaymentEnabledButMissingPaymentProvider()
            {
                return new BusinessUpdateCommand
                {
                    Name = "Business Invalid",
                    Payment = new BusinessPaymentCommand
                    {
                        Currency = "USD",
                        IsOnlinePaymentEnabled = true,
                        ForceOnlinePayment = false
                    }
                };
            }

            private BusinessUpdateCommand GivenOnlinePaymentEnabledAndInvalidPaymentProvider()
            {
                return new BusinessUpdateCommand
                {
                    Name = "Business Invalid",
                    Payment = new BusinessPaymentCommand
                    {
                        Currency = "USD",
                        IsOnlinePaymentEnabled = true,
                        ForceOnlinePayment = true,
                        PaymentProvider = "Fred"
                    }
                };
            }

            private BusinessUpdateCommand GivenValidCommand()
            {
                return new BusinessUpdateCommand
                {
                    Name = "Business Valid",
                    Payment = new BusinessPaymentCommand
                    {
                        Currency = "GBP",
                        IsOnlinePaymentEnabled = true,
                        ForceOnlinePayment = false,
                        PaymentProvider = "PayPal",
                        MerchantAccountIdentifier = "olaf@coachseek.com"
                    }
                };
            }

            private object WhenTryConstructPaymentOptions(BusinessUpdateCommand command)
            {
                try
                {
                    return new PaymentOptions(command, new HardCodedSupportedCurrencyRepository());
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }


            protected void ThenReturnUnsupportedCurrencyError(object response)
            {
                AssertSingleError(response, "This currency is not supported.", "business.payment.currency");
            }

            private void ThenReturnUnsupportedPaymentProviderError(object response)
            {
                AssertSingleError(response, "This payment provider is not supported.", "business.payment.paymentProvider");
            }

            private void ThenReturnUnsupportedCurrencyAndPaymentProviderErrors(object response)
            {
                AssertMultipleErrors(response, new[,] { { "This currency is not supported.", "business.payment.currency" },
                                                        { "This payment provider is not supported.", "business.payment.paymentProvider" } });
            }

            private void ThenReturnMissingForceOnlinePaymentError(object response)
            {
                AssertSingleError(response, "When Online Payment is enabled then the ForceOnlinePayment option must be specified.");
            }

            private void ThenReturnMissingPaymentProviderError(object response)
            {
                AssertSingleError(response, "When Online Payment is enabled then an Online Payment Provider must be specified.");
            }

            private void ThenCreatesValidPaymentOptions(object response)
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.InstanceOf<PaymentOptions>());

                var payment = (PaymentOptions)response;
                Assert.That(payment.CurrencyCode, Is.EqualTo("GBP"));
                Assert.That(payment.IsOnlinePaymentEnabled, Is.True);
                Assert.That(payment.ForceOnlinePayment, Is.False);
                Assert.That(payment.PaymentProvider, Is.EqualTo("PayPal"));
                Assert.That(payment.MerchantAccountIdentifier, Is.EqualTo("olaf@coachseek.com"));
            }
        }
    }
}
