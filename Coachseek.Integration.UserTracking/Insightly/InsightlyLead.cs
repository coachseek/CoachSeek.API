﻿using CoachSeek.Data.Model;
using Newtonsoft.Json;

namespace Coachseek.Integration.UserTracking.Insightly
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InsightlyLead : InsightlyEntity
    {
        public InsightlyLead(UserData user, BusinessData business)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
            Sport = business.Sport;
            Currency = business.Payment.Currency;
        }


        [JsonProperty(PropertyName = "FIRST_NAME", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; private set; }

        [JsonProperty(PropertyName = "LAST_NAME", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; private set; }

        [JsonProperty(PropertyName = "EMAIL_ADDRESS", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; private set; }

        [JsonProperty(PropertyName = "PHONE_NUMBER", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; private set; }

        [JsonProperty(PropertyName = "TITLE", NullValueHandling = NullValueHandling.Ignore)]
        public string Sport { get; private set; }

        [JsonProperty(PropertyName = "INDUSTRY", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; private set; }

        // 398793 is the Insightly Status Id for 'OPEN - NEW LEAD' for the Coachseek Insightly account.   
        [JsonProperty(PropertyName = "LEAD_STATUS_ID", NullValueHandling = NullValueHandling.Ignore)]
        public virtual int StatusId { get { return 398793; } }

        // 420119 is the Insightly Source Id for 'Signup' for the Coachseek Insightly account.   
        [JsonProperty(PropertyName = "LEAD_SOURCE_ID", NullValueHandling = NullValueHandling.Ignore)]
        public virtual int SourceId { get { return 420119; } }
    }
}