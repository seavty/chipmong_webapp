using ChipMongWebApp.Models.DTO.Customer;
using ChipMongWebApp.Models.DTO.SaleOrderItem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderViewDTO : SaleOrderBaseDTO
    {

        [DisplayName("Total:")]
        public double total { get; set; }

        [DisplayName("Status:")]
        public string status { get; set; }

        [DisplayName("Remarks:")]
        public string remarks { get; set; }

        public CustomerViewDTO customer { get; set; }

        public List<SaleOrderItemViewDTO> items { get; set; }
    }
}