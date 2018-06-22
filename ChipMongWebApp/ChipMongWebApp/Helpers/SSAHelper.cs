﻿using ChipMongWebApp.Handlers;
using ChipMongWebApp.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Helpers
{
    public static class SSAHelper
    {
        public static List<SelectListItem> CustomerSSA(int id)
        {
            ChipMongEntities db = new ChipMongEntities();
            var record = db.tblCustomers.FirstOrDefault(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = $"{record.firstName} {record.lastName}",
                    Value = id.ToString()
                }
            };
        }
    }

}