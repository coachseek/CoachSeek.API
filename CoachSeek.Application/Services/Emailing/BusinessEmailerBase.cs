using System.IO;
using System.Reflection;
using CoachSeek.Application.Contracts;
using CoachSeek.Application.Contracts.Models;
using Coachseek.Integration.Contracts.Interfaces;

namespace CoachSeek.Application.Services.Emailing
{
    public abstract class BusinessEmailerBase : IApplicationContextSetter
    {
        protected ApplicationContext Context { get; set; }

        protected bool IsTesting { get { return Context.IsTesting; } }
        protected bool ForceEmail { get { return Context.ForceEmail; } }
        protected string Sender { get { return Context.EmailSender; } }


        protected IEmailer Emailer
        {
            get { return EmailerFactory.CreateEmailer(IsTesting, ForceEmail); }
        }

        protected string ReadEmbeddedTextResource(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        public void Initialise(ApplicationContext context)
        {
            Context = context;
        }
    }
}
