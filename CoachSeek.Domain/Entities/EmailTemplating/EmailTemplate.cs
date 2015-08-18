using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities.EmailTemplating
{
    public abstract class EmailTemplate : IEmailTemplate
    {
        public abstract string Type { get; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public abstract IList<string> Placeholders { get; }


        protected EmailTemplate(EmailTemplateUpdateCommand command)
        {
            ValidateCommand(command);

            Subject = command.Subject;
            Body = command.Body;
        }

        protected EmailTemplate(EmailTemplateData templateData)
        {
            Subject = templateData.Subject;
            Body = templateData.Body;
        }

        protected EmailTemplate(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }

        public EmailTemplateData ToData()
        {
            return new EmailTemplateData
            {
                Type = Type,
                Subject = Subject,
                Body = Body,
                Placeholders = Placeholders
            };
        }


        protected IList<string> WrapPlaceholdersInDelimiter(string[] placeholders)
        {
            return placeholders.Select(placeholder => string.Format("<<{0}>>", placeholder)).ToList();
        }

        private void ValidateCommand(EmailTemplateUpdateCommand command)
        {
            // Email subject is already validated as part of data input validation. 
        }
    }
}
