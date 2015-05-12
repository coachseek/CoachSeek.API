using System;
using System.IO;
using System.Reflection;
using CoachSeek.Application.Contracts;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Contracts.Interfaces;

namespace CoachSeek.Application.Services.Emailing
{
    public abstract class BusinessEmailerBase : IApplicationContextSetter
    {
        protected ApplicationContext Context { get; set; }
        protected Guid BusinessId { get { return Context.BusinessId.Value; } }
        protected string BusinessName { get { return Context.BusinessName; } }
        protected bool IsTesting { get { return Context.IsTesting; } }
        protected bool ForceEmail { get { return Context.ForceEmail; } }
        protected string Sender { get { return Context.EmailSender; } }
        protected bool IsEmailingEnabled { get { return Context.IsEmailingEnabled; } }
        protected IBusinessRepository BusinessRepository { get { return Context.BusinessRepository; } }


        protected IEmailer Emailer
        {
            get { return EmailerFactory.CreateEmailer(IsEmailingEnabled, IsTesting, ForceEmail); }
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
