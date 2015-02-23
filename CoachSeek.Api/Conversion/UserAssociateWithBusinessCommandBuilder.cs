﻿using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class UserAssociateWithBusinessCommandBuilder
    {
        public static UserAssociateWithBusinessCommand BuildCommand(UserData user, Business2Data business)
        {
            return new UserAssociateWithBusinessCommand
            {
                UserId = user.Id,
                BusinessId = business.Id,
                BusinessName = business.Name,
            };
        }
    }
}