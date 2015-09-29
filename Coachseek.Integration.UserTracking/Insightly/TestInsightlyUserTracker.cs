namespace Coachseek.Integration.UserTracking.Insightly
{
    public class TestInsightlyUserTracker : InsightlyUserTracker
    {
        private const string TEST_ACCOUNT_API_KEY = "99e5acda-2333-40e2-8771-02335451206c";

        public TestInsightlyUserTracker() 
            : base(TEST_ACCOUNT_API_KEY)
        { }
    }
}
