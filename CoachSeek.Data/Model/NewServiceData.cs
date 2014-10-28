namespace CoachSeek.Data.Model
{
    public class NewServiceData : IData
    {
        public string Name { get; set; }


        public string GetName()
        {
            return "service";
        }

        public string GetBusinessIdPath()
        {
            return "service.businessId";
        }
    }
}
