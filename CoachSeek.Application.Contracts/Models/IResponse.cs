using System.Collections.Generic;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models
{
    public interface IResponse
    {
        bool IsSuccessful { get; }
        IList<Error> Errors { get; }
        object Data { get; }
    }
}
