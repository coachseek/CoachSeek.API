using System;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;

namespace CoachSeek.Domain.Entities.Subscriptions
{
    public abstract class Subscription
    {
        public static Subscription Create(string subscription)
        {
            if (IsTrial(subscription))
                return new TrialSubscription();

            if (IsSolo(subscription))
                return new SoloSubscription();
            if (IsTeam(subscription))
                return new TeamSubscription();
            if (IsPro(subscription))
                return new ProSubscription();
            if (IsUnlimited(subscription))
                return new UnlimitedSubscription();

            throw new InvalidOperationException(string.Format("'{0}' is not a subscription plan.", subscription));
        }

        public abstract string Plan { get; }
        public abstract int MaximumNumberOfCoaches { get; }


        private static bool IsTrial(string subscription)
        {
            return subscription.CompareIgnoreCase(Constants.SUBSCRIPTION_TRIAL);
        }

        private static bool IsSolo(string subscription)
        {
            return subscription.CompareIgnoreCase(Constants.SUBSCRIPTION_SOLO);
        }

        private static bool IsTeam(string subscription)
        {
            return subscription.CompareIgnoreCase(Constants.SUBSCRIPTION_TEAM);
        }

        private static bool IsPro(string subscription)
        {
            return subscription.CompareIgnoreCase(Constants.SUBSCRIPTION_PRO);
        }

        private static bool IsUnlimited(string subscription)
        {
            return subscription.CompareIgnoreCase(Constants.SUBSCRIPTION_UNLIMITED);
        }
    }
}
