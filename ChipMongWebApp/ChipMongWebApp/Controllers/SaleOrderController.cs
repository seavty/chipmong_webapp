using ChipMongWebApp.Handlers;
using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DTO.SaleOrder;
using ChipMongWebApp.Utils.Attribute;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    [ErrorLogger]
    public class SaleOrderController : Controller
    {
        private SaleOrderHandler handler = null;

        public SaleOrderController() { handler = new SaleOrderHandler(); }

        //-> New
        public ActionResult New() { return View(); }

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
            catch (HttpException)
            {
                return Json(ConstantHelper.ERROR, JsonRequestBehavior.AllowGet);
            }
        }

        //-> View
        public async Task<ActionResult> View(int id) { return View(await handler.SelectByID(id, ConstantHelper.MODE_VIEW)); }

        //-> Edit
        public async Task<ActionResult> Edit(int id) { return View(await handler.SelectByID(id, ConstantHelper.MODE_EDIT)); }

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
        public ActionResult Find() { return View(); }

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

        //-> Edit
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> UploadExcel(SaleOrderUploadExcel uploadExcel)
        {
            if (ModelState.IsValid)
            {

                if (uploadExcel.ExcelFile.ContentLength > 0)
                {
                    if (uploadExcel.ExcelFile.FileName.EndsWith(".xlsx") || uploadExcel.ExcelFile.FileName.EndsWith(".xls"))
                    {
                        XLWorkbook Workbook;
                        try//incase if the file is corrupt
                            {
                            Workbook = new XLWorkbook(uploadExcel.ExcelFile.InputStream);
                        }
                            catch (Exception ex)
                        {
                            ModelState.AddModelError(String.Empty, $"Check your file. {ex.Message}");
                            return null;
                        }
                        IXLWorksheet WorkSheet = null;
                        try//incase if the sheet you are looking for is not found
                            {
                            WorkSheet = Workbook.Worksheet("sheet1");

                        }
                            catch
                        {
                            ModelState.AddModelError(String.Empty, "sheet not found!");
                            return null;
                        }
                        WorkSheet.FirstRow().Delete();//if you want to remove ist row

                        foreach (var row in WorkSheet.RowsUsed())
                        {
                            //do something here
                            var soNumber = row.Cell(1).Value.ToString();//Get ist cell. 1 represent column number
                            var soStatus = row.Cell(2).Value.ToString();

                        }
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Only .xlsx and .xls files are allowed");
                        return null;
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Not a valid file");
                    return null;
                }
            }
            return null;
        }

    }
}