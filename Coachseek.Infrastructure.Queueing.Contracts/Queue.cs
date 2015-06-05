namespace Coachseek.Infrastructure.Queueing.Contracts
{
    public class Queue
    {
        public string Name { get; private set; }
        public string Url { get; private set; }


        public Queue(string name)
        {
            Name = name;
        }

        public Queue(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
