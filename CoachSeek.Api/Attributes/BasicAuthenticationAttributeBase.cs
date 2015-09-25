using System.Net.Http;
using CoachSeek.Api.Results;
using CoachSeek.Common.Extensions;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Api.Attributes
{
    public abstract class BasicAuthenticationAttributeBase : Attribute, IAuthenticationFilter
    {
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

            await AuthenticateUserAsync(username, password, context, cancellationToken);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
        }

        public bool AllowMultiple
        {
            get { return false; }
        }


        protected virtual async Task AuthenticateUserAsync(string username, string password, HttpAuthenticationContext context, CancellationToken cancellationToken)
        {

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
                return null;

            var colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
                return null;

            var userName = decodedCredentials.Substring(0, colonIndex);
            var password = decodedCredentials.Substring(colonIndex + 1);

            return new Tuple<string, string>(userName, password);
        }


        protected DataRepositories CreateDataRepositories(HttpRequestMessage request)
        {
            var isTesting = request.Headers.Contains("Testing");
            return DataAccessFactory.CreateDataAccess(isTesting);
        }
    }
}