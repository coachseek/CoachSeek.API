using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Booking;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Controllers
{
    public class BookingsController : BaseController
    {
        public IBookingGetByIdUseCase BookingGetByIdUseCase { get; set; }
        public IBookingAddUseCase BookingAddUseCase { get; set; }
        public IBookingDeleteUseCase BookingDeleteUseCase { get; set; }

        public BookingsController()
        { }

        public BookingsController(IBookingGetByIdUseCase bookingGetByIdUseCase,
                                  IBookingAddUseCase bookingAddUseCase,
                                  IBookingDeleteUseCase bookingDeleteUseCase)
        {
            BookingGetByIdUseCase = bookingGetByIdUseCase;
            BookingAddUseCase = bookingAddUseCase;
            BookingDeleteUseCase = bookingDeleteUseCase;
        }


        // GET: api/Bookings/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            var response = GetBooking(id);
            return CreateGetWebResponse(response);
        }

        // POST: api/Bookings
        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBookingSaveCommand booking)
        {
            
            if (booking.IsNew())
                return AddBooking(booking);

            //return UpdateBooking(booking);
            return null;
        }

        // DELETE: api/Bookings/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Delete(Guid id)
        {
            var response = DeleteBooking(id);
            return CreateDeleteWebResponse(response);
        }


        private HttpResponseMessage AddBooking(ApiBookingSaveCommand booking)
        {
            var command = BookingAddCommandConverter.Convert(booking);
            var response = AddBooking(command);
            return CreatePostWebResponse(response);
        }

        //private HttpResponseMessage UpdateBooking(ApiBookingSaveCommand booking)
        //{
        //    var command = CoachUpdateCommandConverter.Convert(BusinessId, booking);
        //    var response = CoachUpdateUseCase.UpdateCoach(command);
        //    return CreatePostWebResponse(response);
        //}

        private BookingData GetBooking(Guid id)
        {
            BookingGetByIdUseCase.Initialise(BusinessRepository, BusinessId);
            return BookingGetByIdUseCase.GetBooking(id);
        }

        private Response AddBooking(BookingAddCommand command)
        {
            BookingAddUseCase.Initialise(BusinessRepository, BusinessId);
            return BookingAddUseCase.AddBooking(command);
        }

        private Response DeleteBooking(Guid id)
        {
            BookingDeleteUseCase.Initialise(BusinessRepository, BusinessId);
            return BookingDeleteUseCase.DeleteBooking(id);
        }
    }
}
