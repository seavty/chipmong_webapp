using ChipMongWebApp.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Models.DTO.SaleOrder
{
    public class SaleOrderFindResultDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public DateTime? date { get; set; }
        public string firstName { get; set; }
        public decimal? total { get; set; }
        public string status { get; set; }
    }
}