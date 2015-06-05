namespace Coachseek.Infrastructure.Queueing.Amazon.Models
{
    public class AmazonSesBounceNotification
    {
        public string NotificationType { get; set; }
        public AmazonSesBounce Bounce { get; set; }
    }
}
