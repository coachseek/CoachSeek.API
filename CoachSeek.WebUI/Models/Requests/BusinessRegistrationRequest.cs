namespace CoachSeek.WebUI.Models.Requests
{
    public class BusinessRegistrationRequest
    {
        public string BusinessName { get; set; }

        public BusinessRegistrant Registrant { get; set; }
    }
}