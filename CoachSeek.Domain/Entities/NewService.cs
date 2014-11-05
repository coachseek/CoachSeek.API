using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewService : Service
    {
        public NewService(string name, 
                          string description, 
                          ServiceDefaultsData defaults, 
                          ServicePricingData pricing, 
                          ServiceRepetitionData repetition)
            : base(Guid.NewGuid(), name, description, defaults, pricing,repetition)
        { }

        public NewService(NewServiceData data)
            : this(data.Name, data.Description, data.Defaults, data.Pricing, data.Repetition)
        { }
    }
}