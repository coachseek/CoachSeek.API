using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class StartDateAfterEndDate : DateInvalid
    {
        public StartDateAfterEndDate(Date startDate, Date endDate)
            : base(ErrorCodes.StartDateAfterEndDate,
                   string.Format("Start date '{0}' is after end date '{1}'", startDate, endDate),
                   string.Format("Start date: '{0}', End date: '{1}'", startDate, endDate))
        { }
    }
}
