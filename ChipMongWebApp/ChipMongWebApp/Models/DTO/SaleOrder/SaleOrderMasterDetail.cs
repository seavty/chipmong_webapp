using ChipMongWebApp.Models.DTO.SaleOrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderMasterDetail : SaleOrderNewDTO
    {
        public List<SaleOrderItemNewDTO> items { get; set; }
    }
}