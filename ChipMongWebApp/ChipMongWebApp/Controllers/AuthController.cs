using ChipMongWebApp.Handlers;
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
            return View();
        }

        [HttpPost]
        public async Task<string> Login(UserCredentialDTO credential)
        {
            /*
            if (login.userName == "admin" && login.password == "123")
            {
                Session["id"] = "1";
                return "ok";
            }
            Response.StatusCode = 404;
            return "";
            */
            var hander = new AuthHandler();
            var user = await hander.Login(credential);
            if (user == null)
            {
                Response.StatusCode = 401;
                return "";
            }
            Session["user"] = user;
            return "ok";



        }
    }
}