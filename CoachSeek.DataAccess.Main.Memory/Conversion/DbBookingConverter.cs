//using System;
//using AutoMapper;
//using CoachSeek.Data.Model;
//using CoachSeek.DataAccess.Main.Memory.Models;
//using CoachSeek.Domain.Entities;

//namespace CoachSeek.DataAccess.Main.Memory.Conversion
//{
//    public static class DbBookingConverter
//    {
//        public static DbBooking Convert(Guid businessId, Booking booking)
//        {
//            var data = booking.ToData();

//            var dbBooking = Mapper.Map<BookingData, DbBooking>(data);
//            dbBooking.BusinessId = businessId;

//            return dbBooking;
//        }
//    }
//}