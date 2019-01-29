using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO.Upload;
using ChipMongWebApp.Utils.Helpers;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ChipMongWebApp.Utils.Handlers
{
    public class UploadExcelHandler
    {
        private readonly ChipMongEntities db;
        public UploadExcelHandler()
        {
            db = new ChipMongEntities();
            db.Database.CommandTimeout = ConstantHelper.DB_TIMEOUT;

        }

        //-> UploadRetailer
        public async Task<int> UploadRetailer(UploadRetailer uploadExcel)
        {
            if (uploadExcel.ExcelFile.ContentLength < 0)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Not a valid file");

            if (!uploadExcel.ExcelFile.FileName.EndsWith(".xlsx"))
                throw new HttpException((int)HttpStatusCode.BadRequest, "Only .xlsx is allowed");

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    XLWorkbook Workbook = new XLWorkbook(uploadExcel.ExcelFile.InputStream);
                    IXLWorksheet WorkSheet = null;
                    WorkSheet = Workbook.Worksheet("sheet1");
                    if (!DocumentHelper.SaveExcelFile(uploadExcel.ExcelFile))
                        throw new HttpException((int)HttpStatusCode.BadRequest, "Error saving file.");

                    WorkSheet.FirstRow().Delete();//delete header column
                    int countUpdateRecord = 0;
                    foreach (var row in WorkSheet.RowsUsed())
                    {
                        //var soNumber = row.Cell(1).Value.ToString().Trim().Replace(" ", string.Empty);//Get ist cell. 1 represent column number
                        //var status = row.Cell(2).Value.ToString().Trim().Replace(" ", string.Empty); ;
                        int? custID = null;
                        var custCode = row.Cell(1).Value.ToString().Trim();
                        var cust = await db.tblCustomers.FirstOrDefaultAsync(x => x.deleted == null & x.code == custCode);
                        if (cust != null)
                            custID = cust.id;
                        string code = row.Cell(4).Value.ToString().ToLower().Trim();
                        var _record = await db.tblRetailers.FirstOrDefaultAsync(x => x.retl_Deleted == null 
                            && x.retl_Code.ToLower().Trim() == code &&
                            x.retl_CustomerID == cust.id);
                        if (_record == null)
                        {
                            var record = new tblRetailer();
                            record.retl_Name = row.Cell(5).Value.ToString().Trim();
                            record.retl_Code = row.Cell(4).Value.ToString().Trim();
                            record.retl_District = row.Cell(7).Value.ToString().Trim();
                            record.retl_Province = row.Cell(9).Value.ToString().Trim();
                            record.retl_Phone = row.Cell(10).Value.ToString().Trim();
                            record.retl_Phone2 = row.Cell(11).Value.ToString().Trim();
                            record.retl_CustomerID = custID;
                            db.tblRetailers.Add(record);
                            await db.SaveChangesAsync();
                            countUpdateRecord++;
                        }
                    }
                    transaction.Commit();
                    return countUpdateRecord;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        //-> UploadCustomer
        public async Task<int> UploadCustomer(UploadCustomer uploadExcel)
        {
            if (uploadExcel.ExcelFile.ContentLength < 0)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Not a valid file");

            if (!uploadExcel.ExcelFile.FileName.EndsWith(".xlsx"))
                throw new HttpException((int)HttpStatusCode.BadRequest, "Only .xlsx is allowed");

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    XLWorkbook Workbook = new XLWorkbook(uploadExcel.ExcelFile.InputStream);
                    IXLWorksheet WorkSheet = null;
                    WorkSheet = Workbook.Worksheet("sheet1");
                    if (!DocumentHelper.SaveExcelFile(uploadExcel.ExcelFile))
                        throw new HttpException((int)HttpStatusCode.BadRequest, "Error saving file.");

                    WorkSheet.FirstRow().Delete();//delete header column
                    int countUpdateRecord = 0;
                    foreach (var row in WorkSheet.RowsUsed())
                    {
                        //var soNumber = row.Cell(1).Value.ToString().Trim().Replace(" ", string.Empty);//Get ist cell. 1 represent column number
                        //var status = row.Cell(2).Value.ToString().Trim().Replace(" ", string.Empty); ;
                        var ex = await db.tblCustomers.FirstOrDefaultAsync(x => x.code.ToLower() == row.Cell(1).Value.ToString().Trim().ToLower() && x.deleted == null);
                        if (ex == null)
                        {
                            var record = new tblCustomer();
                            record.code = row.Cell(1).Value.ToString().Trim();
                            record.firstName = row.Cell(2).Value.ToString().Trim();
                            db.tblCustomers.Add(record);
                            await db.SaveChangesAsync();
                            countUpdateRecord++;
                        }
                    }
                    transaction.Commit();
                    return countUpdateRecord;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}