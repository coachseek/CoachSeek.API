using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class SessionGetByIdUseCase : BaseUseCase, ISessionGetByIdUseCase
    {
        public SessionData GetSession(Guid id)
        {
            var session = BusinessRepository.GetSession(BusinessId, id);
            if (session == null)
                return null;

            session.Booking.Bookings = BusinessRepository.GetCustomerBookingsBySessionId(BusinessId, session.Id);

            return session;
        }
    }
}
