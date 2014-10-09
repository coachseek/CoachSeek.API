using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.Contracts.UseCases
{
    public interface ILocationUpdateUseCase
    {
        LocationUpdateResponse UpdateLocation(LocationUpdateRequest request);
    }
}
