using ChipMongWebApp.Models.DTO.User;
using ChipMongWebApp.Utils.Handlers;
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
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (controllerName == "Foo" && actionName == "Bar")
            {
                return;
            }
            */
            var session = filterContext.HttpContext.Session;
            UserViewDTO user = (UserViewDTO)session["user"];
            if (session == null || user == null)
                RedirectToLogin(filterContext);
            else
            {   
                var isValidSession = new AuthHandler().IsValidSession(user);
                if (isValidSession == false)
                    RedirectToLogin(filterContext);
                else
                    return;
            }
        }

        private void RedirectToLogin(ActionExecutingContext filterContext)
        {
            var redirectTarget = new RouteValueDictionary
            {
                { "action", "login/-1"}, {"controller", "auth"}
            };
            filterContext.Result = new RedirectToRouteResult(redirectTarget);
        }
        

    }


}