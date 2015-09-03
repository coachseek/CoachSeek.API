namespace Coachseek.Integration.Contracts.DataImport
{
    public class MessageTooLarge : DataImportException
    {
        public override string Message
        {
            get { return "Message is too large."; }
        }
    }
}
