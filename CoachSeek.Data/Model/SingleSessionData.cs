using System;

namespace CoachSeek.Data.Model
{
    public class SingleSessionData : SessionData
    {
        public Guid? ParentId { get; set; }
        public SingleSessionPricingData Pricing { get; set; }
        public SingleRepetitionData Repetition { get; set; }    // Have this so that in a response the client know it is a single session.


        public SingleSessionData()
        {
            Repetition = new SingleRepetitionData();
        }

        public SingleSessionData(SessionData session, SingleSessionPricingData pricing)
            : base(session)
        {
            Pricing = pricing;
            Repetition = new SingleRepetitionData();
        }
    }
}
