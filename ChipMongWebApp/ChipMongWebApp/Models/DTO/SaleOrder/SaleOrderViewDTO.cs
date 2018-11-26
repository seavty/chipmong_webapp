using ChipMongWebApp.Models.DTO.Customer;
using ChipMongWebApp.Models.DTO.Item;
using ChipMongWebApp.Models.DTO.SaleOrderItem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderViewDTO : SaleOrderBaseDTO
    {
        [MaxLength(100)]
        [DisplayName("Code:")]
        public string code { get; set; }

        [DisplayName("Total:")]
        public double total { get; set; }

        /*
        [DisplayName("Status:")]
        public string status { get; set; }
        */

        public CustomerViewDTO customer { get; set; }

        public List<SaleOrderItemViewDTO> items { get; set; }

        public string sourceOfSupply { get; set; }
        public string shipMode { get; set; }
        public string product { get; set; }


        

        //public List<ItemViewDTO> itemSelection { get; set; }
    }
}