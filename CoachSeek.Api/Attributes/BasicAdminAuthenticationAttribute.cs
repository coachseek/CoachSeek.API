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
            return IsAuthenticated(username, password) ? CreateAdminPrincipal() : null;
        }

        private static bool IsAuthenticated(string username, string password)
        {
            if (AppSettings.AdminUserName == null || AppSettings.AdminPassword == null)
                return false;
            return username == AppSettings.AdminUserName && password == AppSettings.AdminPassword;
        }

        private static IPrincipal CreateAdminPrincipal()
        {
            return new GenericPrincipal(new CoachseekAdminIdentity(), new[] { "Admin" });
        }
    }
}