using System.Web;
using System.Web.Mvc;

namespace Coachseek.Integration.Payments.Paypal.Tester
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
