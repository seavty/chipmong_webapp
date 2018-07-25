using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SourceSupply
{
    public class SourceSupplyBaseDTO: ModeDTO
    {
        public int? id { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Name (*):")]
        public string name { get; set; }
    }
}