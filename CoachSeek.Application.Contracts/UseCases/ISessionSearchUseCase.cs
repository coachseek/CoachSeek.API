using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionSearchUseCase
    {
        Guid BusinessId { set; }

        IList<SingleSessionData> SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null);
    }
}
