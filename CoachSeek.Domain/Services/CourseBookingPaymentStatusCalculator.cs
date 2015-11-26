using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CoachSeek.Common;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Services
{
    public static class CourseBookingPaymentStatusCalculator
    {
        public static string CalculatePaymentStatus(IReadOnlyCollection<SingleSessionBookingData> sessionBookings)
        {
            if (sessionBookings.Any(x => x.PaymentStatus == Constants.PAYMENT_STATUS_OVERDUE_PAYMENT))
                return Constants.PAYMENT_STATUS_OVERDUE_PAYMENT;
            if (sessionBookings.Any(x => x.PaymentStatus == Constants.PAYMENT_STATUS_PENDING_INVOICE))
                return Constants.PAYMENT_STATUS_PENDING_INVOICE;
            if (sessionBookings.Any(x => x.PaymentStatus == Constants.PAYMENT_STATUS_PENDING_PAYMENT))
                return Constants.PAYMENT_STATUS_PENDING_PAYMENT;
            return Constants.PAYMENT_STATUS_PAID;

            //string coursePaymentStatus = string.Empty;
            //foreach (var sessionBooking in sessionBookings)
            //{
            //    if (PaymentStatusIsOverdue(sessionBooking.PaymentStatus))
            //        return Constants.PAYMENT_STATUS_OVERDUE_PAYMENT;
            //    if (PaymentStatusIsNotSet(coursePaymentStatus))
            //        coursePaymentStatus = sessionBooking.PaymentStatus;
            //    else if (PaymentStatusIsPendingInvoice(coursePaymentStatus) && 
            //             PaymentStatusIsPendingPayment(sessionBooking.PaymentStatus) &&
            //             PaymentStatusIsPaid(sessionBooking.PaymentStatus))
            //}

            //return coursePaymentStatus;
        }


        private static bool PaymentStatusIsNotSet(string paymentStatus)
        {
            return string.IsNullOrEmpty(paymentStatus);
        }

        private static bool PaymentStatusIsOverdue(string paymentStatus)
        {
            return paymentStatus == Constants.PAYMENT_STATUS_OVERDUE_PAYMENT;
        }

        private static bool PaymentStatusIsPendingInvoice(string paymentStatus)
        {
            return paymentStatus == Constants.PAYMENT_STATUS_PENDING_INVOICE;
        }

        private static bool PaymentStatusIsPendingPayment(string paymentStatus)
        {
            return paymentStatus == Constants.PAYMENT_STATUS_PENDING_PAYMENT;
        }

        private static bool PaymentStatusIsPaid(string paymentStatus)
        {
            return paymentStatus == Constants.PAYMENT_STATUS_PAID;
        }


        private static bool SessionBookingPaymentStatusIsOverdue(SingleSessionBookingData booking)
        {
            return booking.PaymentStatus == Constants.PAYMENT_STATUS_OVERDUE_PAYMENT;
        }

        private static bool CourseBookingPaymentStatusIsNotSet(string coursePaymentStatus)
        {
            return coursePaymentStatus == Constants.PAYMENT_STATUS_OVERDUE_PAYMENT;
        }

        private static bool CourseBookingPaymentStatusIsOverdue(string coursePaymentStatus)
        {
            return string.IsNullOrEmpty(coursePaymentStatus);
        }
    }
}
