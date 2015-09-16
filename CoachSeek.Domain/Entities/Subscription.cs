using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public abstract class Subscription
    {
        public static Subscription Create(string subscription)
        {
            if (IsTrial(subscription))
                return new TrialSubscription();

            //if (IsSolo(subscription))
            //    return new SoloSubscription();
            //if (IsTrial(subscription))
            //    return new TrialSubscription();
            //if (IsTrial(subscription))
            //    return new TrialSubscription();

            return null;
        }

        private static bool IsTrial(string subscription)
        {
            return String.Equals(subscription, Constants.SUBSCRIPTION_TRIAL, StringComparison.CurrentCultureIgnoreCase);
        }


        public abstract int MaximumNumberOfCoaches { get; }
    }
}
