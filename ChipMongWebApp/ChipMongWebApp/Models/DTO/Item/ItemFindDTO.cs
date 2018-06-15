using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.Item
{
    public class ItemFindDTO : FindDTO
    {
        [MaxLength(100)]
        [DisplayName("Code:")]
        public string code { get; set; }

        [MaxLength(100)]
        [DisplayName("Name:")]
        public string name { get; set; }

    }
}