using System.Collections.Generic;
using AutoMapper;

namespace CoachSeek.Data.Model
{
    public class RepeatedSessionData : SessionData
    {
        public RepetitionData Repetition { get; set; }
        public RepeatedSessionPricingData Pricing { get; set; }

        public IList<SingleSessionData> Sessions { get; set; }


        public RepeatedSessionData()
        { }

        public RepeatedSessionData(SessionData session, RepetitionData repetition, RepeatedSessionPricingData pricing, IList<SingleSessionData> childSessions)
            : base(session)
        {
            Repetition = repetition;
            Pricing = pricing;

            Sessions = childSessions;
        }
    }
}
