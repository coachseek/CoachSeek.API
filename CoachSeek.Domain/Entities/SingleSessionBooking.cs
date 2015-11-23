﻿using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class SingleSessionBooking : Booking
    {
        public Guid? ParentId { get; set; }
        public bool? HasAttended { get; set; } // Null is 'not marked off', true is 'attended', false is 'absent'.
        public BookingSession Session { get; private set; }
        public Date Date { get { return Session.Date; } }
        public TimeOfDay StartTime { get { return Session.StartTime; } }

        public SingleSessionBooking(BookingSession session,
                                    CustomerKeyData customer, 
                                    Guid? parentId = null,
                                    string paymentStatus = Constants.PAYMENT_STATUS_PENDING_INVOICE)
            : base(customer)
        {
            ParentId = parentId;
            Session = session;
            HasAttended = null;
            PaymentStatus = paymentStatus;
        }

        // Data parameters denote that it's data from inside the application (ie. database).
        public SingleSessionBooking(Guid id, 
                                    BookingSessionData session,
                                    CustomerKeyData customer, 
                                    string paymentStatus, 
                                    bool? hasAttended, 
                                    Guid? parentId = null)
            : base(id, paymentStatus, customer)
        {
            ParentId = parentId;
            Session = new BookingSession(session);
            HasAttended = hasAttended;
            PaymentStatus = paymentStatus;
        }

        public SingleSessionBooking(SingleSessionBookingData data)
            : this(data.Id, data.Session, data.Customer, data.PaymentStatus, data.HasAttended, data.ParentId)
        { }

        public override BookingData ToData()
        {
            return new SingleSessionBookingData
            {
                Id = Id,
                Session = Session.ToData(),
                Customer = Customer,
                PaymentStatus = PaymentStatus,
                HasAttended = HasAttended,
                ParentId = ParentId
            };
        }
    }
}
