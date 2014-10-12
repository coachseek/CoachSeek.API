
namespace CoachSeek.Application.Contracts
{
    public enum ErrorCodes
    {
        // Business
        ErrorNoBusinessRegistrationData = 1010,
        ErrorBusinessAdminDuplicateEmail = 1020,
        ErrorInvalidBusiness = 1030,
        ErrorNoBusinessDomain = 1040,
        // Locations
        ErrorNoLocationAddData = 1110,
        ErrorDuplicateLocation = 1120,
        ErrorNoLocationUpdateData = 1130,
        ErrorInvalidLocation = 1140,
        // Coaches
        ErrorNoCoachAddData = 1210,
        ErrorDuplicateCoach = 1220,
        ErrorNoCoachUpdateData = 1230,
        ErrorInvalidCoach = 1240,
    }
}