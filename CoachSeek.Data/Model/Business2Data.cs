using System;

namespace CoachSeek.Data.Model
{
    public class Business2Data : IData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }


        public string GetName()
        {
            return "Business";
        }
    }
}
