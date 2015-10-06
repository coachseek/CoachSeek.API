using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class PersonName
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Name { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public PersonName(string firstName, string lastName)
        {
            Validate(firstName, lastName);

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        private void Validate(string firstName, string lastName)
        {
            var exception = new ValidationException();

            if (string.IsNullOrWhiteSpace(firstName))
                exception.Add(new FirstNameRequired()); 
            if (string.IsNullOrWhiteSpace(lastName))
                exception.Add(new LastNameRequired());

            exception.ThrowIfErrors();
        }
    }
}
