using System.Threading.Tasks;
using CoachSeek.Api.Results;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.Authentication;
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
        protected override async Task AuthenticateUserAsync(string username, string password, HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var principal = await AuthenticateAsync(username, password, request, cancellationToken);
            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password.", request);
            else if (principal is NoBusinessPrincipal)
                context.ErrorResult = new AuthenticationFailureResult("User is not associated with a business.", request);
            else
                context.Principal = principal;
        }


        private IUserRepository CreateUserRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).UserRepository;
        }

        protected IBusinessRepository CreateBusinessRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).BusinessRepository;
        }

        protected ISupportedCurrencyRepository CreateSupportedCurrencyRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).SupportedCurrencyRepository;
        }

        private async Task<IPrincipal> AuthenticateAsync(string username, string password, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var user = GetUserByUsername(username, request, cancellationToken);
            if (user == null)
                return null;
            var isAuthenticated = PasswordHasher.ValidatePassword(password, user.PasswordHash);
            if (!isAuthenticated)
                return null;
            if (!user.BusinessId.HasValue)
                return new NoBusinessPrincipal();
            var business = await GetBusinessAsync(user, request, cancellationToken);
            if (business == null)
                return null;
            return CreatePrincipal(user, business, cancellationToken);
        }
        
        private User GetUserByUsername(string username, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return CreateUserRepository(request).GetByUsername(username);
        }

        private async Task<Business> GetBusinessAsync (User user, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var business = await CreateBusinessRepository(request).GetBusinessAsync(user.BusinessId.GetValueOrDefault());
            if (business.IsNotFound())
                return null;
            return new Business(business, CreateSupportedCurrencyRepository(request));
        }

        private IPrincipal CreatePrincipal(User user, Business business, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var identity = new CoachseekIdentity(user, business);
            return new GenericPrincipal(identity, new[] { "BusinessAdmin" });
        }
    }
}