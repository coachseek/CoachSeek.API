using CoachSeek.Api.Results;
using CoachSeek.Common;
using CoachSeek.Data.Model;
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
            var principal = Authenticate(username, password, request, cancellationToken);

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

        protected IBusinessRepository CreateBusinessRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).BusinessRepository;
        }

        protected ISupportedCurrencyRepository CreateSupportedCurrencyRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).SupportedCurrencyRepository;
        }

        private IPrincipal Authenticate(string username, string password, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = CreateUserRepository(request).GetByUsername(username);
            if (user == null)
                return null;

            var isAuthenticated = PasswordHasher.ValidatePassword(password, user.PasswordHash);
            if (!isAuthenticated)
                return null;

            if (!user.BusinessId.HasValue)
                return new NoBusinessPrincipal();

            cancellationToken.ThrowIfCancellationRequested();
            var business = CreateBusinessRepository(request).GetBusiness(user.BusinessId.Value);
            if (business == null)
                return null;

            var currency = CreateSupportedCurrencyRepository(request).GetByCode(business.Currency);

            cancellationToken.ThrowIfCancellationRequested();
            var identity = new CoachseekIdentity(username, "Basic", ConvertToBusinessDetails(business), ConvertToCurrencyDetails(currency));
            var principal = new GenericPrincipal(identity, new[] { "BusinessAdmin" });
            return principal;
        }

        protected BusinessDetails ConvertToBusinessDetails(BusinessData business)
        {
            return new BusinessDetails(business.Id, business.Name, business.Domain); 
        }

        protected CurrencyDetails ConvertToCurrencyDetails(CurrencyData currency)
        {
            return new CurrencyDetails(currency.Code, currency.Symbol); 
        }
    }
}