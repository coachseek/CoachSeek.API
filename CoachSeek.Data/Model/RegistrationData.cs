namespace CoachSeek.Data.Model
{
    public class RegistrationData
    {
        public UserData Admin { get; set; }
        public BusinessData Business { get; set; }


        public RegistrationData(UserData user, BusinessData business)
        {
            Admin = user;
            Business = business;

            Admin.BusinessId = Business.Id;
            Admin.BusinessName = Business.Name;
        }
    }
}
