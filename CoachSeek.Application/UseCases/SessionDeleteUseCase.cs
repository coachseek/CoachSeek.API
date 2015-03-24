using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class SessionDeleteUseCase : SessionBaseUseCase, ISessionDeleteUseCase
    {
        public Response DeleteSession(Guid id)
        {
            var sessionOrCourse = GetExistingSessionOrCourse(id);
            if (sessionOrCourse == null)
                return new NotFoundResponse();

            try
            {
                if (sessionOrCourse is SingleSession)
                    TryDeleteSession(id);
                else if (sessionOrCourse is RepeatedSession)
                    TryDeleteCourse(id);

                return new Response();
            }
            catch (Exception ex)
            {
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }


        private void TryDeleteSession(Guid sessionId)
        {
            var bookings = BusinessRepository.GetCustomerBookingsBySessionId(BusinessId, sessionId);
            if (bookings.Count > 0)
                throw new ValidationException("Cannot delete session as it has one or more bookings.");

            BusinessRepository.DeleteSession(BusinessId, sessionId);
        }

        private void TryDeleteCourse(Guid courseId)
        {
            // TODO: Determine whether there are any session or course bookings.

            BusinessRepository.DeleteCourse(BusinessId, courseId);
        }
    }
}