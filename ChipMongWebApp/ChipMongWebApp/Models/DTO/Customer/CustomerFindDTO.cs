using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.Customer
{
    public class CustomerFindDTO : FindDTO
    {
        [MaxLength(100)]
        [DisplayName("Code:")]
        public string code { get; set; }

        [MaxLength(100)]
        [DisplayName("First Name:")]
        public string firstName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Last Name:")]
        public string lastName { get; set; }
        
        
    }
}