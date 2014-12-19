namespace CoachSeek.Domain.Entities
{
    public class Credential
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }

        public Credential(string username, string passwordHash)
        {
            Username = username.Trim().ToLower();
            PasswordHash = passwordHash;
        }
    }
}
