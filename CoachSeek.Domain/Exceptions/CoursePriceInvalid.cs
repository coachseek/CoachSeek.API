using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CoursePriceInvalid : PriceInvalid
    {
        public CoursePriceInvalid(decimal coursePrice)
            : base(ErrorCodes.CoursePriceInvalid,
                   string.Format("A CoursePrice of {0} is not valid.", coursePrice),
                   coursePrice)
        { }

        public CoursePriceInvalid(PriceInvalid priceInvalid)
            : base(ErrorCodes.CoursePriceInvalid,
                   string.Format("A CoursePrice of {0} is not valid.", priceInvalid.Price),
                   priceInvalid.Price)
        { }
    }
}
