using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderFindDTO : FindDTO
    {
        [MaxLength(100)]
        [DisplayName("Code:")]
        public string code { get; set; }

        [MaxLength(100)]
        [DisplayName("Customer (First Name):")]
        public string customer { get; set; }

        [DisplayName("Status:")]
        public string status { get; set; }

        [DisplayName("CustomerID:")]
        public int customerID { get; set; }

        [DisplayName("Request Date From:")]
        public string slor_RequiredDate_From { get; set; }

        [DisplayName("Request Date To :")]
        public string slor_RequiredDate_To { get; set; }


        public string slor_SourceOfSupply { get; set; }

        [DisplayName("Truck No:")]
        public string slor_TruckNo { get; set; }

        [DisplayName("Doc No:")]
        public string slor_DocNo { get; set; }

        [DisplayName("Shipment No:")]
        public string slor_ShipmentNo { get; set; }


    }
}