namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public class DbRepetition
    {
        public int SessionCount { get; set; }
        public string RepeatFrequency { get; set; }


        public DbRepetition()
        {
            SessionCount = 1;
        }
    }
}
