using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomFieldDeleteUseCase : IApplicationContextSetter
    {
        Task<Response> DeleteCustomFieldAsync(string type, string key);
    }
}
