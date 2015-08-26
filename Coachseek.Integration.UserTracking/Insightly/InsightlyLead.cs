using Newtonsoft.Json;

namespace Coachseek.Integration.UserTracking.Insightly
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InsightlyLead
    {
        public InsightlyLead(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }


        [JsonProperty(PropertyName = "FIRST_NAME", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; private set; }

        [JsonProperty(PropertyName = "LAST_NAME", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; private set; }

        [JsonProperty(PropertyName = "EMAIL_ADDRESS", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; private set; }
    }
}