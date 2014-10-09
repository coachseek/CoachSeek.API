using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.Contracts.UseCases
{
    public interface ILocationAddUseCase
    {
        LocationAddResponse AddLocation(LocationAddRequest request);
    }
}
