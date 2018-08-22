using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.Customer;
using ChipMongWebApp.Models.DTO.DealerSourceSupply;
using ChipMongWebApp.Models.DTO.SaleOrder;
using ChipMongWebApp.Models.DTO.SourceSupply;
using ChipMongWebApp.Models.DTO.SSA;
using ChipMongWebApp.Utils.Attribute;
using ChipMongWebApp.Utils.Handlers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace ChipMongWebApp.Controllers
{
    [Authentication]
    [ErrorLogger]
    public class DealerSourceSupplyController : Controller
    {
        private readonly DealerSourceSupplyHandler handler;

        public DealerSourceSupplyController()
        {
            handler = new DealerSourceSupplyHandler();
        }
        //-> Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Edit(int id)
        {
            return await handler.Edit(id, int.Parse(Request.QueryString["sourceOfSupplyID"].ToString()));
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
