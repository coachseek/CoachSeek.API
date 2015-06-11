namespace CoachSeek.Domain.Entities
{
    public class GoodOrService
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Money Money { get; set; }

        public GoodOrService(string id, string name, Money money)
        {
            Id = id;
            Name = name;
            Money = money;
        }
    }
}
