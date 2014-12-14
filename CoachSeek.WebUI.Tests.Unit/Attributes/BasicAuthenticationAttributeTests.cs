using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CoachSeek.Api.Attributes;
using NUnit.Framework;

namespace CoachSeek.WebUI.Tests.Unit.Attributes
{
    [TestFixture]
    public class BasicAuthenticationAttributeTests
    {
        [Test, Ignore]
        public void GivenNoAuthorizationHeader_WhenCallAuthenticateAsync_ThenReturnNothing()
        {
            var context = GivenNoAuthorizationHeader();
            WhenCallAuthenticateAsync(context);
            ThenReturnNothing();
        }

        private HttpAuthenticationContext GivenNoAuthorizationHeader()
        {
            var context = new HttpAuthenticationContext(new HttpActionContext(), null);
            context.Request.Headers.Authorization = null;
            return context;
        }

        private void WhenCallAuthenticateAsync(HttpAuthenticationContext context)
        {
            var cancellationToken = new CancellationToken();
            var attribute = new BasicAuthenticationAttribute();
            var x = attribute.AuthenticateAsync(context, cancellationToken);
        }

        private void ThenReturnNothing()
        {
            throw new System.NotImplementedException();
        }
    }
}
