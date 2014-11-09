namespace CoachSeek.Data.Model
{
    public class NewSessionData : IData
    {
        public string GetName()
        {
            return "session";
        }

        public string GetBusinessIdPath()
        {
            return "session.businessId";
        }
    }
}
