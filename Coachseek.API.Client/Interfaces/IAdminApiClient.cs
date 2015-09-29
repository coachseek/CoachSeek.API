using Coachseek.API.Client.Models;

namespace Coachseek.API.Client.Interfaces
{
    public interface IAdminApiClient
    {
        ApiResponse Get<TResponse>(string relativeUrl);
    }
}
