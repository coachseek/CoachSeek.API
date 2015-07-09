﻿using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    // Aggregate Root for booking process.
    public abstract class Booking
    {
        public Guid Id { get; private set; }
        public string PaymentStatus { get; set; }
        public CustomerKeyData Customer { get; set; }


        // Command parameters denote that it's data from outside the application (ie. user input).
        protected Booking(CustomerKeyCommand customer)
        {
            Id = Guid.NewGuid();
            PaymentStatus = Constants.PAYMENT_STATUS_PENDING_INVOICE;
            Customer = new CustomerKeyData { Id = customer.Id };
        }

        // Data parameters denote that it's data from inside the application (ie. database).
        protected Booking(Guid id, string paymentStatus, CustomerKeyData customer)
        {
            Id = id;
            PaymentStatus = paymentStatus;
            Customer = customer;
        }
    }
}
