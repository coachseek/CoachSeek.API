namespace CoachSeek.Api.Models.Api
{
    public class ApiEmailTemplateSaveCommand
    {
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}