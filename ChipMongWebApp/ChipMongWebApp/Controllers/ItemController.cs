using ChipMongWebApp.Handlers;
using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DTO.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    public class ItemController : Controller
    {
        private ItemHandler handler = null;

        public ItemController() { handler = new ItemHandler(); }

        //-> Index
        public ActionResult Index() { return View(); }

        //-> Create
        public ActionResult Create() { return View(); }

        //-> Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(ItemNewDTO newDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;

                var myTEst = await handler.Create(newDTO);
                return Json(await handler.Create(newDTO), JsonRequestBehavior.AllowGet);

            }
            catch (HttpException)
            {
                return Json(ConstantHelper.ERROR, JsonRequestBehavior.AllowGet);
            }
            
        }
        
        //-> view
        public async Task<ActionResult> View(int id) { return View(await handler.SelectByID(id)); }


    }
}