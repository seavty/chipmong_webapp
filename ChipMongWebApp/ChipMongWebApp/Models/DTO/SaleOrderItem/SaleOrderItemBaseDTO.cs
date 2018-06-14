using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrderItem
{
    public class SaleOrderItemBaseDTO
    {
        public int? id { get; set; }
        public double quantity { get; set; }
        public double price { get; set; }
    }
}