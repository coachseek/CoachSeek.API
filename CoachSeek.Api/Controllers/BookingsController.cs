﻿using System;
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
        [Authorize]
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
        [Authorize]
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

        private HttpResponseMessage UpdateBooking(ApiBookingSaveCommand booking)
        {
            return CreateWebErrorResponse(new BookingUpdateNotSupportedErrorResponse());
        }

        private HttpResponseMessage UpdateOnlineBooking(ApiBookingSaveCommand booking)
        {
            return CreateWebErrorResponse(new BookingUpdateNotSupportedErrorResponse());
        }

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