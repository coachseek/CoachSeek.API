using System.Web.Mvc;
using CoachSeek.Api.Attributes;

namespace CoachSeek.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
