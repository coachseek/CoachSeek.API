using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomFieldTemplatesGetByTypeAndKeyUseCase : IApplicationContextSetter
    {
        Task<IList<CustomFieldTemplateData>> GetCustomFieldTemplatesByTypeAndKeyAsync(string type, string key = null);
    }
}
