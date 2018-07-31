using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChipMongWebApp.Utils.Attribute
{
    public class CheckSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            /*
            var session = filterContext.HttpContext.Session;
            if ((bool?)session["IsManager"] == true)
                return;

            //Redirect him to somewhere.
            */
            /*
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (controllerName == "Foo" && actionName == "Bar")
            {
                return;
            }
            */
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            
            if (controllerName == "Auth")
            {
                return;
            }

            var redirectTarget = new RouteValueDictionary
            {
                { "action", "login"}, {"controller", "auth"}
            };
            filterContext.Result = new RedirectToRouteResult(redirectTarget);

        }
    }
}