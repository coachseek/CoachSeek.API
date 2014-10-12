namespace CoachSeek.Domain
{
    public class Credential
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        private string PasswordSalt { get; set; }

        public Credential(string username, string password)
        {
            Username = username.Trim().ToLower();
            // TODO: Hash password!
            PasswordHash = password;
        }

        public Credential(string username, string passwordHash, string passwordSalt)
        {
            Username = username.Trim().ToLower();
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
