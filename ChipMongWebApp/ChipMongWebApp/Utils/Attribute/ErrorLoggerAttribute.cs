using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using System.Web.Mvc;

namespace ChipMongWebApp.Utils.Attribute
{
    public class ErrorLoggerAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext);
            base.OnException(filterContext);
        }

        public void LogError(ExceptionContext filterContext)
        {
            StringBuilder builder = new StringBuilder();
            builder
                .AppendLine($"****************** {DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss")} *******************")
                .AppendFormat($"Source:\t {filterContext.Exception.Source}")
                .AppendLine()
                .AppendFormat($"Target:\t {filterContext.Exception.TargetSite}")
                .AppendLine()
                .AppendFormat($"Type:\t {filterContext.Exception.GetType().Name}")
                .AppendLine()
                .AppendFormat($"Message:\t {filterContext.Exception.Message}")
                .AppendLine()
                .AppendFormat($"Stack:\t {filterContext.Exception.StackTrace}")
                .AppendLine()
                .AppendLine("======================================================================================================")
                .AppendLine()
                .AppendLine();

            string path = "";
            string folderName = "logs";
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month > 9 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month;
            string day = DateTime.Now.Day > 9 ? DateTime.Now.Day.ToString() : "0" + DateTime.Now.Day;

            path = folderName + @"\" + year + @"\" + month;
            path = HttpContext.Current.Server.MapPath(@"~\" + path);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = day + ".log";
            using (StreamWriter writer = File.AppendText(path + @"\" + fileName))
            {
                writer.Write(builder.ToString());
                writer.Flush();
            }
        }
    }
}