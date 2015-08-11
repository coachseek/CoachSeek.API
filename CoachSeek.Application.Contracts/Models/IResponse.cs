using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models
{
    public interface IResponse
    {
        bool IsSuccessful { get; }
        IList<ErrorData> Errors { get; }
        object Data { get; }
    }
}
