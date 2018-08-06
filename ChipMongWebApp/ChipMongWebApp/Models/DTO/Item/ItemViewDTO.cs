using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.Item
{
    public class ItemViewDTO : ItemBaseDTO
    {
        public int id { get; set; }
        [DisplayName("Code")]
        public string code { get; set; }
    }
}