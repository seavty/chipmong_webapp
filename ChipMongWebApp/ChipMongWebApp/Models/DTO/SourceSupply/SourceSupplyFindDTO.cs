using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SourceSupply
{
    public class SourceSupplyFindDTO: FindDTO
    {
        [MaxLength(100)]
        [DisplayName("Name:")]
        public string name { get; set; }
    }
}