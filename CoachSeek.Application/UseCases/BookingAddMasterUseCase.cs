using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Factories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BookingAddMasterUseCase : SessionBaseUseCase, IBookingAddMasterUseCase
    {
        private IBookingAddUseCaseFactory BookingAddUseCaseFactory { get; set; }


        public BookingAddMasterUseCase(IBookingAddUseCaseFactory bookingAddUseCaseFactory)
        {
            BookingAddUseCaseFactory = bookingAddUseCaseFactory;
        }


        public Response AddBooking(BookingAddCommand command)
        {
            try
            {
                var bookingAddUseCase = CreateBookingAddUseCase(command);
                return bookingAddUseCase.AddBooking(command);
            }
            catch (Exception ex)
            {
                if (ex is InvalidSession)
                    return new InvalidSessionErrorResponse("booking.session.id");
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        public Response AddOnlineBooking(BookingAddCommand command)
        {
            try
            {
                var bookingAddUseCase = CreateOnlineBookingAddUseCase(command);
                return bookingAddUseCase.AddBooking(command);
            }
            catch (Exception ex)
            {
                if (ex is InvalidSession)
                    return new InvalidSessionErrorResponse("booking.session.id");
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }


        private IBookingAddUseCase CreateBookingAddUseCase(BookingAddCommand command)
        {
            BookingAddUseCaseFactory.Initialise(Context);
            return BookingAddUseCaseFactory.CreateBookingUseCase(command);
        }

        private IBookingAddUseCase CreateOnlineBookingAddUseCase(BookingAddCommand command)
        {
            BookingAddUseCaseFactory.Initialise(Context);
            return BookingAddUseCaseFactory.CreateOnlineBookingUseCase(command);
        }


        //private void ValidateAdd(BookingAddCommand newBooking)
        //{
        //    var errors = new ValidationException();

        //    ValidateSession(newBooking.Session.Id, errors);
        //    ValidateCustomer(newBooking.Customer.Id, errors);

        //    errors.ThrowIfErrors();

        //    //ValidateBooking(newBooking, errors);
        //}


        //private void ValidateAdd(Booking newBooking)
        //{
        //    var errors = new ValidationException();

        //    ValidateSession(newBooking, errors);
        //    ValidateCustomer(newBooking, errors);

        //    errors.ThrowIfErrors();

        //    ValidateBooking(newBooking, errors);
        //}

        //private void ValidateSession(Guid sessionId, ValidationException errors)
        //{
        //    var sessionOrCourse = GetExistingSessionOrCourse(sessionId);
        //    if (sessionOrCourse.IsNotFound())
        //        errors.Add("This session does not exist.", "booking.session.id");
        //}

        //private void ValidateSession(Booking newBooking, ValidationException errors)
        //{
        //    var sessionOrCourse = GetExistingSessionOrCourse(newBooking.Session.Id);
        //    if (sessionOrCourse.IsNotFound())
        //        errors.Add("This session does not exist.", "booking.session.id");
        //}

        //private void ValidateCustomer(Guid customerId, ValidationException errors)
        //{
        //    var customer = BusinessRepository.GetCustomer(BusinessId, customerId);
        //    if (customer.IsNotFound())
        //        errors.Add("This customer does not exist.", "booking.customer.id");
        //}

        //private void ValidateCustomer(Booking newBooking, ValidationException errors)
        //{
        //    var customer = BusinessRepository.GetCustomer(BusinessId, newBooking.Customer.Id);
        //    if (customer.IsNotFound())
        //        errors.Add("This customer does not exist.", "booking.customer.id");
        //}

        //private void ValidateBooking(Booking newBooking, ValidationException errors)
        //{
        //    var bookings = BusinessRepository.GetAllBookings(BusinessId);
        //    var isExistingBooking = bookings.Any(x => x.Session.Id == newBooking.Session.Id
        //                                              && x.Customer.Id == newBooking.Customer.Id);
        //    if (isExistingBooking)
        //        throw new ValidationException("This customer is already in this session.");

        //    // TODO: Session or Course is full.
        //}
    }
}