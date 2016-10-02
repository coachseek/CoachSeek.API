namespace CoachSeek.Domain.Entities
{
    class Sex
    {
        public string SexType { get; private set; }

        public Sex(string sex)
        {
            if (sex != null)
                SexType = sex.Trim();
        }
    }
}
