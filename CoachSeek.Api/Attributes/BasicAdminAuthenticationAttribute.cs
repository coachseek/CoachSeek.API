using CoachSeek.Api.Results;
using CoachSeek.Common;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Filters;

namespace CoachSeek.Api.Attributes
{
    public class BasicAdminAuthenticationAttribute : BasicAuthenticationAttributeBase
    {
        protected override void AuthenticateUser(string username, string password, HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var principal = Authenticate(username, password, cancellationToken);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password.", request);
                return;
            }

            context.Principal = principal;
        }


        private IPrincipal Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var adminUsername = AppSettings.AdminUserName;
            var adminPassword = AppSettings.AdminPassword;
            if (adminUsername == null || adminPassword == null)
                return null;

            var isAuthenticated = username == adminUsername && password == adminPassword;
            if (!isAuthenticated)
                return null;

            cancellationToken.ThrowIfCancellationRequested();
            var identity = new CoachseekAdminIdentity();
            var principal = new GenericPrincipal(identity, new[] { "Admin" });
            return principal;
        }
    }
}