using ChipMongWebApp.Utils.Attribute;
using ChipMongWebApp.Utils.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    [Authentication]
    [ErrorLogger]
    public class RetailerController : Controller
    {
        private readonly RetailerHandler handler;

        public RetailerController()
        {
            handler = new RetailerHandler();
        }

        //-> Delete
        //??? why if use http delete - resource alway not found ? ???
        [HttpPost]
        public async Task<string> Delete(int id)
        {
            try
            {
                if (await handler.Delete(id))
                {
                    Response.StatusCode = 200;
                    return "ok";
                }
                return null;
            }
            catch (HttpException ex)
            {
                Response.StatusCode = 400;
                return ex.Message;
            }
        }
    }
}