namespace CoachSeek.Domain.Exceptions
{
    public class NotEnoughDataParts : CoachseekException
    {
        public NotEnoughDataParts() 
            : base("Not enough data parts in data import line.")
        { }
    }
}
