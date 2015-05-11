using System;
using System.Security.Principal;

namespace CoachSeek.Common
{
    public class CoachseekIdentity : GenericIdentity
    {
        public Guid BusinessId { get; protected set; }
        public string BusinessName { get; protected set; }

        public CoachseekIdentity(string name, string type, Guid businessId, string businessName)
            : base(name, type)
        {
            BusinessId = businessId;
            BusinessName = businessName;
        }
    }
}
