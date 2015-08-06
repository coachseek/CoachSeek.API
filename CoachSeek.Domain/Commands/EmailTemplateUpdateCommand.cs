using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Commands
{
    public class EmailTemplateUpdateCommand : ICommand
    {
        public string Type { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
