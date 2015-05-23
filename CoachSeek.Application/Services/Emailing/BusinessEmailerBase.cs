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
        protected Guid BusinessId { get { return Context.Business.BusinessId.Value; } }
        protected string BusinessName { get { return Context.Business.BusinessName; } }
        protected bool IsTesting { get { return Context.IsTesting; } }
        protected bool ForceEmail { get { return Context.Email.ForceEmail; } }
        protected string Sender { get { return Context.Email.EmailSender; } }
        protected bool IsEmailingEnabled { get { return Context.Email.IsEmailingEnabled; } }
        protected IBusinessRepository BusinessRepository { get { return Context.Business.BusinessRepository; } }


        protected IEmailer Emailer
        {
            get { return EmailerFactory.CreateEmailer(IsTesting, Context.Email); }
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
