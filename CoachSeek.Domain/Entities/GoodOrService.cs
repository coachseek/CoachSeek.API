using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachSeek.Domain.Entities
{
    public class GoodOrService
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Money Amount { get; set; }

        public GoodOrService(string id, string name, Money amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }
    }
}
