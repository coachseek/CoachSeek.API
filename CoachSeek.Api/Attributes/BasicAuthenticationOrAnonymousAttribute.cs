using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.Authentication;

namespace CoachSeek.Api.Attributes
{
    public class BasicAuthenticationOrAnonymousAttribute : BasicAuthenticationAttribute
    {
        public async override Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // When the Principal is set this indicates Success
            // when the ErrorResult is set this indicates an Error
            // when we just return then the authentication is skipped (but the Authorize attribute will create 401 - Unauthorized error)

            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization.IsNotFound())
            {
                await AuthoriseForAnonymousBusinessUser(context);
                return;
            }

            await base.AuthenticateAsync(context, cancellationToken);
        }


        private async Task AuthoriseForAnonymousBusinessUser(HttpAuthenticationContext context)
        {
            var request = context.Request;
            if (!request.Headers.Contains(Constants.BUSINESS_DOMAIN))
                return;
            var business = await LookupBusinessFromDomainAsync(request);
            if (business.IsNotFound())
                return;
            context.Principal = CreateAnonymousPrincipal(business);
        }

        private async Task<Business> LookupBusinessFromDomainAsync(HttpRequestMessage request)
        {
            var domain = request.Headers.GetValues(Constants.BUSINESS_DOMAIN).ToList().First();
            var business = await CreateBusinessRepository(request).GetBusinessAsync(domain);
            if (business.IsNotFound())
                return null;
            return new Business(business, CreateSupportedCurrencyRepository(request));
        }

        private GenericPrincipal CreateAnonymousPrincipal(Business business)
        {
            var identity = new CoachseekAnonymousIdentity(business);
            return new GenericPrincipal(identity, new[] { "Anonymous" });
        }
    }
}