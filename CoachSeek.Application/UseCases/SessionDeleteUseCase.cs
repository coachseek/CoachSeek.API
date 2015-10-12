using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class SessionDeleteUseCase : SessionBaseUseCase, ISessionDeleteUseCase
    {
        public async Task<IResponse> DeleteSessionAsync(Guid id)
        {
            var sessionOrCourse = await GetExistingSessionOrCourseAsync(id);
            if (sessionOrCourse.IsNotFound())
                return new NotFoundResponse();
            try
            {
                await TryDeleteSessionOrCourseAsync(sessionOrCourse);
                return new Response();
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private async Task TryDeleteSessionOrCourseAsync(Session sessionOrCourse)
        {
            ValidateDeleteSessionOrCourse(sessionOrCourse);
            await BusinessRepository.DeleteSessionAsync(Business.Id, sessionOrCourse.Id);
        }

        private void ValidateDeleteSessionOrCourse(Session sessionOrCourse)
        {
            if (sessionOrCourse.Booking.BookingCount > 0)
            {
                if (sessionOrCourse is SingleSession)
                    throw new SessionHasBookingsCannotDelete(sessionOrCourse.Id);
                if (sessionOrCourse is RepeatedSession)
                    throw new CourseHasBookingsCannotDelete(sessionOrCourse.Id);
            }
        }
    }
}