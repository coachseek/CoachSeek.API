namespace CoachSeek.Data.Model
{
    public class CoreData
    {
        public LocationData Location { get; set; }
        public CoachData Coach { get; set; }
        public ServiceData Service { get; set; }


        public CoreData()
        { }

        public CoreData(LocationData location, CoachData coach, ServiceData service)
        {
            Location = location;
            Coach = coach;
            Service = service;
        }
    }
}
