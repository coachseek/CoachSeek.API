
namespace CoachSeek.WebUI.Models
{
    public enum ErrorCodes
    {
        // Business
        ErrorNoBusinessRegistrationData = 1010,
        ErrorBusinessAdminDuplicateEmail = 1020,
        ErrorInvalidBusiness = 1030,
        // BusinessLocations
        ErrorNoLocationAddData = 1110,
        ErrorDuplicateLocation = 1120
    }
}