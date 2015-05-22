using System.Security.Principal;

namespace CoachSeek.Common
{
    public class CoachseekAdminIdentity : GenericIdentity
    {
        public CoachseekAdminIdentity()
            : base("admin", "none")
        { }
    }
}
