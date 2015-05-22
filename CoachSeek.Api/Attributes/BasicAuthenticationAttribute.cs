using CoachSeek.Api.Results;
using CoachSeek.Common;
using CoachSeek.Domain.Repositories;
using CoachSeek.Domain.Services;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Filters;

namespace CoachSeek.Api.Attributes
{
    public class BasicAuthenticationAttribute : BasicAuthenticationAttributeBase
    {
        protected override void AuthenticateUser(string username, string password, HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var userRepository = CreateUserRepository(request);
            var principal = Authenticate(username, password, userRepository, cancellationToken);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password.", request);
                return;
            }

            if (principal is NoBusinessPrincipal)
            {
                context.ErrorResult = new AuthenticationFailureResult("User is not associated with a business.", request);
                return;
            }

            context.Principal = principal;
        }


        private IUserRepository CreateUserRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).UserRepository;
        }

        private IPrincipal Authenticate(string username, string password, IUserRepository userRepository, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = userRepository.GetByUsername(username);
            if (user == null)
                return null;

            var isAuthenticated = PasswordHasher.ValidatePassword(password, user.PasswordHash);
            if (!isAuthenticated)
                return null;

            if (!user.BusinessId.HasValue)
                return new NoBusinessPrincipal();

            cancellationToken.ThrowIfCancellationRequested();
            var identity = new CoachseekIdentity(username, "Basic", user.BusinessId.Value, user.BusinessName);
            var principal = new GenericPrincipal(identity, new[] { "BusinessAdmin" });
            return principal;
        }
    }
}