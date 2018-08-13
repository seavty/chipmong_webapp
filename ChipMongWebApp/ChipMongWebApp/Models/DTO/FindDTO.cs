using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO
{
    public class FindDTO
    {
        public int currentPage { get; set; }
        public string orderBy { get; set; }
        public string orderDirection { get; set; }
    }
}