using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Common.Extensions
{
    public static class ListExtensions
    {
        public static IReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> input)
        {
            if (input == null)
                return null;
            return input.ToList().AsReadOnly();
        }
    }
}
