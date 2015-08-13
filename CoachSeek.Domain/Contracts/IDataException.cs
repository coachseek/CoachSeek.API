using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Contracts
{
    public interface IDataException
    {
        IList<ErrorData> ToData();
    }
}
