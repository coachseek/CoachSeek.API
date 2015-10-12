using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class SessionGetByIdUseCase : SessionBaseUseCase, ISessionGetByIdUseCase
    {
        public async Task<SessionData> GetSessionAsync(Guid id)
        { 
            var sessionOrCourse = await GetExistingSessionOrCourseAsync(id);
            if (sessionOrCourse.IsNotFound())
                return null;
            return sessionOrCourse.ToData();
        }
    }
}
