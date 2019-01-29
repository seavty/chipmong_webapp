using ChipMongWebApp.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Utils.Helpers
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
        //-> User
        public static List<SelectListItem> user(int? id = null)
        {
            ChipMongEntities db = new ChipMongEntities();
            IQueryable<tblUser> records = from x in db.tblUsers
                                                    where x.deleted == null
                                                    orderby x.userName ascending
                                                    select x;
            var items = records.ToList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            
            foreach (var item in items)
            {

                listItems.Add(new SelectListItem()
                {
                    Text = item.userName,
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

        public static List<SelectListItem> profile(string value = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "Admin",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Normal",
                    Value = "2"
                }
            };

            if (!string.IsNullOrEmpty(value))
            {
                foreach (var item in listItems)
                {
                    if (item.Value.ToLower() == value.ToLower())
                    {
                        item.Selected = true;
                        break;
                    }

                }
            }
            return listItems;
        }

        //-> YesNo
        public static List<SelectListItem> YesNo(string value = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "Yes",
                    Value = "Y"
                },
                new SelectListItem
                {
                    Text = "No",
                    Value = "N"
                }
            };

            if (!string.IsNullOrEmpty(value))
            {
                foreach (var item in listItems)
                {
                    if (item.Value.ToLower() == value.ToLower())
                    {
                        item.Selected = true;
                        break;
                    }

                }
            }
            return listItems;
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
                    Text = "Processed",
                    Value = "Processed"
                },
                new SelectListItem
                {
                    Text = "Complete",
                    Value = "Complete"
                },
                new SelectListItem
                {
                    Text = "Cancelled",
                    Value = "Cancelled"
                },
                new SelectListItem
                {
                    Text = "Rejected",
                    Value = "Rejected"
                },
                new SelectListItem
                {
                    Text = "Insufficient balance",
                    Value = "Insufficient balance"
                },
                new SelectListItem
                {
                    Text = "Pending",
                    Value = "Pending"
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


        //-> Source Supply Selection
        public static List<SelectListItem> SourceSupplySelection(int? id = null)
        {
            ChipMongEntities db = new ChipMongEntities();
            IQueryable<tblSourceOfSupply> records = from x in db.tblSourceOfSupplies
                                          where x.deleted == null
                                          orderby x.name ascending
                                          select x;
            var items = records.ToList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Text = "--None--",
                Value = ""
            });
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

        // shipCondition
        public static List<SelectListItem> shipCondition(string id)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            listItems.Add(new SelectListItem()
            {
                Text = "P1",
                Value = "P1"
            });

            listItems.Add(new SelectListItem()
            {
                Text = "D1",
                Value = "D1"
            });

            listItems.Add(new SelectListItem()
            {
                Text = "D2",
                Value = "D2"
            });

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
        // Pickup Mode

        public static List<SelectListItem> pickupMode(string id)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Text = "Pickup",
                Value = "Y"
            });

            listItems.Add(new SelectListItem()
            {
                Text = "Delivery",
                Value = ""
            });

            /*
            if (id != null)
            {
                foreach (var item in listItems)
                {
                    if (item.Value.ToLower() == id.ToLower())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }*/
            if (id == null)
            {
                listItems[1].Selected = true;
            }
            else
            {
                if (id.ToLower() == "y")
                {
                    listItems[0].Selected = true;
                }
                else
                {
                    listItems[1].Selected = true;
                }
            }
            return listItems;
        }
    }
}