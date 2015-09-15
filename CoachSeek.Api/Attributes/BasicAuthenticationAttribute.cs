using CoachSeek.Api.Results;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
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
            var user = GetUserByUsername(username, request, cancellationToken);
            if (user == null)
                return null;
            var isAuthenticated = PasswordHasher.ValidatePassword(password, user.PasswordHash);
            if (!isAuthenticated)
                return null;
            if (!user.BusinessId.HasValue)
                return new NoBusinessPrincipal();
            var business = GetBusiness(user, request, cancellationToken);
            if (business == null)
                return null;
            var currency = GetCurrency(business, request);
            return CreatePrincipal(user, business, currency, cancellationToken);
        }
        
        private User GetUserByUsername(string username, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return CreateUserRepository(request).GetByUsername(username);
        }

        private BusinessData GetBusiness(User user, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return CreateBusinessRepository(request).GetBusiness(user.BusinessId.GetValueOrDefault());
        }

        private CurrencyData GetCurrency(BusinessData business, HttpRequestMessage request)
        {
            return CreateSupportedCurrencyRepository(request).GetByCode(business.Payment.Currency);
        }

        private IPrincipal CreatePrincipal(User user, BusinessData business, CurrencyData currency, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var identity = new CoachseekIdentity(ConvertToUserDetails(user),
                                                 ConvertToBusinessDetails(business),
                                                 ConvertToCurrencyDetails(currency));
            return new GenericPrincipal(identity, new[] { "BusinessAdmin" });
        }

        protected UserDetails ConvertToUserDetails(User user)
        {
            return new UserDetails(user.Id, user.UserName, user.FirstName, user.LastName);
        }

        protected BusinessDetails ConvertToBusinessDetails(BusinessData business)
        {
            return new BusinessDetails(business.Id, business.Name, business.Domain, business.AuthorisedUntil);
        }

        protected CurrencyDetails ConvertToCurrencyDetails(CurrencyData currency)
        {
            return new CurrencyDetails(currency.Code, currency.Symbol); 
        }
    }
}