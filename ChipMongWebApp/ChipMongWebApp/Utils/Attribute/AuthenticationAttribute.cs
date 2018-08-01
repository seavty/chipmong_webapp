using ChipMongWebApp.Handlers;
using ChipMongWebApp.Models.DTO.User;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChipMongWebApp.Utils.Attribute
{
    public class AuthenticationAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            /*
            var session = filterContext.HttpContext.Session;
            if ((bool?)session["IsManager"] == true)
                return;
            */
            /*
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (controllerName == "Foo" && actionName == "Bar")
            {
                return;
            }
            */

            //Redirect him to somewhere.
            var session = filterContext.HttpContext.Session;
            var id = session["id"];
            UserViewDTO user = (UserViewDTO)session["user"];
            var isValidSession = new AuthHandler().IsValidSession(user);

            if (user == null || isValidSession == false)
                RedirectToLogin(filterContext);
            else
                return;
        }

        private void RedirectToLogin(ActionExecutingContext filterContext)
        {
            var redirectTarget = new RouteValueDictionary
            {
                { "action", "login/0"}, {"controller", "auth"}
            };
            filterContext.Result = new RedirectToRouteResult(redirectTarget);

            
            
        }
        

    }


}