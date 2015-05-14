using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CourseBookingAddUseCase : SessionBaseUseCase, IBookingAddUseCase
    {
        protected RepeatedSession Course { get; set; }


        public CourseBookingAddUseCase(RepeatedSession existingCourse)
        {
            Course = existingCourse;
        }


        public Response AddBooking(BookingAddCommand command)
        {
            try
            {
                ValidateCommand(command);
                var newBooking = new CourseBooking(command, Course.ToData());
                ValidateAddBooking(newBooking);
                var data = BusinessRepository.AddCourseBooking(BusinessId, newBooking);
                return new Response(data);
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


        private void ValidateCommand(BookingAddCommand newBooking)
        {
            var errors = new ValidationException();

            ValidateCustomer(newBooking.Customer.Id, errors);

            ValidateCommandAdditional(newBooking, errors);

            errors.ThrowIfErrors();
        }

        protected virtual void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
        }


        private void ValidateAddBooking(CourseBooking newBooking)
        {
            ValidateIsNewBooking(newBooking);
            ValidateSpacesAvailable();

            ValidateAddBookingAdditional(newBooking);
        }

        private void ValidateIsNewBooking(CourseBooking newBooking)
        {
            var bookings = BusinessRepository.GetAllCustomerBookings(BusinessId);
            var isExistingBooking = bookings.Any(x => x.SessionId == newBooking.Course.Id
                                              && x.Customer.Id == newBooking.Customer.Id);
            if (isExistingBooking)
                throw new ValidationException("This customer is already booked for this course.");
        }

        private void ValidateSpacesAvailable()
        {
            if (Course.Booking.BookingCount >= Course.Booking.StudentCapacity)
                throw new ValidationException("This course is already fully booked.");

            foreach(var session in Course.Sessions)
                if (session.Booking.BookingCount >= session.Booking.StudentCapacity)
                    throw new ValidationException("This course cannot be booked as it has sessions that are fully booked.");
        }

        protected virtual void ValidateAddBookingAdditional(CourseBooking newBooking)
        {
            // When overrides error they must throw a ValidationException.
        }


        private void ValidateCustomer(Guid customerId, ValidationException errors)
        {
            var customer = BusinessRepository.GetCustomer(BusinessId, customerId);
            if (customer.IsNotFound())
                errors.Add("This customer does not exist.", "booking.customer.id");
        }
    }
}