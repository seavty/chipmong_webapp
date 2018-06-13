using ChipMongWebApp.Handlers;
using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DTO.Customer;
using ChipMongWebApp.Models.DTO.SSA;
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

        //--> Create
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

        //-> SSA
        [HttpGet]
        public async Task<JsonResult> SSA()
        {
            /*
            var result = new SSAList<CustomerSSADTO>();
            var myList = new List<CustomerSSADTO>();
            for (int i = 0; i < 20; i++)
            {
                var item = new CustomerSSADTO();
                item.id = i;
                item.name = "name " + i;
                item.code = "code " + i;
                myList.Add(item);
            }
            
            result.results = myList;
            return Json(result, JsonRequestBehavior.AllowGet);
            */

            //return await handler.SSA(null);

            return  Json(await handler.SSA(Request.QueryString["q"]), JsonRequestBehavior.AllowGet);
        }
    }
}