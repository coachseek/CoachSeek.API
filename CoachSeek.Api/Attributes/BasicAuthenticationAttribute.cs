using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using CoachSeek.Api.Results;
using CoachSeek.Common;
using CoachSeek.Domain.Repositories;
using CoachSeek.Domain.Services;

namespace CoachSeek.Api.Attributes
{
    public class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private IUserRepository UserRepository { get; set; }

        public BasicAuthenticationAttribute()
        {
            UserRepository = MvcApplication.IocContainer.GetInstance<IUserRepository>();
        }


        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            if (authorization.Scheme != "Basic")
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            var userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);

            if (userNameAndPasword == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return;
            }

            var username = userNameAndPasword.Item1;
            var password = userNameAndPasword.Item2;

            var principal = Authenticate(username, password, cancellationToken);

            if (principal == null)
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password.", request);
                return;
            }
            
            if (principal is NoBusinessPrincipal)
            {
                // Authentication was successful but the user is not associated with a business.
                context.ErrorResult = new AuthenticationFailureResult("User is not associated with a business.", request);
                return;
            }

            // Authentication was attempted and succeeded. Set Principal to the authenticated user.
            context.Principal = principal;
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
        }

        public bool AllowMultiple
        {
            get { return false; }
        }


        private IPrincipal Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = UserRepository.GetByUsername(username);
            if (user == null)
                return null;

            var isAuthenticated = PasswordHasher.ValidatePassword(password, user.PasswordHash);
            if (!isAuthenticated)
                return null;

            if (!user.BusinessId.HasValue)
                return new NoBusinessPrincipal();

            cancellationToken.ThrowIfCancellationRequested();
            var identity = new CoachseekIdentity(username, "Basic", user.BusinessId.Value);
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
            Encoding encoding = Encoding.ASCII;
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

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            string userName = decodedCredentials.Substring(0, colonIndex);
            string password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName, password);
        }

        //private UserManager<User> CreateUserManager()
        //{
        //    var userStore = UserStore;

        //    return new UserManager<User>(userStore);
        //}
    }
}