using Coachseek.Integration.Contracts.Models;

namespace Coachseek.Integration.Emailing.Amazon.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var bounceEmail = new Email("olaf@coachseek.com", "bounce@simulator.amazonses.com", "bounce email", "this is a bounce email.");

            var emailer = new AmazonEmailer();
            emailer.Send(bounceEmail);
        }
    }
}
