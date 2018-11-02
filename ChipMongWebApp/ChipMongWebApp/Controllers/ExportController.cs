using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO.SaleOrder;
using ChipMongWebApp.Utils.Extension;
using ChipMongWebApp.Utils.Helpers;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    public class ExportController : Controller
    {
        // GET: Export
        public ActionResult Index()
        {
            return View();
        }

        //-> Find 
        [HttpGet]
        public ActionResult Export()
        {
            return View();
        }


        [HttpPost]
        public async Task<string> Export(SaleOrderExportExcel export)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[6]
                {
                    new DataColumn("SaleOrder Code", typeof(string)),
                    new DataColumn("Date", typeof(string)),
                    new DataColumn("Customer Name",typeof(string)),
                    new DataColumn("Source Supply Name",typeof(string)),
                    new DataColumn("Status",typeof(string)),
                    new DataColumn("Total",typeof(string)),

                });

                DateTime? fromDate = null;
                DateTime? toDate = null;
               
                    
                if(!string.IsNullOrEmpty(export.fromDate) && !string.IsNullOrEmpty(export.toDate))
                {
                   
                    fromDate = DateTime.ParseExact(export.fromDate, ConstantHelper.ddMMyyyy_DASH_SEPARATOR, CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(export.toDate, ConstantHelper.ddMMyyyy_DASH_SEPARATOR, CultureInfo.InvariantCulture);

                }

                ChipMongEntities db = new ChipMongEntities();

                var query =  db.vExportSaleOrders.Where(x => x.slor_Deleted == null);
                if (!string.IsNullOrEmpty(export.fromDate) && !string.IsNullOrEmpty(export.toDate))
                {
                    query = query.Where(x => DbFunctions.TruncateTime(x.slor_Date) >= DbFunctions.TruncateTime(fromDate) &&  DbFunctions.TruncateTime(x.slor_Date) <= DbFunctions.TruncateTime(toDate));
                }

                var records = await query.OrderBy(x => x.slor_Date).ToListAsync();
                foreach (var item in records)
                {

                   var slorDate = "";
                    var total = double.Parse(item.slor_Total.ToString()).ToString("0.00");
                    if (!string.IsNullOrEmpty(item.slor_Date.ToString()))
                        slorDate = item.slor_Date.ToString().ToHumanDate();

                    dt.Rows.Add(
                        item.slor_Code                               ,
                        slorDate                                     , 
                        $"{item.cust_FirstName} {item.cust_LastName}", 
                        item.scsp_Name                               , 
                        item.slor_Status                             ,
                        total
                        );
                }
                
                //Exporting to Excel
                string folderPath = "C:\\SaleOrder-Export\\";
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                
                //Codes for the Closed XML
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "SaleOrder");

                    var fileName = $"SaleOrder-Export-{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff")}.xlsx";
                    wb.SaveAs(folderPath + fileName);

                    var stream = GetStream(wb);// The method is defined below
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }


            return "ok";
        }

        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
    }
}