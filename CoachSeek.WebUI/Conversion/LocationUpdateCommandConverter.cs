using System;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class LocationUpdateCommandConverter
    {
        public static LocationUpdateCommand Convert(ApiLocationSaveCommand apiCommand)
        {
            return new LocationUpdateCommand
            {
                LocationId = apiCommand.Id.HasValue ? apiCommand.Id.Value : Guid.Empty,
                BusinessId = apiCommand.BusinessId.Value,
                LocationName = apiCommand.Name
            };
        }
    }
}