using System.Security.Principal;

namespace CoachSeek.Common
{
    public class NoBusinessPrincipal : IPrincipal
    {
        public IIdentity Identity
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsInRole(string role)
        {
            throw new System.NotImplementedException();
        }
    }
}
