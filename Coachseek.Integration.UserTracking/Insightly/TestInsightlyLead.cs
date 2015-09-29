using CoachSeek.Data.Model;
using Newtonsoft.Json;

namespace Coachseek.Integration.UserTracking.Insightly
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TestInsightlyLead : InsightlyLead
    {
        public TestInsightlyLead(UserData user, BusinessData business)
            : base(user, business)
        { }


        // 475333 is the Insightly Status Id for 'OPEN - NEW LEAD' for the Coachseek Insightly Test account.   
        [JsonProperty(PropertyName = "LEAD_STATUS_ID", NullValueHandling = NullValueHandling.Ignore)]
        public override int StatusId { get { return 475333; } }

        // 476459 is the Insightly Source Id for 'Signup' for the Coachseek Insightly Test account.   
        [JsonProperty(PropertyName = "LEAD_SOURCE_ID", NullValueHandling = NullValueHandling.Ignore)]
        public override int SourceId { get { return 476459; } }
    }
}