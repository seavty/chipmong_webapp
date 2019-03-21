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
    public class SubCustomerController : Controller
    {
        private readonly SubCustomerHandler handler;
        // GET: SubCustomer
        public SubCustomerController()
        {
            handler = new SubCustomerHandler();
        }

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