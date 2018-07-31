using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.User
{
    public class UserBaseDTO
    {
        public int userID { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string session { get; set; }

    }
}