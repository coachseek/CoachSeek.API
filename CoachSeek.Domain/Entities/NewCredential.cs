using CoachSeek.Domain.Services;

namespace CoachSeek.Domain.Entities
{
    public class NewCredential : Credential
    {
        public NewCredential(string username, string password):
            base(username, PasswordHasher.CreateHash(password))
        { }
    }
}
