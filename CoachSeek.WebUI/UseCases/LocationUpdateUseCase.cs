using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Models.Responses;

namespace CoachSeek.WebUI.UseCases
{
    public class LocationUpdateUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public LocationUpdateUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }

        
        public LocationUpdateResponse UpdateLocation(LocationUpdateRequest request)
        {
            if (request == null)
                return new NoLocationUpdateDataResponse();

            return null;
        }
    }
}