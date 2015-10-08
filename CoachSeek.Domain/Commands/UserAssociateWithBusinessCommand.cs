using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class UserAssociateWithBusinessCommand
    {
        public Guid UserId { get; private set; }
        public Guid BusinessId { get; private set; }
        public string BusinessName { get; private set; } // Debug
        public Role Role { get; private set; }


        public UserAssociateWithBusinessCommand(UserData user, BusinessData business, Role role)
            : this(user.Id, role, business.Id, business.Name)
        { }

        private UserAssociateWithBusinessCommand(Guid userId, Role role, Guid businessId, string businessName)
        {
            UserId = userId;
            Role = role;
            BusinessId = businessId;
            BusinessName = businessName;
        }
    }
}
