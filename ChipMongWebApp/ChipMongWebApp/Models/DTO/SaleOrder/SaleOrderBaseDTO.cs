using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderBaseDTO
    {
        public int? id { get; set; }

        [Required]
        [DisplayName("Customer (*):")]
        public int customerID { get; set; }
    }
}