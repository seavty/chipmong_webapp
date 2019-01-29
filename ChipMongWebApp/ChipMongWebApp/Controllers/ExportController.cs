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
                dt.Columns.AddRange(new DataColumn[52]
                {
                    new DataColumn("Purchase Order Date", typeof(string)),
                    new DataColumn("Purchase Order No.", typeof(string)),
                    new DataColumn("Sold To Name",typeof(string)),
                    new DataColumn("Ship To Name",typeof(string)),
                    new DataColumn("Transportation Zone (District)",typeof(string)),
                    new DataColumn("SO No",typeof(string)),
                    new DataColumn("Delivery No",typeof(string)),
                    new DataColumn("Shipment​",typeof(string)),
                    new DataColumn("Material Description​",typeof(string)),
                    new DataColumn("QTY​",typeof(string)),
                    new DataColumn("Cust. Shipping Condition​",typeof(string)),
                    new DataColumn("CS. Shipping Condition​",typeof(string)),
                    new DataColumn("Truck No​",typeof(string)),
                    new DataColumn("Phone number​",typeof(string)),
                    new DataColumn("Booking Date",typeof(string)),
                    new DataColumn("Plant",typeof(string)),
                    new DataColumn("Remark",typeof(string)),
                    new DataColumn("Processed by",typeof(string)),
                    new DataColumn("Date1",typeof(string)),
                    new DataColumn("Time1",typeof(string)),
                    new DataColumn("Send Date1",typeof(string)),
                    new DataColumn("Send Time1",typeof(string)),
                    new DataColumn("Completed by",typeof(string)),
                    new DataColumn("Date2",typeof(string)),
                    new DataColumn("Time2",typeof(string)),
                    new DataColumn("Send Date2",typeof(string)),
                    new DataColumn("Send Time2",typeof(string)),
                    new DataColumn("Cancalled by",typeof(string)),
                    new DataColumn("Date3",typeof(string)),
                    new DataColumn("Time3",typeof(string)),
                    new DataColumn("Send Date3",typeof(string)),
                    new DataColumn("Send Time3",typeof(string)),
                    new DataColumn("Rejected by",typeof(string)),
                    new DataColumn("Date4",typeof(string)),
                    new DataColumn("Time4",typeof(string)),
                    new DataColumn("Send Date4",typeof(string)),
                    new DataColumn("Send Time4",typeof(string)),
                    new DataColumn("Insufficient balance by",typeof(string)),
                    new DataColumn("Date5",typeof(string)),
                    new DataColumn("Time5",typeof(string)),
                    new DataColumn("Send Date5",typeof(string)),
                    new DataColumn("Send Time5",typeof(string)),
                    new DataColumn("Pending by",typeof(string)),
                    new DataColumn("Date6",typeof(string)),
                    new DataColumn("Time6",typeof(string)),
                    new DataColumn("Send Date6",typeof(string)),
                    new DataColumn("Send Time6",typeof(string)),

                    new DataColumn("Last change by",typeof(string)),
                    new DataColumn("Date7",typeof(string)),
                    new DataColumn("Time7",typeof(string)),
                    new DataColumn("Send Date7",typeof(string)),
                    new DataColumn("Send Time7",typeof(string)),
                });

                DateTime? fromDate = null;
                DateTime? toDate = null;
               
                    
                if(!string.IsNullOrEmpty(export.fromDate) && !string.IsNullOrEmpty(export.toDate))
                {
                   
                    fromDate = DateTime.ParseExact(export.fromDate, ConstantHelper.ddMMyyyy_DASH_SEPARATOR, CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(export.toDate, ConstantHelper.ddMMyyyy_DASH_SEPARATOR, CultureInfo.InvariantCulture);

                }

                ChipMongEntities db = new ChipMongEntities();

                var query =  db.vExportSaleOrders.AsNoTracking().Where(x => x.slor_Deleted == null);
                if (!string.IsNullOrEmpty(export.fromDate) && !string.IsNullOrEmpty(export.toDate))
                {
                    query = query.Where(x => DbFunctions.TruncateTime(x.slor_Date) >= DbFunctions.TruncateTime(fromDate) &&  DbFunctions.TruncateTime(x.slor_Date) <= DbFunctions.TruncateTime(toDate));
                }

                var records = await query.OrderBy(x => x.slor_Date).ToListAsync();
                foreach (var item in records)
                {
                    var tt = item.sloi_SaleOrderItemID;
                    var ttt = item.sloi_Quantity;

                   var slorDate = "";
                    var total = double.Parse(item.slor_Total.ToString()).ToString("0.00");
                    if (!string.IsNullOrEmpty(item.slor_Date.ToString()))
                        slorDate = item.slor_Date.ToString().ToHumanDate();

                    dt.Rows.Add(
                        slorDate,
                        item.slor_Code                               ,
                        $"{item.cust_FirstName} {item.cust_LastName}",
                        item.retl_Name,
                        item.retl_District                             , 
                        item.slor_SONo                             ,
                        item.slor_DocNo                            ,
                        item.slor_ShipmentNo,
                        
                        item.item_Name,//proudct
                        (item.sloi_Quantity == null ? "" : item.sloi_Quantity.Value.ToString("#,##0.00")),//qty

                        (string.IsNullOrEmpty(item.slor_PickUp) ? "Delivery" : "Pickup"),
                        item.slor_ShipConidtion,
                        item.slor_TruckNo,
                        item.slor_TruckDriverPhoneNo,
                        (item.slor_RequiredDate == null ? "" : item.slor_RequiredDate.ToString().ToHumanDate()),
                        item.slor_SourceOfSupply,
                        item.slor_Remarks,
                        item.user1,
                        (item.slor_Status1Date == null ? "" : item.slor_Status1Date.ToString().ToHumanDate()),
                        (item.slor_Status1Date == null ? "" : item.slor_Status1Date.ToString().ToHumanTime()),
                        (item.slor_Status1Send == null ? "" : item.slor_Status1Send.ToString().ToHumanDate()),
                        (item.slor_Status1Send == null ? "" : item.slor_Status1Send.ToString().ToHumanTime()),
                        item.user2,
                        (item.slor_Status2Date == null ? "" : item.slor_Status2Date.ToString().ToHumanDate()),
                        (item.slor_Status2Date == null ? "" : item.slor_Status2Date.ToString().ToHumanTime()),
                        (item.slor_Status2Send == null ? "" : item.slor_Status2Send.ToString().ToHumanDate()),
                        (item.slor_Status2Send == null ? "" : item.slor_Status2Send.ToString().ToHumanTime()),
                        item.user3,
                        (item.slor_Status3Date == null ? "" : item.slor_Status3Date.ToString().ToHumanDate()),
                        (item.slor_Status3Date == null ? "" : item.slor_Status3Date.ToString().ToHumanTime()),
                        (item.slor_Status3Send == null ? "" : item.slor_Status3Send.ToString().ToHumanDate()),
                        (item.slor_Status3Send == null ? "" : item.slor_Status3Send.ToString().ToHumanTime()),
                        item.user4,
                        (item.slor_Status4Date == null ? "" : item.slor_Status4Date.ToString().ToHumanDate()),
                        (item.slor_Status4Date == null ? "" : item.slor_Status4Date.ToString().ToHumanTime()),
                        (item.slor_Status4Send == null ? "" : item.slor_Status4Send.ToString().ToHumanDate()),
                        (item.slor_Status4Send == null ? "" : item.slor_Status4Send.ToString().ToHumanTime()),
                        item.user5,
                        (item.slor_Status5Date == null ? "" : item.slor_Status1Date.ToString().ToHumanDate()),
                        (item.slor_Status5Date == null ? "" : item.slor_Status5Date.ToString().ToHumanTime()),
                        (item.slor_Status5Send == null ? "" : item.slor_Status5Send.ToString().ToHumanDate()),
                        (item.slor_Status5Send == null ? "" : item.slor_Status5Send.ToString().ToHumanTime()),
                        item.user6,
                        (item.slor_Status6Date == null ? "" : item.slor_Status6Date.ToString().ToHumanDate()),
                        (item.slor_Status6Date == null ? "" : item.slor_Status6Date.ToString().ToHumanTime()),
                        (item.slor_Status6Send == null ? "" : item.slor_Status6Send.ToString().ToHumanDate()),
                        (item.slor_Status6Send == null ? "" : item.slor_Status6Send.ToString().ToHumanTime()),
                        item.editBy,
                        (item.slor_UpdatedDate == null ? "" : item.slor_UpdatedDate.ToString().ToHumanDate()),
                        (item.slor_UpdatedDate == null ? "" : item.slor_UpdatedDate.ToString().ToHumanTime()),
                        (item.slor_TimeUpdateStatus == null ? "" : item.slor_TimeUpdateStatus.ToString().ToHumanDate()),
                        (item.slor_TimeUpdateStatus == null ? "" : item.slor_TimeUpdateStatus.ToString().ToHumanTime())
                        );
                }

                //Exporting to Excel
                string folderPath = Server.MapPath("~/uploads/exp/"); //"C:\\SaleOrder-Export\\";
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
                    Response.Clear();
                    Response.Write(fileName);
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