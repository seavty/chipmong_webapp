using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.Customer
{
    public class CustomerBaseDTO: ModeDTO
    {
        [Required]
        [MaxLength(100)]
        [DisplayName("First Name (*):")]
        public string firstName  { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Last Name (*):")]
        public string lastName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Phone1 (*):")]
        public string phone1 { get; set; }

        
        [MaxLength(100)]
        [DisplayName("Phone2:")]
        public string phone2 { get; set; }

        
        [MaxLength(100)]
        [DisplayName("E-mail:")]
        public string email { get; set; }

        
        [MaxLength(100)]
        [DisplayName("Address:")]
        public string address { get; set; }

        [DisplayName("User:")]
        public int? cust_UserID { get; set; }
    }
}