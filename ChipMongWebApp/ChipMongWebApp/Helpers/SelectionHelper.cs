using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<SelectListItem> SaleOrderStatus()
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
            return listItems;
        }
    }
}