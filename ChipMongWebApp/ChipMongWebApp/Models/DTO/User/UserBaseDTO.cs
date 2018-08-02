﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.User
{
    public class UserBaseDTO
    {
        public int? userID { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Login Name (*):")]
        public string userName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("First Name (*):")]
        public string firstName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Last Name (*):")]
        public string lastName { get; set; }
    }
}