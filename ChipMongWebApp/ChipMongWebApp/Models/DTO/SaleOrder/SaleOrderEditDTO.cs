using ChipMongWebApp.Models.DTO.SaleOrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderEditDTO : SaleOrderBaseDTO
    {

        public string deleteLineItemID { get; set; }
        public List<SaleOrderItemEditDTO> items { get; set; }
    }
}