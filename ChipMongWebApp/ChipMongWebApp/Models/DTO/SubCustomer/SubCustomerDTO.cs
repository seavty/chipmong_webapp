using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SubCustomer
{
    public class SubCustomerDTO
    {
        public int sbcs_SubCustomerID { get; set; }
        public Nullable<System.DateTime> sbcs_CreatedDate { get; set; }
        public Nullable<int> sbcs_CreatedBy { get; set; }
        public Nullable<System.DateTime> sbcs_UpdatedDate { get; set; }
        public Nullable<int> sbcs_UpdatedBy { get; set; }
        public string sbcs_Phone { get; set; }
        public Nullable<int> sbcs_CustomerID { get; set; }
        public string sbcs_LineID { get; set; }
        public Nullable<int> sbcs_Deleted { get; set; }
    }
}