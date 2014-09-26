namespace CoachSeek.WebUI.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsExisting(this object obj)
        {
            return obj != null;
        }
    }
}