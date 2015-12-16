using System;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Booking;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Api.Controllers
{
    public class BookingsController : BaseController
    {
        public IBookingGetByIdUseCase BookingGetByIdUseCase { get; set; }
        public IBookingAddMasterUseCase BookingAddMasterUseCase { get; set; }
        public IBookingDeleteUseCase BookingDeleteUseCase { get; set; }
        public IBookingUseCaseExecutor BookingUseCaseExecutor { get; set; }


        public BookingsController()
        { }

        public BookingsController(IBookingGetByIdUseCase bookingGetByIdUseCase,
                                  IBookingAddMasterUseCase bookingAddMasterUseCase,
                                  IBookingDeleteUseCase bookingDeleteUseCase,
                                  IBookingUseCaseExecutor bookingUseCaseExecutor)
        {
            BookingGetByIdUseCase = bookingGetByIdUseCase;
            BookingAddMasterUseCase = bookingAddMasterUseCase;
            BookingDeleteUseCase = bookingDeleteUseCase;
            BookingUseCaseExecutor = bookingUseCaseExecutor;
        }


        // GET: Bookings/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            var response = GetBooking(id);
            return CreateGetWebResponse(response);
        }

        // POST: Bookings
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBookingSaveCommand booking)
        {
            return booking.IsNew() ? AddBooking(booking) : UpdateBooking(booking);
        }

        // POST: OnlineBooking/Bookings
        [Route("OnlineBooking/Bookings")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage PostOnlineBooking(ApiBookingSaveCommand booking)
        {
            return booking.IsNew() ? AddOnlineBooking(booking) : UpdateOnlineBooking(booking);
        }

        // POST: Bookings/{booking_id}
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        public HttpResponseMessage Post(Guid id, [FromBody] dynamic apiCommand)
        {
            apiCommand.BookingId = id;
            ICommand command = DomainCommandConverter.Convert(apiCommand);
            var response = BookingUseCaseExecutor.ExecuteFor(command, Context);
            return CreatePostWebResponse(response);
        }

        // DELETE: Bookings/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public HttpResponseMessage Delete(Guid id)
        {
            var response = DeleteBooking(id);
            return CreateDeleteWebResponse(response);
        }

        //// DELETE: Bookings?customerId=D65BA9FE-D2C9-4C05-8E1A-326B1476DE08&courseId=028CE759-1643-4DD3-9A0D-65C4B80D0B48
        //[BasicAuthentication]
        //[BusinessAuthorize(Role.BusinessAdmin)]
        //public HttpResponseMessage Delete(Guid customerId, Guid courseId)
        //{
        //    var response = DeleteBooking(customerId);
        //    return CreateDeleteWebResponse(response);
        //}


        private HttpResponseMessage AddBooking(ApiBookingSaveCommand booking)
        {
            var command = BookingAddCommandConverter.Convert(booking);
            var response = AddBooking(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage AddOnlineBooking(ApiBookingSaveCommand booking)
        {
            var command = BookingAddCommandConverter.Convert(booking);
            var response = AddOnlineBooking(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateBooking(ApiBookingSaveCommand booking)
        {
            return CreateWebErrorResponse(new BookingUpdateNotSupported());
        }

        private HttpResponseMessage UpdateOnlineBooking(ApiBookingSaveCommand booking)
        {
            return CreateWebErrorResponse(new BookingUpdateNotSupported());
        }

        private BookingData GetBooking(Guid id)
        {
            BookingGetByIdUseCase.Initialise(Context);
            return BookingGetByIdUseCase.GetBooking(id);
        }

        private IResponse AddBooking(BookingAddCommand command)
        {
            BookingAddMasterUseCase.Initialise(Context);
            return BookingAddMasterUseCase.AddBooking(command);
        }

        private IResponse AddOnlineBooking(BookingAddCommand command)
        {
            command.IsOnlineBooking = true;
            BookingAddMasterUseCase.Initialise(Context);
            return BookingAddMasterUseCase.AddOnlineBooking(command);
        }

        private IResponse DeleteBooking(Guid id)
        {
            BookingDeleteUseCase.Initialise(Context);
            return BookingDeleteUseCase.DeleteBooking(id);
        }
    }
}