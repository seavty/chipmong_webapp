using ChipMongWebApp.Models.DTO.SaleOrder;
using ChipMongWebApp.Utils.Attribute;
using ChipMongWebApp.Utils.Handlers;
using ChipMongWebApp.Utils.Helpers;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    [Authentication]
    [ErrorLogger]
    public class SaleOrderController : Controller
    {
        private readonly SaleOrderHandler handler;

        public SaleOrderController()
        {
            handler = new SaleOrderHandler();
        }

        //-> New
        public ActionResult New()
        {
            var record = new SaleOrderViewDTO();
            record.date = DateTime.Now.ToShortDateString();
            record.requiredDate = DateTime.Now.ToShortDateString();
            record.mode = ConstantHelper.MODE_NEW;
            return View(record);
        }

        //-> New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> New(SaleOrderNewDTO newDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return Json(await handler.New(newDTO), JsonRequestBehavior.AllowGet);
            }
            catch (HttpException ex)
            {
                return Json(ConstantHelper.ERROR, JsonRequestBehavior.AllowGet);
            }
        }

        //-> View
        public async Task<ActionResult> View(int id)
        {
            var record = await handler.SelectByID(id);
            record.mode = ConstantHelper.MODE_VIEW;
            return View(record);
        }

        //-> Edit
        public async Task<String> unlockRec(int id)
        {
            return await handler.unlockRec(id);
        }

        public async Task<String> lockRec(int id)
        {
            return await handler.lockRec(id);
        }


        //-> Edit
        public async Task<ActionResult> Edit(int id)
        {
            var record = await handler.SelectByID(id,true);
            record.mode = ConstantHelper.MODE_EDIT;
            return View(record);
        }

        

        //-> Edit
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<String> qedit(int id,string p1,string p2,string p3, string p4, string p5, string p6, string p7, string p8)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return (await handler.QEdit(id, p1, p2, p3, p4, p5, p6, p7, p8));

            }
            catch (HttpException ex)
            {
                return ex.Message;
            }
        }
        //-> Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        //-> Find 
        [HttpGet]
        public ActionResult Find() {

            return View();
        }

        //-> Paging
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Paging(SaleOrderFindDTO findDTO) { return PartialView(await handler.GetList(findDTO)); }


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

        //-> UploadExcel
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<String> UploadExcel(SaleOrderUploadExcel uploadExcel)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return $"ok{await handler.UploadExcel(uploadExcel)}";
            }
            catch (HttpException)
            {
                return "no";
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<String> sendLine(string ids)
        {
            try
            {
                Response.StatusCode = 200;
                return await handler.sendLine(ids);
            }
            catch (HttpException)
            {
                return "no";
            }
        }
    }
}