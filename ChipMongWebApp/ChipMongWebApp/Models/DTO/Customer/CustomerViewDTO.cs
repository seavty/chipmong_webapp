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
        [MaxLength(100)]
        [DisplayName("Code:")]
        public string code { get; set; }
    }
}