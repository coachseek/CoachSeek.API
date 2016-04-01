using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class CourseSessionBookingCollection
    {
        protected Guid CourseBookingId { get; set; }
        protected RepeatedSessionData Course { get; set; }
        protected CustomerKeyData Customer { get; set; }
        protected IList<SingleSessionBooking> SessionBookings { get; set; }

        public IReadOnlyCollection<SingleSessionBookingData> Bookings 
        { 
            get
            {
                return SessionBookings.Select(x => (SingleSessionBookingData)x.ToData())
                                      .OrderBy(x => x.Session.Date)
                                      .ThenBy(x => x.Session.StartTime)
                                      .AsReadOnly();
            }  
        }

        public CourseSessionBookingCollection(Guid courseBookingId, RepeatedSessionData course, CustomerKeyData customer)
        {
            CourseBookingId = courseBookingId;
            Course = course;
            Customer = customer;
            SessionBookings = new List<SingleSessionBooking>();
        }


        public void Add(BookingAddCommand command)
        {
            foreach (var commandSession in command.Sessions)
            {
                if (Contains(commandSession))
                    continue;
                var bookingSession = CreateBookingSession(commandSession.Id);
                if (bookingSession.IsFull)
                    throw new SessionFullyBooked(bookingSession);
                var sessionBooking = CreateNewSessionBooking(bookingSession, command.IsOnlineBooking);
                SessionBookings.Add(sessionBooking);
            }
        }

        public void Add(CourseBookingData data)
        {
            Customer = data.Customer;
            foreach (var sessionBooking in data.SessionBookings)
            {
                var bookingSession = CreateBookingSession(sessionBooking.Session.Id);
                var booking = CreateSessionBooking(sessionBooking, bookingSession);
                SessionBookings.Add(booking);                
            }
        }

        private BookingSession CreateBookingSession(Guid sessionId)
        {
            var courseSession = Course.Sessions.Single(x => x.Id == sessionId);
            return new BookingSession(courseSession);
        }

        public bool Contains(SessionKeyCommand session)
        {
            return SessionBookings.Select(x => x.Session.Id).Contains(session.Id);
        }

        public bool Contains(Guid sessionId)
        {
            return SessionBookings.Select(x => x.Session.Id).Contains(sessionId);
        }

        public bool ContainsNot(Guid sessionId)
        {
            return !Contains(sessionId);
        }


        protected virtual SingleSessionBooking CreateNewSessionBooking(BookingSession session, bool isOnlineBooking)
        {
            if (isOnlineBooking)
                return new SingleSessionOnlineBooking(session, Customer, 0, CourseBookingId);
            return new SingleSessionBooking(session, Customer, 0, CourseBookingId);
        }

        protected virtual SingleSessionBooking CreateSessionBooking(BookingSession session)
        {
            return new SingleSessionBooking(session, Customer, 0, CourseBookingId);
        }

        protected virtual SingleSessionBooking CreateSessionBooking(SingleSessionBookingData data, BookingSession session)
        {
            if (IsOnlineBooking(data))
                return new SingleSessionOnlineBooking(data.Id, session.ToData(), Customer, data.PaymentStatus, data.HasAttended, CourseBookingId);
            return new SingleSessionBooking(data.Id, session.ToData(), Customer, data.PaymentStatus, data.HasAttended, CourseBookingId);
        }

        private bool IsOnlineBooking(SingleSessionBookingData data)
        {
            return data.IsOnlineBooking.HasValue && data.IsOnlineBooking.Value;
        }
    }
}
