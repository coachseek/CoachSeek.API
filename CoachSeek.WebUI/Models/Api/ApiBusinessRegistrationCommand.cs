namespace CoachSeek.WebUI.Models.Api
{
    public class ApiBusinessRegistrationCommand
    {
        public string BusinessName { get; set; }
        public ApiBusinessRegistrant Registrant { get; set; }
    }
}