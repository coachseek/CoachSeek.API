using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomFieldGetByTypeAndKeyUseCase : IApplicationContextSetter
    {
        Task<IList<CustomFieldTemplateData>> GetCustomFieldsByTypeAndKeyAsync(string type, string key);
    }
}
