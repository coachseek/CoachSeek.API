using System.Threading.Tasks;
using CoachSeek.Api.Results;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Filters;
using CoachSeek.Domain.Entities.Authentication;

namespace CoachSeek.Api.Attributes
{
    public class BasicAdminAuthenticationAttribute : BasicAuthenticationAttributeBase
    {
        protected override async Task AuthenticateUserAsync(string username, string password, HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var principal = await AuthenticateAsync(username, password, cancellationToken);
            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password.", request);
            else
                context.Principal = principal;
        }


        private async Task<IPrincipal> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
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