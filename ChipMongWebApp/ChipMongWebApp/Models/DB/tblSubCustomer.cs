//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChipMongWebApp.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSubCustomer
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
