using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.Item
{
    public class ItemBaseDTO: ModeDTO
    {
        [Required]
        [MaxLength(100)]
        [DisplayName("Name (*):")]
        public string name { get; set; }

        [DisplayName("Price (*):")]
        public double price { get; set; }
    }
}