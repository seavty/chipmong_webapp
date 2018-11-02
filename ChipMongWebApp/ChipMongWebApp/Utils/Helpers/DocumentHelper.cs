using ChipMongWebApp.Models.DTO.SaleOrder;
using ChipMongWebApp.Models.DTO.Upload;
using ChipMongWebApp.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Utils.Helpers
{
    public class DocumentHelper
    {
        /*
        //-> SaveExcelFile
        public static bool SaveExcelFile(SaleOrderUploadExcel uploadExcel)
        {
            string path = "";
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month > 9 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month;

            path = ConstantHelper.UPLOAD_FOLDER + @"\" + year + @"\" + month;
            path = HttpContext.Current.Server.MapPath(@"~\" + path);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var file = path + @"\" + uploadExcel.ExcelFile.FileName;
            var fileName = uploadExcel.ExcelFile.FileName;
            
            if (File.Exists(file))
            {
                var currentDateIime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
                var fileWithoutExtension = fileName.Substring(0, fileName.Length - 5);
                uploadExcel.ExcelFile.SaveAs(path + @"\" + fileWithoutExtension + "-" + currentDateIime + ".xlsx");

            }
            else
                uploadExcel.ExcelFile.SaveAs(path + @"\" + fileName);
            return true;
        }
        */

        //->  SaveExcelFile
        public static bool SaveExcelFile(HttpPostedFileBase uploadExcel)
        {
            string path = "";
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month > 9 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month;

            path = ConstantHelper.UPLOAD_FOLDER + @"\" + year + @"\" + month;
            path = HttpContext.Current.Server.MapPath(@"~\" + path);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var file = path + @"\" + uploadExcel.FileName;
            var fileName = uploadExcel.FileName;

            if (File.Exists(file))
            {
                var currentDateIime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
                var fileWithoutExtension = fileName.Substring(0, fileName.Length - 5);
                uploadExcel.SaveAs(path + @"\" + fileWithoutExtension + "-" + currentDateIime + ".xlsx");

            }
            else
                uploadExcel.SaveAs(path + @"\" + fileName);
            return true;
        }
    }
}