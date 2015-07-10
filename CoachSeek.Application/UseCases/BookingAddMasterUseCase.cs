using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BookingAddMasterUseCase : SessionBaseUseCase, IBookingAddMasterUseCase
    {
        public IStandaloneSessionBookingAddUseCase StandaloneSessionBookingAddUseCase { get; private set; }
        public IStandaloneSessionOnlineBookingAddUseCase StandaloneSessionOnlineBookingAddUseCase { get; private set; }
        public ICourseSessionBookingAddUseCase CourseSessionBookingAddUseCase { get; private set; }
        public ICourseSessionOnlineBookingAddUseCase CourseSessionOnlineBookingAddUseCase { get; private set; }


        public BookingAddMasterUseCase(IStandaloneSessionBookingAddUseCase standaloneSessionBookingAddUseCase,
                                       IStandaloneSessionOnlineBookingAddUseCase standaloneSessionOnlineBookingAddUseCase,
                                       ICourseSessionBookingAddUseCase courseSessionBookingAddUseCase,
                                       ICourseSessionOnlineBookingAddUseCase courseSessionOnlineBookingAddUseCase)
        {
            StandaloneSessionBookingAddUseCase = standaloneSessionBookingAddUseCase;
            StandaloneSessionOnlineBookingAddUseCase = standaloneSessionOnlineBookingAddUseCase;
            CourseSessionBookingAddUseCase = courseSessionBookingAddUseCase;
            CourseSessionOnlineBookingAddUseCase = courseSessionOnlineBookingAddUseCase;
        }


        public Response AddBooking(BookingAddCommand command)
        {
            try
            {
                Validate(command);
                var useCase = SelectBookingAddUseCase(command);
                return useCase.AddBooking(command);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public Response AddOnlineBooking(BookingAddCommand command)
        {
            try
            {
                Validate(command);
                var useCase = SelectOnlineBookingAddUseCase(command);
                return useCase.AddBooking(command);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        private void Validate(BookingAddCommand command)
        {
            if (!command.Sessions.Any())
                throw new ValidationException("A booking must have at least one session.");
        }

        private IBookingAddUseCase SelectBookingAddUseCase(BookingAddCommand command)
        {
            var firstSession = GetSession(command.Sessions.First().Id);
            if (IsStandaloneSession(firstSession))
                return SetupStandaloneSessionBookingAddUseCase(firstSession);
            var course = GetCourse(firstSession.ParentId.Value);
            return SetupCourseSessionBookingAddUseCase(course);
        }

        private IBookingAddUseCase SelectOnlineBookingAddUseCase(BookingAddCommand command)
        {
            var firstSession = GetSession(command.Sessions.First().Id);
            if (IsStandaloneSession(firstSession))
                return SetupStandaloneSessionOnlineBookingAddUseCase(firstSession);
            var course = GetCourse(firstSession.ParentId.Value);
            return SetupCourseSessionOnlineBookingAddUseCase(course);
        }

        private static Response HandleException(Exception ex)
        {
            if (ex is InvalidSession)
                return new InvalidSessionErrorResponse();
            if (ex is ValidationException)
                return new ErrorResponse((ValidationException)ex);

            throw ex;
        }

        private SingleSessionData GetSession(Guid sessionId)
        {
            var session = BusinessRepository.GetSession(Business.Id, sessionId);
            if (session.IsNotFound())
                throw new InvalidSession();
            var bookings = BusinessRepository.GetAllCustomerBookings(Business.Id);
            AddBookingsToSession(session, bookings);
            return session;
        }

        private void AddBookingsToSession(SingleSessionData session, IList<CustomerBookingData> bookings)
        {
            session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
        }

        private bool IsStandaloneSession(SingleSessionData session)
        {
            return !session.ParentId.HasValue;
        }

        private IStandaloneSessionBookingAddUseCase SetupStandaloneSessionBookingAddUseCase(SingleSessionData session)
        {
            StandaloneSessionBookingAddUseCase.Session = session;
            StandaloneSessionBookingAddUseCase.Initialise(Context);

            return StandaloneSessionBookingAddUseCase;
        }

        private IStandaloneSessionBookingAddUseCase SetupStandaloneSessionOnlineBookingAddUseCase(SingleSessionData session)
        {
            StandaloneSessionOnlineBookingAddUseCase.Session = session;
            StandaloneSessionOnlineBookingAddUseCase.Initialise(Context);

            return StandaloneSessionOnlineBookingAddUseCase;
        }

        private ICourseSessionBookingAddUseCase SetupCourseSessionBookingAddUseCase(RepeatedSessionData course)
        {
            CourseSessionBookingAddUseCase.Course = course;
            CourseSessionBookingAddUseCase.Initialise(Context);

            return CourseSessionBookingAddUseCase;
        }

        private ICourseSessionOnlineBookingAddUseCase SetupCourseSessionOnlineBookingAddUseCase(RepeatedSessionData course)
        {
            CourseSessionOnlineBookingAddUseCase.Course = course;
            CourseSessionOnlineBookingAddUseCase.Initialise(Context);

            return CourseSessionOnlineBookingAddUseCase;
        }

        private RepeatedSessionData GetCourse(Guid courseId)
        {
            var course = BusinessRepository.GetCourse(Business.Id, courseId);
            var bookings = BusinessRepository.GetAllCustomerBookings(Business.Id);
            AddBookingsToCourse(course, bookings);
            return course;
        }

        private void AddBookingsToCourse(RepeatedSessionData course, IList<CustomerBookingData> bookings)
        {
            course.Booking.Bookings = bookings.Where(x => x.SessionId == course.Id).ToList();

            foreach (var session in course.Sessions)
                session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
        }
    }
}