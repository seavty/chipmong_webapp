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
        [MaxLength(100)]
        [DisplayName("Code:")]
        public string code { get; set; }
        public List<SaleOrderItemNewDTO> items { get; set; }
    }
}