namespace BouncedEmailProcessor
{
    public class Queue
    {
        public string Name { get; private set; }
        public string Url { get; private set; }

        public Queue(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
