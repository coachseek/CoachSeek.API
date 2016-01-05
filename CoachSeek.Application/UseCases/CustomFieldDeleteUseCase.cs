using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;

namespace CoachSeek.Application.UseCases
{
    public class CustomFieldDeleteUseCase : BaseUseCase, ICustomFieldDeleteUseCase
    {
        public async Task<Response> DeleteCustomFieldAsync(string type, string key)
        {
            var templates = await BusinessRepository.GetCustomFieldTemplatesAsync(Business.Id, type, key);
            var template = templates.SingleOrDefault();
            if (template.IsNotFound())
                return new NotFoundResponse();
            await BusinessRepository.DeleteCustomFieldTemplateAsync(Business.Id, type, key);
            return new Response();
        }
    }
}
