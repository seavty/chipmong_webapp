﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ChipMongEntities : DbContext
    {
        public ChipMongEntities()
            : base("name=ChipMongEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblCustomer> tblCustomers { get; set; }

        public System.Data.Entity.DbSet<ChipMongWebApp.Models.DTO.Customer.CustomerViewDTO> CustomerViewDTOes { get; set; }
    }
}
