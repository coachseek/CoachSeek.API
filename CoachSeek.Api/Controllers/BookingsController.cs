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
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Api.Controllers
{
    public class BookingsController : BaseController
    {
        public IBookingGetByIdUseCase BookingGetByIdUseCase { get; set; }
        public IBookingAddMasterUseCase BookingAddMasterUseCase { get; set; }
        public IBookingDeleteUseCase BookingDeleteUseCase { get; set; }
        public IUseCaseExecutor UseCaseExecutor { get; set; }

        public BookingsController()
        { }

        public BookingsController(IBookingGetByIdUseCase bookingGetByIdUseCase,
                                  IBookingAddMasterUseCase bookingAddMasterUseCase,
                                  IBookingDeleteUseCase bookingDeleteUseCase,
                                  IUseCaseExecutor useCaseExecutor)
        {
            BookingGetByIdUseCase = bookingGetByIdUseCase;
            BookingAddMasterUseCase = bookingAddMasterUseCase;
            BookingDeleteUseCase = bookingDeleteUseCase;
            UseCaseExecutor = useCaseExecutor;
        }


        // GET: Bookings/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            var response = GetBooking(id);
            return CreateGetWebResponse(response);
        }

        // POST: Bookings
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

        // POST: OnlineBooking/Bookings
        [Route("OnlineBooking/Bookings")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage PostOnlineBooking(ApiBookingSaveCommand booking)
        {
            if (booking.IsNew())
                return AddOnlineBooking(booking);

            //return UpdateBooking(booking);
            return null;
        }


        // POST: Bookings/{booking_id}
        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        public HttpResponseMessage Post(Guid id, [FromBody] dynamic apiCommand)
        {
            apiCommand.BookingId = id;
            ICommand command = DomainCommandConverter.Convert(apiCommand);
            var response = UseCaseExecutor.ExecuteFor(command, Context);
            return CreateGetWebResponse(response);
        }
        

        // DELETE: Bookings/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
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

        private HttpResponseMessage AddOnlineBooking(ApiBookingSaveCommand booking)
        {
            var command = BookingAddCommandConverter.Convert(booking);
            var response = AddOnlineBooking(command);
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
            BookingGetByIdUseCase.Initialise(Context);
            return BookingGetByIdUseCase.GetBooking(id);
        }

        private Response AddBooking(BookingAddCommand command)
        {
            BookingAddMasterUseCase.Initialise(Context);
            return BookingAddMasterUseCase.AddBooking(command);
        }

        private Response AddOnlineBooking(BookingAddCommand command)
        {
            BookingAddMasterUseCase.Initialise(Context);
            return BookingAddMasterUseCase.AddOnlineBooking(command);
        }

        private Response DeleteBooking(Guid id)
        {
            BookingDeleteUseCase.Initialise(Context);
            return BookingDeleteUseCase.DeleteBooking(id);
        }
    }
}