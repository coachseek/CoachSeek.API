using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models
{
    public class Response : IResponse
    {
        public object Data { get; protected set; }

        public Response(object data = null)
        {
            Data = data;
        }

        public virtual bool IsSuccessful { get { return true; } }
        public IList<ErrorData> Errors { get { return null; } }
    }
}