using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities.Booking
{
    public class BusinessBookings
    {
        private List<Booking> Bookings { get; set; }

        public BusinessBookings()
        {
            Bookings = new List<Booking>();
        }

        //public BusinessBookings(IEnumerable<CustomerData> customers) 
        //    : this()
        //{
        //    if (customers == null)
        //        return;

        //    foreach (var customer in customers)
        //        Append(customer);
        //}


        public BookingData GetById(Guid id)
        {
            var booking = Bookings.FirstOrDefault(x => x.Id == id);
            if (booking == null)
                return null;
            return booking.ToData();
        }

        public Booking Add(SessionKeyData session, CustomerKeyData customer)
        {
            var newBooking = new NewBooking(session, customer);
            ValidateAdd(newBooking);

            // Add the customer to the session
            // Add the session to the customer

            Bookings.Add(newBooking);

            return newBooking;
        }

        //public void Append(CustomerData customerData)
        //{
        //    // Data is not Validated. Eg. It comes from the database.
        //    Customers.Add(new Customer(customerData));
        //}

        //public void Update(CustomerData customerData)
        //{
        //    var customer = new Customer(customerData);
        //    ValidateUpdate(customer);
        //    ReplaceCustomerInCustomers(customer);
        //}

        //public IList<BookingData> ToData()
        //{
        //    return Customers.Select(customer => customer.ToData()).ToList();
        //}


        //private void ReplaceCustomerInCustomers(Customer customer)
        //{
        //    var updateCustomer = Customers.Single(x => x.Id == customer.Id);
        //    var updateIndex = Customers.IndexOf(updateCustomer);
        //    Customers[updateIndex] = customer;
        //}

        private void ValidateAdd(NewBooking newBooking)
        {
            var isExistingBooking = Bookings.Any(x => x.Session.Id == newBooking.Session.Id 
                                                   && x.Customer.Id == newBooking.Customer.Id);
            if (isExistingBooking)
                throw new DuplicateBooking();
        }

        //private void ValidateUpdate(Customer customer)
        //{
        //    var isExistingCustomer = Customers.Any(x => x.Id == customer.Id);
        //    if (!isExistingCustomer)
        //        throw new InvalidCustomer();

        ////    var existingCoach = Coaches.FirstOrDefault(x => x.Name.ToLower() == coach.Name.ToLower());
        ////    if (existingCoach != null && existingCoach.Id != coach.Id)
        ////        throw new DuplicateCoach();
        //}
    }
}