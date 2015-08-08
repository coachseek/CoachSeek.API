using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Factories;

namespace CoachSeek.Domain.Entities.EmailTemplating
{
    public class EmailTemplateCollection
    {
        private List<EmailTemplate> Templates { get; set; }


        public EmailTemplateCollection(IEnumerable<EmailTemplateData> customisedTemplates)
        {
            Templates = new List<EmailTemplate>();
            PopulateTemplates(customisedTemplates);
            Templates = Templates.OrderBy(x => x.Type).ToList();
        }

        public IList<EmailTemplateData> ToData()
        {
            return Templates.Select(x => x.ToData()).ToList();
        }


        private void PopulateTemplates(IEnumerable<EmailTemplateData> customisedTemplates)
        {
            PopulateCustomiseTemplates(customisedTemplates);
            PopulateDefaultTemplates();
        }

        private void PopulateCustomiseTemplates(IEnumerable<EmailTemplateData> customisedTemplatesData)
        {
            foreach (var templateData in customisedTemplatesData)
            {
                var template = EmailTemplateFactory.BuildEmailTemplate(templateData);
                Templates.Add(template);
            }
        }

        private void PopulateDefaultTemplates()
        {
            if (DoesNotContain(typeof(CustomerOnlineBookingSessionEmailTemplate)))
                Templates.Add(new CustomerOnlineBookingSessionEmailTemplateDefault());

            if (DoesNotContain(typeof(CustomerOnlineBookingCourseEmailTemplate)))
                Templates.Add(new CustomerOnlineBookingCourseEmailTemplateDefault());
        }

        private bool DoesNotContain(Type templateType)
        {
            return !Templates.Select(x => x.GetType()).Contains(templateType);
        }
    }
}
