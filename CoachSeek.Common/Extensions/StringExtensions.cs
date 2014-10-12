using System.ComponentModel;

namespace CoachSeek.Common.Extensions
{
    public static class StringExtensions
    {
        public static string GetLastCharacter(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return input.Substring(input.Length - 1, 1);
        }

        public static T Parse<T>(this string input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            try
            {
                return (T)converter.ConvertFromString(input);
            }
            catch
            {
                return default(T);                
            }
        }
    }
}