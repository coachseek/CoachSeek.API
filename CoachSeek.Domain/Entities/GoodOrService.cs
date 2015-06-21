using System;

namespace CoachSeek.Domain.Entities
{
    public class GoodOrService
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Money Money { get; set; }

        public GoodOrService(Guid id, string name, Money money)
        {
            Id = id;
            Name = name;
            Money = money;
        }
    }
}
