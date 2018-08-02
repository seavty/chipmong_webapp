using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Helpers
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
    }
}