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
        [DisplayName("Customer:")]
        public string customer { get; set; }
    }
}