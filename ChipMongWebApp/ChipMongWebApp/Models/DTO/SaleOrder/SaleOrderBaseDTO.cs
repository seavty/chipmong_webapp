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

        [Required]
        [DisplayName("Source Supply: (*)")]
        public int sourceOfSupplyID { get; set; }
    }
}