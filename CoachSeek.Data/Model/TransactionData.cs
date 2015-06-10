namespace CoachSeek.Data.Model
{
    public class TransactionData
    {
        public string Id { get; private set; }
        public string Type { get; private set; }
        public string Status { get; private set; }


        public TransactionData(string id, string type, string status)
        {
            Id = id;
            Type = type;
            Status = status;
        }
    }
}
