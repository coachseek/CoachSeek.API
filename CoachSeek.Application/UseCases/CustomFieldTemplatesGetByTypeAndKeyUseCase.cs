using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomFieldTemplatesGetByTypeAndKeyUseCase : BaseUseCase, ICustomFieldTemplatesGetByTypeAndKeyUseCase
    {
        public async Task<IList<CustomFieldTemplateData>> GetCustomFieldTemplatesByTypeAndKeyAsync(string type, string key = null)
        {
            return await BusinessRepository.GetCustomFieldTemplatesAsync(Business.Id, type, key);
        }
    }
}
