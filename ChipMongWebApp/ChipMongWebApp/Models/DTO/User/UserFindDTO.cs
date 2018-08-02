using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.User
{
    public class UserFindDTO: FindDTO
    {

        [MaxLength(100)]
        [DisplayName("Login Name:")]
        public string userName { get; set; }

        [MaxLength(100)]
        [DisplayName("First Name:")]
        public string firstName { get; set; }

        [MaxLength(100)]
        [DisplayName("Last Name:")]
        public string lastName { get; set; }

    }
}