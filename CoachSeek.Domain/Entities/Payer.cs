namespace CoachSeek.Domain.Entities
{
    public class Payer
    {
        private PersonName PersonName { get; set; }
        private EmailAddress EmailAddress { get; set; }

        public string FirstName { get { return PersonName.FirstName; } }
        public string LastName { get { return PersonName.LastName; } }
        public string Email { get { return EmailAddress.Email; } }


        public Payer(string firstName, string lastName, string email)
        {
            PersonName = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
        }
    }
}
