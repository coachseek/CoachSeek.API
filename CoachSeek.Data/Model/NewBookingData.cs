namespace CoachSeek.Data.Model
{
    public class NewBookingData : IData
    {
        public SessionKeyData Session { get; set; }
        public CustomerKeyData Customer { get; set; }


        public string GetName()
        {
            return "booking";
        }
    }
}