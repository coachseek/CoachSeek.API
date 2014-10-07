using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain
{
    public class PersonName
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Name { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public PersonName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new MissingFirstName();
            if (string.IsNullOrWhiteSpace(lastName))
                throw new MissingLastName();

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }
    }
}
