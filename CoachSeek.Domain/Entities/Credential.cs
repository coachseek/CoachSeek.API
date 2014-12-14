namespace CoachSeek.Domain.Entities
{
    public class Credential
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }

        public Credential(string username, string password)
        {
            Username = username.Trim().ToLower();
            Password = password;
            // TODO: Hash password!
            PasswordHash = password;
            PasswordSalt = string.Empty;
        }

        public Credential(string username, string passwordHash, string passwordSalt)
        {
            Username = username.Trim().ToLower();
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
