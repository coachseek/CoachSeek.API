//using System;
//using System.Collections.Generic;
//using System.Linq;
//using CoachSeek.Application.Contracts.Models;
//using CoachSeek.Application.Contracts.UseCases;
//using CoachSeek.Application.Contracts.UseCases.Factories;
//using CoachSeek.Common.Extensions;
//using CoachSeek.Data.Model;
//using CoachSeek.Domain.Commands;
//using CoachSeek.Domain.Entities;
//using CoachSeek.Domain.Exceptions;
//using CoachSeek.Domain.Repositories;

//namespace CoachSeek.Application.UseCases.Factories
//{
//    public class BookingAddUseCaseFactory : IBookingAddUseCaseFactory
//    {
//        private ApplicationContext Context { set; get; }
//        private Guid BusinessId { set; get; }
//        private IBusinessRepository BusinessRepository { set; get; }


//        public void Initialise(ApplicationContext context)
//        {
//            Context = context;

//            BusinessId = Context.BusinessContext.Business.Id;
//            BusinessRepository = Context.BusinessContext.BusinessRepository;
//        }
        

//        public IBookingAddUseCase CreateBookingUseCase(BookingAddCommand command)
//        {
//            Session sessionOrCourse = null;
//            //var sessionOrCourse = GetExistingSessionOrCourse(command.Session.Id);
//            if (sessionOrCourse.IsNotFound())
//                throw new InvalidSession();

//            var useCase = CreateSpecificBookingUseCase(sessionOrCourse);
//            InitialiseSpecificUseCase(useCase);

//            return useCase;
//        }

//        public IBookingAddUseCase CreateOnlineBookingUseCase(BookingAddCommand command)
//        {
//            Session sessionOrCourse = null;
//            //var sessionOrCourse = GetExistingSessionOrCourse(command.Session.Id);
//            if (sessionOrCourse.IsNotFound())
//                throw new InvalidSession();

//            var useCase = CreateSpecificOnlineBookingUseCase(sessionOrCourse);
//            InitialiseSpecificUseCase(useCase);

//            return useCase;
//        }


//        protected Session GetExistingSessionOrCourse(Guid sessionId)
//        {
//            var bookings = BusinessRepository.GetAllCustomerBookings(BusinessId);

//            // Is it a Session or a Course?
//            var session = BusinessRepository.GetSession(BusinessId, sessionId);
//            if (session.IsExisting())
//            {
//                AddBookingsToSession(session, bookings);

//                if (session.ParentId == null)
//                    return new StandaloneSession(session, LookupCoreData(session));

//                return new SessionInCourse(session, LookupCoreData(session));
//            }

//            var course = BusinessRepository.GetCourse(BusinessId, sessionId);
//            if (course.IsExisting())
//            {
//                AddBookingsToCourse(course, bookings);

//                return new RepeatedSession(course, LookupCoreData(course));
//            }

//            return null;
//        }


//        private void AddBookingsToSession(SingleSessionData session, IList<CustomerBookingData> bookings)
//        {
//            session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
//            session.Booking.BookingCount = session.Booking.Bookings.Count;
//        }

//        private void AddBookingsToCourse(RepeatedSessionData course, IList<CustomerBookingData> bookings)
//        {
//            course.Booking.Bookings = bookings.Where(x => x.SessionId == course.Id).ToList();
//            course.Booking.BookingCount = course.Booking.Bookings.Count;

//            foreach (var session in course.Sessions)
//            {
//                session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
//                session.Booking.BookingCount = session.Booking.Bookings.Count;
//            }
//        }

//        private CoreData LookupCoreData(SessionData data)
//        {
//            var location = BusinessRepository.GetLocation(BusinessId, data.Location.Id);
//            var coach = BusinessRepository.GetCoach(BusinessId, data.Coach.Id);
//            var service = BusinessRepository.GetService(BusinessId, data.Service.Id);

//            return new CoreData(location, coach, service);
//        }

//        private IBookingAddUseCase CreateSpecificBookingUseCase(Session sessionOrCourse)
//        {
//            if (sessionOrCourse is SingleSession)
//                return new SingleSessionBookingAddUseCase((SingleSession)sessionOrCourse);
//            if (sessionOrCourse is RepeatedSession)
//                return new CourseBookingAddUseCase((RepeatedSession)sessionOrCourse);

//            throw new InvalidOperationException("Invalid session type.");
//        }

//        private IBookingAddUseCase CreateSpecificOnlineBookingUseCase(Session sessionOrCourse)
//        {
//            if (sessionOrCourse is SingleSession)
//                return new SingleSessionOnlineBookingAddUseCase((SingleSession)sessionOrCourse);
//            if (sessionOrCourse is RepeatedSession)
//                return new CourseOnlineBookingAddUseCase((RepeatedSession)sessionOrCourse);

//            throw new InvalidOperationException("Invalid session type.");
//        }

//        private void InitialiseSpecificUseCase(IBookingAddUseCase useCase)
//        {
//            useCase.Initialise(Context);
//        }
//    }
//}
