using System.Security.Principal;

namespace CoachSeek.Domain.Entities.Authentication
{
    public class CoachseekAdminIdentity : GenericIdentity
    {
        public CoachseekAdminIdentity()
            : base("admin", "none")
        { }
    }
}
