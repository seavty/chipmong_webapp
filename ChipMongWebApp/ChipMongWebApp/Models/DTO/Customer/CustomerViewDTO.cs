using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.Customer
{
    public class CustomerViewDTO : CustomerNewDTO
    {
        public int? id { get; set; }

        [MaxLength(100)]
        [DisplayName("Code:")]
        public string code { get; set; }
    }
}