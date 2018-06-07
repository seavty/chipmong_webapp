using ChipMongWebApp.Handlers;
using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerHandler handler = null;

        public CustomerController()
        {
            handler = new CustomerHandler();
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Customer
        public ActionResult Create()
        {
            return View();
        }

        //-> Create
        [HttpPost]
        public async Task<JsonResult> Create(CustomerNewDTO customer)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return Json(await handler.Create(customer), JsonRequestBehavior.AllowGet);

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
        public async Task<JsonResult> Edit(CustomerViewDTO customer)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return Json(await handler.Edit(customer), JsonRequestBehavior.AllowGet);

            }
            catch (HttpException)
            {
                return Json(ConstantHelper.ERROR, JsonRequestBehavior.AllowGet);
            }
        }

        //-> Paging
        [HttpPost]
        public async Task<ActionResult> Paging(CustomerFindDTO findDTO)
        {
            return PartialView(await handler.GetList(findDTO));
        }

        //-> Find 
        [HttpGet]
        public ActionResult Find()
        {
            return View();
        }




    }
}