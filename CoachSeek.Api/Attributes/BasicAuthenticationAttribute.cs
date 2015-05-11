using System.Linq;
using CoachSeek.Api.Results;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;
using CoachSeek.Domain.Services;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace CoachSeek.Api.Attributes
{
    public class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private const string BUSINESS_DOMAIN = "Business-Domain";
        private const string BASIC_AUTH = "Basic";

        public async virtual Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // When the Principal is set this indicates Success
            // when the ErrorResult is set this indicates an Error
            // when we just return then the authentication is skipped (but the Authorize attribute will create 401 - Unauthorized error)

            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization.IsNotFound() || authorization.Scheme != BASIC_AUTH)
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            var userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);
            if (userNameAndPasword == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return;
            }

            var username = userNameAndPasword.Item1;
            var password = userNameAndPasword.Item2;

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

        private GenericPrincipal CreateAnonymousPrincipal(BusinessData business)
        {
            var identity = new CoachseekAnonymousIdentity(business.Id, business.Name);
            return new GenericPrincipal(identity, new[] { "Anonymous" });
        }

        private BusinessData LookupBusinessFromDomain(HttpRequestMessage request)
        {
            var domain = request.Headers.GetValues(BUSINESS_DOMAIN).ToList().First();
            var businessRepository = CreateBusinessRepository(request);
            return businessRepository.GetBusiness(domain);
        }

        private IUserRepository CreateUserRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).Item2;
        }

        private IBusinessRepository CreateBusinessRepository(HttpRequestMessage request)
        {
            return CreateDataRepositories(request).Item1;
        }

        private Tuple<IBusinessRepository, IUserRepository> CreateDataRepositories(HttpRequestMessage request)
        {
            var isTesting = request.Headers.Contains("Testing");
            return DataAccessFactory.CreateDataAccess(isTesting);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
        }

        public bool AllowMultiple
        {
            get { return false; }
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

        private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            var encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (string.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            var colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            var userName = decodedCredentials.Substring(0, colonIndex);
            var password = decodedCredentials.Substring(colonIndex + 1);

            return new Tuple<string, string>(userName, password);
        }
    }
}