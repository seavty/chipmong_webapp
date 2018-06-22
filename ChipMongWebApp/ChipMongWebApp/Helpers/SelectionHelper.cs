using ChipMongWebApp.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Helpers
{
    public static class SelectionHelper
    {
        public static string AccountStatusCaption(string status)
        {
            switch (status)
            {
                case "SubmitForApproval":
                    return "Submit For Approval";
                case "Pending":
                case "Approved":
                case "Rejected":
                    return status;
                default:
                    return "";
            }
        }

        //-> SaleOrderStatus
        public static List<SelectListItem> SaleOrderStatus(string value = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "New",
                    Value = "New"
                },
                new SelectListItem
                {
                    Text = "In Progress",
                    Value = "InProgress"
                },
                new SelectListItem
                {
                    Text = "Completed",
                    Value = "Completed"
                }
            };

            if (!string.IsNullOrEmpty(value))
            {
                foreach (var item in listItems)
                {
                    if (item.Value == value)
                    {
                        item.Selected = true;
                        break;
                    }

                }
            }
            return listItems;
        }

        //-> ItemSelection
        public static List<SelectListItem> ItemSelection(int? id = null)
        {
            ChipMongEntities db = new ChipMongEntities();
            IQueryable<tblItem> records = from x in db.tblItems
                                          where x.deleted == null
                                          orderby x.id ascending
                                          select x;
            var items = records.ToList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var item in items)
            {

                listItems.Add(new SelectListItem()
                {
                    Text = item.name,
                    Value = item.id.ToString()
                });

            }

            if (id != null)
            {
                foreach (var item in listItems)
                {
                    if (item.Value == id.ToString())
                    {
                        item.Selected = true;
                        break;
                    }

                }
            }
            return listItems;
        }


    }
}