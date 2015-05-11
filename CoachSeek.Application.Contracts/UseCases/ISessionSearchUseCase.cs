using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionSearchUseCase : IApplicationContextSetter
    {
        SessionSearchData SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null);
        SessionSearchData SearchForOnlineBookableSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null);

        // Deprecated. TODO: Remove
        IList<SingleSessionData> SearchForSessionsOld(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null);
    }
}
