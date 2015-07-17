using System.Web;
using CoachSeek.Common;
using CoachSeek.Domain.Exceptions;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Exceptions;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public static class IsTestMessageDeterminator
    {
        //public static bool IsTestMessage(PaymentProcessingMessage message)
        //{
        //    try
        //    {
        //        return CheckIsTestMessage(message);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        throw new PaymentConversionException(ex);
        //    }
        //}

        public static bool IsTestMessage(PaymentProcessingMessage message)
        {
            if (message.PaymentProvider == Constants.PAYPAL)
                return IsTestMessageFromPaypal(message.Contents);

            if (message.PaymentProvider == Constants.TEST)
                return true;

            throw new UnknownPaymentProvider();
        }


        public static bool IsTestMessageFromPaypal(string paypalMessage)
        {
            var keyValuePairs = HttpUtility.ParseQueryString(paypalMessage);

            return GetIsTestingFromPaypal(keyValuePairs.Get("test_ipn"));
        }

        private static bool GetIsTestingFromPaypal(string paypalIsTesting)
        {
            return paypalIsTesting != null && paypalIsTesting == "1";
        }
    }
}
