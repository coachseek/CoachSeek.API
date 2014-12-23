namespace CoachSeek.Data.Model
{
    public class RegistrationData
    {
        public UserData User { get; set; }
        public BusinessData Business { get; set; }


        public RegistrationData(UserData user, BusinessData business)
        {
            User = user;
            Business = business;
        }
    }
}
