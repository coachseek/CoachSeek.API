using Coachseek.Integration.Contracts.Models;

namespace Coachseek.Integration.Emailing.Amazon.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            SendBounceEmail();
            //SendComplaintEmail();
            //SendOutOfOfficeEmail();
        }


        private static void SendBounceEmail()
        {
            var bounceEmail = new Email("noreply@coachseek.com", "bounce@simulator.amazonses.com", "bounce email", "this is a bounce email.");
            var emailer = new AmazonEmailer(null);
            emailer.Send(bounceEmail);
        }

        private static void SendComplaintEmail()
        {
            var complaintEmail = new Email("noreply@coachseek.com", "complaint@simulator.amazonses.com", "complaint email", "this is a complaint email.");
            var emailer = new AmazonEmailer(null);
            emailer.Send(complaintEmail);
        }

        private static void SendOutOfOfficeEmail()
        {
            var outOfOfficeEmail = new Email("noreply@coachseek.com", "ooto@simulator.amazonses.com", "out of office email", "this is a out of office email.");
            var emailer = new AmazonEmailer(null);
            emailer.Send(outOfOfficeEmail);
        }
    }
}
