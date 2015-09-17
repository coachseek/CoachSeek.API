using CoachSeek.Common;

namespace CoachSeek.Domain.Entities.Subscriptions
{
    public class UnlimitedSubscription : Subscription
    {
        public override string Plan { get { return Constants.SUBSCRIPTION_UNLIMITED; } }
        public override int MaximumNumberOfCoaches { get { return Constants.MAXIMUM_NUMBER_OF_COACHES; } }
    }
}
