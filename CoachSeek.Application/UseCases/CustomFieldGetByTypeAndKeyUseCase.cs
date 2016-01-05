using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomFieldGetByTypeAndKeyUseCase : BaseUseCase, ICustomFieldGetByTypeAndKeyUseCase
    {
        public async Task<IList<CustomFieldTemplateData>> GetCustomFieldsByTypeAndKeyAsync(string type, string key)
        {
            return await BusinessRepository.GetCustomFieldTemplatesAsync(Business.Id, type, key);
        }
    }
}
