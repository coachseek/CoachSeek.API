using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Coachseek.Integration.Payments.Paypal.Tester.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Payment()
        {
            ViewBag.Message = "Your payment page.";

            return View();
        }

        public RedirectResult MakePayment()
        {
            ViewBag.Message = "Your payment page.";

            var paypalParameters = new StringBuilder();
            paypalParameters.AppendFormat("business={0}", "olaf-facilitator@coachseek.com");
            paypalParameters.AppendFormat("&currency_code={0}", "NZD");
            paypalParameters.AppendFormat("&amount={0}", "12.35");
            paypalParameters.AppendFormat("&item_name={0}", "Mini Red Session");
            paypalParameters.AppendFormat("&notify_url={0}", "https://api.coachseek.com/PayPal/Notify");
            paypalParameters.AppendFormat("&return={0}", "https://api.coachseek.com/PayPal/return");
            paypalParameters.AppendFormat("&invoice={0}", "ABC123");

            var url = String.Format("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&{0}", paypalParameters);
            return new RedirectResult(url);
        }
    }
}