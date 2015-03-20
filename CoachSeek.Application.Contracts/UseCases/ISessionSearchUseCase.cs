using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionSearchUseCase : IBusinessRepositorySetter
    {
        IList<SingleSessionData> SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null);
    }
}
