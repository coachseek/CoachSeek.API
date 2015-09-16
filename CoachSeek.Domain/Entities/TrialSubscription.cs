
using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class TrialSubscription : Subscription
    {
        public override int MaximumNumberOfCoaches
        {
            get { return Constants.MAXIMUM_NUMBER_OF_COACHES; }
        }
    }
}
