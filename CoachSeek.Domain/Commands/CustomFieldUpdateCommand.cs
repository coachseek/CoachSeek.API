namespace CoachSeek.Domain.Commands
{
    public class CustomFieldUpdateCommand : CustomFieldAddCommand
    {
        public string Key { get; set; }
    }
}
