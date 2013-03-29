using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authen.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index(string ReturnURL)
        {

            // if passed token, then check token
            //if (Token != null && Token == Session["Token"] as string)
            //{
            //    return Redirect(ReturnURL + "?Token="+Token );
            //}
            
            var httpCookie = Request.Cookies["Token"];
            if (httpCookie != null)
            {
                var token = httpCookie.Value as string;

                if (!string.IsNullOrEmpty(token) && token == Session["Token"] as string)
                {
                  return  Redirect(ReturnURL + "?Token=" + Session["Token"] as string);
                }
                
            }
            ViewBag.ReturnURL = ReturnURL;
            return View();   
        }


        public ActionResult Test()
        {
            return Content("hello Test");
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string URL = form["ReturnURL"] as string;

            string token = Guid.NewGuid().ToString();

            Session["Token"] =token ;

            Response.Cookies.Add(new HttpCookie("Token",token));

            return Redirect(URL + "?Token=" + token);

        }

        public ActionResult CheckToken(string token, string ReturnURL)
        {

            var serverLoginSession = Session["Token"] as string;
            if (token ==serverLoginSession )
            {
                return Redirect(ReturnURL + "?Token="+serverLoginSession+"&success=true");
            }
            else
            {
                return Redirect(ReturnURL + "?Token=" + serverLoginSession + "&success=false");
            }
        }




        public ActionResult LoginWithJsonp(string callback)
        {
            return new JsonpResult<object>(new { success = true, message = "hello world" }, callback);
        }

        public ActionResult Back()
        {
            return Redirect(@"http://www.a.com");
        }

    }


    public class JsonpResult<T> : ActionResult
    {
        public T Obj { get; set; }
        public string CallbackName { get; set; }

        public JsonpResult(T obj, string callback)
        {
            this.Obj = obj;
            this.CallbackName = callback;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonp = this.CallbackName + "(" + js.Serialize(this.Obj) + ")";

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write(jsonp);
        }
    }
}
