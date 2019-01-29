using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.Retailer
{
    public class RetailerDTO
    {
        public int retl_RetailerID { get; set; }
        public string retl_Name { get; set; }
        public string retl_Address { get; set; }
        public int? retl_Deleted { get; set; }
        public int? retl_CustomerID { get; set; }
        public string retl_Code { get; set; }
        public string retl_Province { get; set; }
        public string retl_District { get; set; }
        public string retl_Phone { get; set; }
        public string retl_Phone2 { get; set; }

    }
}