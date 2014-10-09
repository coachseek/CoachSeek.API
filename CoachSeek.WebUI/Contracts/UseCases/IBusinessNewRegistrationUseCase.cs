using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.Contracts.UseCases
{
    public interface IBusinessNewRegistrationUseCase
    {
        BusinessRegistrationResponse RegisterNewBusiness(BusinessRegistrationRequest registration);
    }
}
