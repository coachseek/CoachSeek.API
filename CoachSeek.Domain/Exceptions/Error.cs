using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Exceptions
{
    public class Error
    {
        public string Message { get; private set; }
        public string Field { get; private set; }
        public string Code { get; private set; }
        public string Data { get; private set; }

        public Error(string message, string field = null)
        {
            Message = message;
            Field = field;
        }

        public Error(string message, string code, string data = null)
        {
            Message = message;
            Code = code;
            Data = data;
        }


        public ErrorData ToData()
        {
            return AutoMapper.Mapper.Map<Error, ErrorData>(this);
        }
    }
}
