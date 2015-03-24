namespace CoachSeek.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNew(this object obj)
        {
            return obj == null;
        }

        public static bool IsExisting(this object obj)
        {
            return obj != null;
        }


        public static bool IsFound(this object obj)
        {
            return obj != null;
        }

        public static bool IsNotFound(this object obj)
        {
            return !IsFound(obj);
        }
    }
}