using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.User
{
    public class UserChangePasswordDTO
    {
        [Required]
        [MaxLength(100)]
        [DisplayName("Current Password (*):")]
        public string password { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("New Password (*):")]
        public string newPassword { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Comfirm Password (*):")]
        public string comfirmPassword { get; set; }
    }
}