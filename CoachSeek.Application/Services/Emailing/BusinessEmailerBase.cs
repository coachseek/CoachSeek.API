using System;
using System.IO;
using System.Reflection;
using CoachSeek.Application.Contracts;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Contracts.Emailing.Interfaces;

namespace CoachSeek.Application.Services.Emailing
{
    public abstract class BusinessEmailerBase : IApplicationContextSetter
    {
        protected ApplicationContext Context { get; set; }
        protected Guid BusinessId { get { return Context.BusinessContext.Business.Id; } }
        protected string BusinessName { get { return Context.BusinessContext.Business.Name; } }
        protected string CurrencySymbol { get { return Context.BusinessContext.Currency.Symbol; } }
        protected bool IsTesting { get { return Context.IsTesting; } }
        protected bool ForceEmail { get { return Context.EmailContext.ForceEmail; } }
        protected string Sender { get { return Context.EmailContext.EmailSender; } }
        protected bool IsEmailingEnabled { get { return Context.EmailContext.IsEmailingEnabled; } }
        protected IBusinessRepository BusinessRepository { get { return Context.BusinessContext.BusinessRepository; } }


        protected IEmailer Emailer
        {
            get { return EmailerFactory.CreateEmailer(IsTesting, Context.EmailContext); }
        }

        protected string ReadEmbeddedTextResource(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
        }

        public virtual void Initialise(ApplicationContext context)
        {
            Context = context;
        }
    }
}
