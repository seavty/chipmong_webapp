using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderExportExcel
    {
        [MaxLength(100)]
        [DisplayName("From Date:")]
        public string fromDate { get; set; }


        [MaxLength(100)]
        [DisplayName("To Date:")]
        public string toDate { get; set; }
    }
}