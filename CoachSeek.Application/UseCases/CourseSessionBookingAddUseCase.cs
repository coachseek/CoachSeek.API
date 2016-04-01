using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Services;

namespace CoachSeek.Application.UseCases
{
    public class CourseSessionBookingAddUseCase : BookingAddUseCase, ICourseSessionBookingAddUseCase
    {
        public RepeatedSessionData Course { set; protected get; }


        public IResponse AddBooking(BookingAddCommand command)
        {
            try
            {
                ValidateCommand(command);
                //var courseBooking = LookupCourseBooking(command.Customer);
                //if (courseBooking.IsExisting())
                //    courseBooking = AppendSessionBookingsToCourseBooking(courseBooking, command);
                //else
                var courseBooking = AddCourseBooking(command);
                PostProcessing(courseBooking);
                return new Response(courseBooking.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private void ValidateCommand(BookingAddCommand newBooking)
        {
            var errors = new ValidationException();

            ValidateSessions(newBooking.Sessions, errors);
            ValidateCustomer(newBooking.Customer.Id, errors);

            ValidateCommandAdditional(newBooking, errors);

            errors.ThrowIfErrors();
        }

        private CourseBooking AppendSessionBookingsToCourseBooking(CourseBooking originalCourseBooking, BookingAddCommand command)
        {
            var updatedCourseBooking = new CourseBooking(originalCourseBooking, Business);
            updatedCourseBooking.AppendSessionBookings(command);
            foreach (var sessionBooking in updatedCourseBooking.SessionBookings)
                if (originalCourseBooking.ContainsNot(sessionBooking))
                    BusinessRepository.AddSessionBooking(Business.Id, sessionBooking);
            return LookupCourseBooking(originalCourseBooking.Id);
        }

        private CourseBooking AddCourseBooking(BookingAddCommand command)
        {
            var customer = BusinessRepository.GetCustomer(Business.Id, command.Customer.Id);
            var courseBooking = CreateCourseBooking(command, customer);
            ValidateAddBooking(courseBooking);
            var data = BusinessRepository.AddCourseBooking(Business.Id, courseBooking);
            return CreateCourseBooking(data);
        }

        protected virtual CourseBooking CreateCourseBooking(BookingAddCommand command, CustomerData customer)
        {
            return new CourseBooking(command, Course, customer, Business);
        }

        protected virtual CourseBooking CreateCourseBooking(CourseBookingData data)
        {
            if (data == null)
                return null;
            return new CourseBooking(data, Course, Business);
        }

        private CourseBooking LookupCourseBooking(CustomerKeyCommand customer)
        {
            // Note: GetCourseBookings() still returns multiple course bookings because that used to be allowed.
            // Once we have merged all multiple course bookings we should remove the call to First(). 
            var courseBookings = BusinessRepository.GetCourseBookings(Business.Id, Course.Id, customer.Id);
            if (!courseBookings.Any())
                return null;
            return new CourseBooking(courseBookings.First(), Course, Business);
        }

        private CourseBooking LookupCourseBooking(Guid courseBookingId)
        {
            var courseBookingData = BusinessRepository.GetCourseBooking(Business.Id, courseBookingId);
            if (courseBookingData == null)
                return null;
            return new CourseBooking(courseBookingData, Course, Business);
        }

        private void ValidateSessions(IEnumerable<SessionKeyCommand> bookingSessions, ValidationException errors)
        {
            try
            {
                SessionsInCourseValidator.Validate(bookingSessions, Course);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected virtual void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
        }


        private void ValidateAddBooking(CourseBooking newBooking)
        {
            ValidateIsNewBooking(newBooking);
            ValidateSpacesAvailable(newBooking);

            ValidateAddBookingAdditional(newBooking);
        }

        private void ValidateAppendBooking(CourseBooking courseBooking)
        {
            ValidateSpacesAvailable(courseBooking);
        }

        private void ValidateIsNewBooking(CourseBooking newBooking)
        {
            var customerSessionBookings = BusinessRepository.GetAllCustomerSessionBookingsByCustomerId(Business.Id, newBooking.Customer.Id)
                                                            .ToList();

            foreach (var customerSessionBooking in customerSessionBookings)
                if (newBooking.SessionBookings.Select(x => x.Session.Id).Contains(customerSessionBooking.SessionId))
                    throw new CustomerAlreadyBookedOntoSession(newBooking.Customer.Id, customerSessionBooking.SessionId);
        }

        private void ValidateSpacesAvailable(CourseBooking newBooking)
        {
            foreach (var sessionBookings in newBooking.SessionBookings)
            {
                var session = Course.Sessions.First(x => x.Id == sessionBookings.Session.Id);
                if (session.Booking.BookingCount >= session.Booking.StudentCapacity)
                    throw new SessionFullyBooked(sessionBookings.Session.Id);
            }
        }

        protected virtual void ValidateAddBookingAdditional(CourseBooking newBooking)
        {
            // When overrides error they must throw a ValidationException.
        }

        protected virtual void PostProcessing(CourseBooking newBooking)
        {
            // Nothing to do for a coach-initiated booking.
        }
    }
}
