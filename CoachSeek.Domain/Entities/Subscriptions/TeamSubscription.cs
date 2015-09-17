using CoachSeek.Common;

namespace CoachSeek.Domain.Entities.Subscriptions
{
    public class TeamSubscription : Subscription
    {
        public override string Plan { get { return Constants.SUBSCRIPTION_TEAM; } }
        public override int MaximumNumberOfCoaches { get { return 5; } }
    }
}
