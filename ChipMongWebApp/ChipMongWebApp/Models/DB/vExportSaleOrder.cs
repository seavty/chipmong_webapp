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
    
    public partial class vExportSaleOrder
    {
        public int slor_SaleOrderID { get; set; }
        public Nullable<System.DateTime> slor_CreatedDate { get; set; }
        public Nullable<int> slor_CreatedBy { get; set; }
        public Nullable<System.DateTime> slor_UpdatedDate { get; set; }
        public Nullable<int> slor_UpdatedBy { get; set; }
        public string slor_Code { get; set; }
        public Nullable<int> slor_Deleted { get; set; }
        public Nullable<int> slor_CustomerID { get; set; }
        public Nullable<decimal> slor_Total { get; set; }
        public string slor_Status { get; set; }
        public string slor_Remarks { get; set; }
        public Nullable<System.DateTime> slor_Date { get; set; }
        public Nullable<int> slor_RetailerID { get; set; }
        public Nullable<int> slor_PreOrderID { get; set; }
        public Nullable<int> slor_SourceOfSupply { get; set; }
        public Nullable<System.DateTime> slor_RequiredDate { get; set; }
        public string slor_PickUp { get; set; }
        public string slor_TruckNo { get; set; }
        public Nullable<int> slor_UserID { get; set; }
        public Nullable<int> retl_RetailerID { get; set; }
        public string retl_Name { get; set; }
        public string retl_Address { get; set; }
        public Nullable<int> retl_Deleted { get; set; }
        public Nullable<int> retl_CustomerID { get; set; }
        public string retl_Code { get; set; }
        public string retl_Province { get; set; }
        public string retl_District { get; set; }
        public string retl_Phone { get; set; }
        public string retl_Phone2 { get; set; }
        public Nullable<int> scsp_SourceOfSupplyID { get; set; }
        public string scsp_Name { get; set; }
        public Nullable<int> scsp_Deleted { get; set; }
        public Nullable<int> cust_CustomerID { get; set; }
        public Nullable<System.DateTime> cust_CreatedDate { get; set; }
        public Nullable<int> cust_CreatedBy { get; set; }
        public Nullable<System.DateTime> cust_UpdatedDate { get; set; }
        public Nullable<int> cust_UpdatedBy { get; set; }
        public string cust_Code { get; set; }
        public string cust_FirstName { get; set; }
        public string cust_LastName { get; set; }
        public string cust_Phone1 { get; set; }
        public string cust_Phone2 { get; set; }
        public string cust_Password { get; set; }
        public string cust_Token { get; set; }
        public string cust_Email { get; set; }
        public string cust_Address { get; set; }
        public Nullable<int> cust_Deleted { get; set; }
        public string cust_LineID { get; set; }
        public string cust_LastCommand { get; set; }
        public string cust_LastCommandValue { get; set; }
        public string cust_LastCommandValue2 { get; set; }
        public string cust_Lang { get; set; }
        public string cust_LastCommandValue3 { get; set; }
        public Nullable<int> cust_EditPOID { get; set; }
        public Nullable<int> cust_EditSOID { get; set; }
        public Nullable<int> cust_cPage { get; set; }
        public Nullable<int> cust_UserID { get; set; }
        public string slor_DocNo { get; set; }
        public string slor_ShipmentNo { get; set; }
        public Nullable<int> slor_LockBy { get; set; }
        public Nullable<System.DateTime> slor_LockOn { get; set; }
        public string slor_TruckDriverPhoneNo { get; set; }
    }
}