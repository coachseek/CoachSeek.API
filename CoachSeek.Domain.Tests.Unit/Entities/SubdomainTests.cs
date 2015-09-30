using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SubdomainTests
    {
        [Test]
        public void SubdomainCreationTests()
        {
            SubdomainCreationFailure(null);
            SubdomainCreationFailure("");
            SubdomainCreationFailure("  domain");
            SubdomainCreationFailure("domain  ");
            SubdomainCreationFailure("do main");
            SubdomainCreationFailure("-domain");
            SubdomainCreationFailure("domain-");
            SubdomainCreationFailure("do.main");
            SubdomainCreationFailure(".domain");
            SubdomainCreationFailure("domain.");

            SubdomainCreationSuccess("domain");
            SubdomainCreationSuccess("do-main");
            SubdomainCreationSuccess("do-ma-in");
            SubdomainCreationSuccess("do--main");
        }


        private void SubdomainCreationSuccess(string domain)
        {
            var subdomain = new Subdomain(domain);
            Assert.That(subdomain, Is.Not.Null);
            Assert.That(subdomain.Domain, Is.EqualTo(domain.ToLower()));
        }

        private void SubdomainCreationFailure(string domain)
        {
            try
            {
                var subdomain = new Subdomain(domain);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<SubdomainFormatInvalid>());
            }
        }
    }
}
