namespace CoachSeek.Domain.Exceptions
{
    public class Error
    {
        public string Code { get; private set; }
        public string Message { get; private set; }
        public string Data { get; private set; }
        public string Field { get; private set; }

        public Error(string message, string field = null)
        {
            Message = message;
            Field = field;
        }

        public Error(string code, string message, string data = null)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
