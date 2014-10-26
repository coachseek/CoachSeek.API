using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Exceptions
{
    public class Error
    {
        //public int Code { get; private set; }
        public string Message { get; private set; }
        public string Field { get; private set; }

        public Error(string message, string field = null)
        {
            //Code = code;
            Message = message;
            Field = field;
        }


        public ErrorData ToData()
        {
            return AutoMapper.Mapper.Map<Error, ErrorData>(this);
        }
    }
}
