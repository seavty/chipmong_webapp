using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderBaseDTO: ModeDTO
    {
        public int? id { get; set; }

        [Required]
        [DisplayName("Customer (*):")]
        public int customerID { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Status (*):")]
        public string status { get; set; }
   
        [MaxLength(1000)]
        [DisplayName("Remarks:")]
        public string remarks { get; set; }

        [MaxLength(100)]
        [DisplayName("Date: (*)")]
        public string date { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Require Date (*):")]
        public string requiredDate { get; set; }
        
        [DisplayName("Source Supply:")]
        public int? sourceOfSupplyID { get; set; }

        [DisplayName("Pickup:")]
        public string pickup { get; set; }

        [DisplayName("Truck No:")]
        public string slor_TruckNo { get; set; }

        [DisplayName("User :")]
        public int? slor_UserID { get; set; }

        [DisplayName("Doc No :")]
        public string slor_DocNo { get; set; }

        [DisplayName("ShipmentNo :")]
        public string slor_ShipmentNo { get; set; }

        [DisplayName("Updated Date :")]
        public string updatedDate { get; set; }

        [DisplayName("Updated By :")]
        public int? updatedBy { get; set; }


        [DisplayName("Truck Driver Phone No:")]
        public string slor_TruckDriverPhoneNo { get; set; }

        [DisplayName("Transportation Zone (District):")]
        public string slor_TransportZone { get; set; }
        [DisplayName("SO No:")]
        public string slor_SONo { get; set; }
        [DisplayName("CS. Shipping Condition:")]
        public string slor_ShipConidtion { get; set; }
        
    }
}