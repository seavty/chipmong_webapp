using ChipMongWebApp.Models.DTO.Upload;
using ChipMongWebApp.Utils.Attribute;
using ChipMongWebApp.Utils.Handlers;
using ChipMongWebApp.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    [Authentication]
    [ErrorLogger]
    public class UploadController : Controller
    {
        private readonly UploadExcelHandler handler;

        public UploadController()
        {
            handler = new UploadExcelHandler();
        }

        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        //-> Find 
        [HttpGet]
        public ActionResult Find() { return View(); }

        //-> UploadExcel
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<String> UploadRetailer(UploadRetailer uploadExcel)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return $"ok{await handler.UploadRetailer(uploadExcel)}";
            }
            catch (HttpException)
            {
                return "no";
            }
        }

        //->
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<String> UploadCustomer(UploadCustomer uploadExcel)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return $"ok{await handler.UploadCustomer(uploadExcel)}";
            }
            catch (HttpException)
            {
                return "no";
            }
        }
    }

    
}