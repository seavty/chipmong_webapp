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
    
    public partial class tblUser
    {
        public int id { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedDate { get; set; }
        public Nullable<int> updatedBy { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Nullable<int> deleted { get; set; }
        public string password { get; set; }
        public string session { get; set; }
        public string userName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
    }
}
