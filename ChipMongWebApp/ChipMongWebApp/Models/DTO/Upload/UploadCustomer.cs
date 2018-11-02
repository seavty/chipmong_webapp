using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.Upload
{
    public class UploadCustomer
    {
        [Required]
        public HttpPostedFileBase ExcelFile { get; set; }
    }
}