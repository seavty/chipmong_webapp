using ChipMongWebApp.Models.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Login(LoginDTO login)
        {
            if (login.userName == "admin" && login.password == "123")
                return "ok";
            Response.StatusCode = 404;
            return "";
        }
    }
}