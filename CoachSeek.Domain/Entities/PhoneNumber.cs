namespace CoachSeek.Domain.Entities
{
    public class PhoneNumber
    {
        public string Phone { get; private set; }

        public PhoneNumber(string phone)
        {
            // Uppercase:0800 HOTCHICKS
            Phone = phone.Trim().ToUpper();
        }
    }
}
