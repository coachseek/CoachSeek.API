using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace CoachSeek.Api.Attributes
{
    public class BasicAuthenticationOrAnonymousAttribute : BasicAuthenticationAttribute
    {
        private const string BUSINESS_DOMAIN = "Business-Domain";

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
            if (!request.Headers.Contains(BUSINESS_DOMAIN))
                return;
            var business = LookupBusinessFromDomain(request);
            if (business.IsNotFound())
                return;
            context.Principal = CreateAnonymousPrincipal(business);
        }

        private BusinessData LookupBusinessFromDomain(HttpRequestMessage request)
        {
            var domain = request.Headers.GetValues(BUSINESS_DOMAIN).ToList().First();
            var businessRepository = CreateBusinessRepository(request);
            return businessRepository.GetBusiness(domain);
        }

        private GenericPrincipal CreateAnonymousPrincipal(BusinessData business)
        {
            var identity = new CoachseekAnonymousIdentity(business.Id, business.Name);
            return new GenericPrincipal(identity, new[] { "Anonymous" });
        }

        private IBusinessRepository CreateBusinessRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).BusinessRepository;
        }

        private DataRepositories CreateDataRepositories(HttpRequestMessage request)
        {
            var isTesting = request.Headers.Contains("Testing");
            return DataAccessFactory.CreateDataAccess(isTesting);
        }
    }
}