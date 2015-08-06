using System.Collections.Generic;

namespace CoachSeek.Data.Model
{
    public class EmailTemplateData
    {
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<string> Placeholders { get; set; }

        public EmailTemplateData()
        {
            Placeholders = new List<string>();
        }
    }
}
