using ChipMongWebApp.Models.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ChipMongWebApp.Utils.Helpers
{
    public static class ConstantHelper
    {
        public static readonly int TABLE_ACCOUNT_ID = 1;

        public static readonly string ALREADY_REQUEST_LOAN = "You already requested a loan!";
        public static readonly string KEY_IN_REQUIRED_FIELD = "Please key in all required field(s)!";

        public static readonly int ONE_EQUAL_ONE = 1;

        public static readonly string ERROR = "Error occured while processing your request...";


        public static readonly int MODE_NEW = 1;
        public static readonly int MODE_EDIT = 2;
        public static readonly int MODE_VIEW = 3;

        public static readonly string UPLOAD_FOLDER = "uploads";

        public static readonly string LOGIN_NAME_EXIST = "This login name already exists!";


        public static readonly string INCORRECT_PASSWORD = "Incorrect Password!";

        public static readonly string PASSWORD_DOES_NOT_MATCH = "New password and comfirm password do not match";


        public static readonly string INCORRECT_USER_NAME_OR_PASSWORD = "Incorrect user name or password!";


        public static readonly string CUSTOMER_CONTROLLER = "/customer";
        public static readonly string ITEM_CONTROLLER = "/item";
        public static readonly string SOURCE_SUPPLY_CONTROLLER = "/sourcesupply";
        public static readonly string USER_CONTROLLER = "/user";
        public static readonly string SALEORDER_CONTROLLER = "/saleorder";
        public static readonly string AUTH_CONTROLLER = "/auth";

        public static readonly string yyyyMMd_DASH_SEPARATOR = "yyyy-MM-dd";
        public static readonly string ddMMyyyy_DASH_SEPARATOR = "dd-MM-yyyy";

        public static readonly string ASC = "ASC";
        public static readonly string DESC = "DESC";

    }
}