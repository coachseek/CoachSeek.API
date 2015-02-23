namespace CoachSeek.Data.Model
{
    public class NewLocationData : IData
    {
        public string Name { get; set; }


        public string GetName()
        {
            return "location";
        }
    }
}