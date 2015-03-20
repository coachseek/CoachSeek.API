using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.DataAccess.Main.Memory.Conversion;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        // Spy behaviour is included
        public bool WasSaveNewCalled;
        public bool WasSaveExistingCalled; 

        public static List<DbBooking> Bookings { get; private set; }


        static InMemoryBookingRepository()
        {
            Bookings = new List<DbBooking>();
        }

        public void Clear()
        {
            Bookings.Clear();
        }



        public Booking SaveNew(Guid businessId, Booking booking)
        {
            WasSaveNewCalled = true;

            var dbBooking = DbBookingConverter.Convert(businessId, booking);
            Bookings.Add(dbBooking);

            return booking;
        }

        public Booking SaveExisting(Guid businessId, Booking booking)
        {
            WasSaveExistingCalled = true;

            var dbBooking = DbBookingConverter.Convert(businessId, booking);
            var existingBooking = Bookings.Single(x => x.Id == dbBooking.Id);
            var existingIndex = Bookings.IndexOf(existingBooking);
            Bookings[existingIndex] = dbBooking;
            var updateBooking = Bookings.Single(x => x.Id == dbBooking.Id);
            return CreateBooking(updateBooking);
        }

        public Booking Get(Guid businessId, Guid id)
        {
            var dbBooking = Bookings.FirstOrDefault(x => x.BusinessId == businessId && x.Id == id);
            return CreateBooking(dbBooking);
        }


        private Booking CreateBooking(DbBooking dbBooking)
        {
            if (dbBooking == null)
                return null;

            // For now.
            return null;

            //return new Booking(dbBooking.Id, dbBooking.Session, dbBooking.Customer);
        }


        //private static BusinessAdminData CreateBusinessAdmin(DbBusinessAdmin dbAdmin)
        //{
        //    return new BusinessAdmin(dbAdmin.Id, dbAdmin.Email,
        //                             dbAdmin.FirstName, dbAdmin.LastName,
        //                             dbAdmin.Username, dbAdmin.PasswordHash).ToData();
        //}

        //// Only used for tests to add a business while bypassing the validation that occurs using Save.
        //public Business Add(Business business)
        //{
        //    var dbBusiness = DbBusinessConverter.Convert(business);

        //    Businesses.Add(dbBusiness);
        //    return business;
        //}
    }
}