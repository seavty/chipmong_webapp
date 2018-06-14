using ChipMongWebApp.Models.DTO.SaleOrderItem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderNewDTO : SaleOrderBaseDTO
    {
        
        public List<SaleOrderItemNewDTO> items { get; set; }
    }
}