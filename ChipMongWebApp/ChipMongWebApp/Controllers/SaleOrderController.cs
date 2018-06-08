using ChipMongWebApp.Handlers;
using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DTO.SaleOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    public class SaleOrderController : Controller
    {
        private SaleOrderHandler handler = null;

        public SaleOrderController()
        {
            handler = new SaleOrderHandler();
        }

        // GET: SaleOrder
        public ActionResult Index()
        {
            return View();
        }

        //--> Create
        public ActionResult Create()
        {
            return View();
        }

        //-> Create
        [HttpPost]
        public async Task<JsonResult> Create(SaleOrderNewDTO newDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return Json(await handler.Create(newDTO), JsonRequestBehavior.AllowGet);

            }
            catch (HttpException)
            {
                return Json(ConstantHelper.ERROR, JsonRequestBehavior.AllowGet);
            }
        }

        //-> Details
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            return View(await handler.SelectByID(id));
        }

        //-> Edit
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            return PartialView(await handler.SelectByID(id));
        }

        //-> Create
        [HttpPost]
        public async Task<JsonResult> Edit(SaleOrderEditDTO editDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return Json(await handler.Edit(editDTO), JsonRequestBehavior.AllowGet);

            }
            catch (HttpException)
            {
                return Json(ConstantHelper.ERROR, JsonRequestBehavior.AllowGet);
            }
        }

        //-> Paging
        [HttpPost]
        public async Task<ActionResult> Paging(SaleOrderFindDTO findDTO)
        {
            return PartialView(await handler.GetList(findDTO));
        }

        //-> Find 
        [HttpGet]
        public ActionResult Find()
        {
            return View();
        }

        public ActionResult TableRow()
        {
            return PartialView();
        }
    }
}