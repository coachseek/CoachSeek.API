namespace CoachSeek.Domain.Commands
{
    public class CustomFieldAddCommand
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
    }
}