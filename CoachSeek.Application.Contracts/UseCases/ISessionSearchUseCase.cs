using System;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionSearchUseCase : IApplicationContextSetter
    {
        Task<SessionSearchData> SearchForSessionsAsync(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null);
        Task<SessionSearchData> SearchForOnlineBookableSessionsAsync(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null);
    }
}
