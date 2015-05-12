using System;
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

        public static T Parse<T>(this string input, T defaultValue)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            try
            {
                return (T)converter.ConvertFromString(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T ParseOrThrow<T>(this string input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            try
            {
                return (T)converter.ConvertFromString(input);
            }
            catch(Exception)
            {
                throw new FormatException();
            }
        }

        public static string Capitalise(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}