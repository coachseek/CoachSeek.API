namespace CoachSeek.WebUI.Models.Api
{
    public class ApiServiceDefaults
    {
        // Session related
        public int Duration { get; set; }
        public int Price { get; set; }
        public int StudentCapacity { get; set; }
        public bool IsOnlineBookable { get; set; }

        // Session Repeatability releated
        public bool IsRepeated { get; set; }
        public string RepeatFrequency { get; set; }
        public int RepeatTimes { get; set; }

        // Display related
        public string Colour { get; set; }
    }
}