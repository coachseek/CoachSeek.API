using CoachSeek.Common;

namespace CoachSeek.Domain.Entities.Subscriptions
{
    public class ProSubscription : Subscription
    {
        public override string Plan { get { return Constants.SUBSCRIPTION_PRO; } }
        public override int MaximumNumberOfCoaches { get { return 10; } }
    }
}
