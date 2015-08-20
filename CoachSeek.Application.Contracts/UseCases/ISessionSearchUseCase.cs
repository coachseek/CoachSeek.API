using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionSearchUseCase : IApplicationContextSetter
    {
        SessionSearchData SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null);
        SessionSearchData SearchForOnlineBookableSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null);
    }
}
