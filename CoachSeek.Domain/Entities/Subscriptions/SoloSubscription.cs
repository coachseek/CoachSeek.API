using CoachSeek.Common;

namespace CoachSeek.Domain.Entities.Subscriptions
{
    public class SoloSubscription : Subscription
    {
        public override string Plan { get { return Constants.SUBSCRIPTION_SOLO; } }
        public override int MaximumNumberOfCoaches { get { return 1; } }
    }
}
