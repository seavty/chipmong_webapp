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
    
    public partial class tblCustomer
    {
        public int id { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedDate { get; set; }
        public Nullable<int> updatedBy { get; set; }
        public string code { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string password { get; set; }
        public string token { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public Nullable<int> deleted { get; set; }
        public string cust_LineID { get; set; }
        public string cust_LastCommand { get; set; }
        public string cust_LastCommandValue { get; set; }
        public string cust_LastCommandValue2 { get; set; }
        public string cust_Lang { get; set; }
    }
}
