using System.Collections.Generic;

namespace CoachSeek.Domain.Entities.EmailTemplating
{
    public interface IEmailTemplate
    {
        string Type { get; }
        string Subject { get; }
        string Body { get; }
        IList<string> Placeholders { get; }
    }
}
