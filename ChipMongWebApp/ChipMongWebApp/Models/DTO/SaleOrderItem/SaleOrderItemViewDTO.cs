using ChipMongWebApp.Models.DTO.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrderItem
{
    public class SaleOrderItemViewDTO : SaleOrderItemBaseDTO
    {

        public double total { get; set; }

        public ItemViewDTO item { get; set; }
    }
}