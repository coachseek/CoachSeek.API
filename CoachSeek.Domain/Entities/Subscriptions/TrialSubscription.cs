using CoachSeek.Common;

namespace CoachSeek.Domain.Entities.Subscriptions
{
    public class TrialSubscription : Subscription
    {
        public override string Plan { get { return Constants.SUBSCRIPTION_TRIAL; } }
        public override int MaximumNumberOfCoaches { get { return Constants.MAXIMUM_NUMBER_OF_COACHES; } }
    }
}
