namespace CoachSeek.Data.Model
{
    public class RegistrationData
    {
        public UserData Admin { get; set; }
        public Business2Data Business { get; set; }


        public RegistrationData(UserData user, Business2Data business)
        {
            Admin = user;
            Business = business;
        }
    }
}
