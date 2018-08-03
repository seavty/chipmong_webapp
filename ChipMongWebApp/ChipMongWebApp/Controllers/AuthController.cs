using ChipMongWebApp.Handlers;
using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DTO.Auth;
using ChipMongWebApp.Models.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            var id = RouteData.Values["id"];
            if (id == null)
                return View();
            
            Response.StatusCode = 401;
            return View();
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Login(UserCredentialDTO credential)
        {
            var hander = new AuthHandler();
            var user = await hander.Login(credential);
            if (user == null)
            {
                Response.StatusCode = 404;
                return "";
            }
                    
            else
            {
                Response.StatusCode = 200;
                Session["user"] = user;
                return "ok";

            }
        }

        [HttpPost]
        public string Logout()
        {
            Session.Abandon();
            return "ok";
        }
    }
}