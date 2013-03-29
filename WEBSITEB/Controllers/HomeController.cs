using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEBSITEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["Login"] == null)
            {
                var returnUrl = GetAbsoluteUrl("Login", "Home");
                return Redirect(@"http://www.login.com/Login?ReturnURL=" + returnUrl);
            }

            return View();
        }

        public ActionResult Login()
        {
            if (Request.Params["Token"] != null)
            {
                Session["Token"] = Request.Params["Token"] as string;

                var returnUrl = GetAbsoluteUrl("SetLogin", "Home");

                return Redirect(@"http://www.login.com/Login/CheckToken?ReturnURL=" + returnUrl + "&Token=" + Request.Params["Token"]);
            }

            return null;


        }

        public ActionResult SetLogin()
        {
            if (Request.Params["success"] == "true" && Request.Params["Token"] as string==Session["Token"] as string)
            {
                Session["Login"] = true;
            }

            return RedirectToAction("Index");
            
        }


        private string GetAbsoluteUrl(string action, string controller)
        {
            Uri requestUrl = Url.RequestContext.HttpContext.Request.Url;

            string absoluteAction = string.Format("{0}://{1}{2}",
                                                  requestUrl.Scheme,
                                                  requestUrl.Authority,
                                                  Url.Action(action, controller));

            return absoluteAction;
        }

    }
}
