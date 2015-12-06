using System.Collections.Generic;
using System.Linq;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Services;

namespace CoachSeek.Domain.Entities
{
    public class CourseBooking : Booking, IPaymentStatusGetter
    {
        protected CourseSessionBookingCollection BookingCollection;

        public RepeatedSessionData Course { get; protected set; }
        public decimal BookingPrice { get; protected set; }
        public string PaymentStatus { get { return CalculateCourseBookingPaymentStatus(); } }

        public override bool IsOnlineBooking
        {
            get { return SessionBookings.First().IsOnlineBooking.GetValueOrDefault(); }
        }


        public IReadOnlyCollection<SingleSessionBookingData> SessionBookings
        {
            get { return BookingCollection.Bookings; }
        }


        public CourseBooking(BookingAddCommand command, RepeatedSessionData course, CustomerData customer)
            : base(customer.ToKeyData())
        {
            Course = course;

            BookingCollection = CreateCourseSessionBookingCollection();
            BookingCollection.Add(command);
            CalculateBookingPrice();
        }

        public CourseBooking(CourseBookingData data, RepeatedSessionData course)
            : base(data)
        {
            Course = course;

            BookingCollection = CreateCourseSessionBookingCollection();
            BookingCollection.Add(data);
            CalculateBookingPrice();
        }

        public CourseBooking(CourseBooking courseBooking)
            : base(courseBooking.ToData())
        {
            Course = courseBooking.Course;

            BookingCollection = CreateCourseSessionBookingCollection();
            BookingCollection.Add((CourseBookingData)courseBooking.ToData());
            CalculateBookingPrice();
        }

        private CourseSessionBookingCollection CreateCourseSessionBookingCollection()
        {
            return new CourseSessionBookingCollection(Id, Course, Customer);
        }

        public bool Contains(SingleSessionBookingData sessionBooking)
        {
            return BookingCollection.Contains(sessionBooking.Session.Id);
        }

        public bool ContainsNot(SingleSessionBookingData sessionBooking)
        {
            return BookingCollection.ContainsNot(sessionBooking.Session.Id);
        }

        public override BookingData ToData()
        {
            return new CourseBookingData
            {
                Id = Id,
                Course = Course.ToKeyData(),
                Customer = Customer,
                PaymentStatus = PaymentStatus,
                IsOnlineBooking = IsOnlineBooking,
                Price = BookingPrice,
                SessionBookings = SessionBookings.ToList()
            };
        }

        public void AppendSessionBookings(BookingAddCommand command)
        {
            BookingCollection.Add(command);
        }


        protected void CreateSessionBookings(CourseBookingData data)
        {
            BookingCollection.Add(data);
        }

        protected void CalculateBookingPrice()
        {
            BookingPrice = CourseBookingPriceCalculator.CalculatePrice(SessionBookings.Select(x => x.Session).AsReadOnly(), 
                                                                       Course.Sessions.AsReadOnly(), 
                                                                       Course.Pricing.CoursePrice);
        }

        private string CalculateCourseBookingPaymentStatus()
        {
            return CourseBookingPaymentStatusCalculator.CalculatePaymentStatus(SessionBookings);
        }
    }
}
