using ChipMongWebApp.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Utils.Extension
{
    public static class StringExtension
    {
        //private static readonly string SOURCE_DATE_FORMAT = "dd/MM/yyyy";
        //private static readonly string DESTINATION_DATE_FORMAT = "yyyy-MM-dd";

        public static string ToDBDate(this string value)
        {
            DateTime dateTime = DateTime.ParseExact(value, ConstantHelper.ddMMyyyy_DASH_SEPARATOR, CultureInfo.InvariantCulture);
            return dateTime.ToString(ConstantHelper.yyyyMMd_DASH_SEPARATOR);
        }

        public static string ToDisplayDate(this string value)
        {
            if (value == null) return "";
            DateTime dateTime = DateTime.ParseExact(value.Split(' ')[0], CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture);
            return dateTime.ToString(ConstantHelper.ddMMyyyy_DASH_SEPARATOR);
        }

        public static string ToHumanDate(this string value)
        {
            /*
            DateTime dateTime = DateTime.ParseExact(value, ConstantHelper.yyyyMMd_DASH_SEPARATOR, CultureInfo.InvariantCulture);
            return dateTime.ToString(ConstantHelper.ddMMyyyy_FORWARD_SLASH_SEPARATOR);
            */
            var tmp = CultureInfo.CurrentCulture.DateTimeFormat.ToString();
            var another = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            var systemDateTimeFormat = $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} {CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern}";

            DateTime dateTime = DateTime.Parse(value);
            return dateTime.ToString(ConstantHelper.ddMMyyyy_DASH_SEPARATOR);
            /*
            DateTime dateTime = DateTime.ParseExact(value, systemDateTimeFormat, CultureInfo.InvariantCulture);
            return dateTime.ToString(ConstantHelper.ddMMyyyy_FORWARD_SLASH_SEPARATOR);
            */
        }

        public static string ToHumanDateTime(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            /*
            DateTime dateTime = DateTime.ParseExact(value, ConstantHelper.yyyyMMd_DASH_SEPARATOR, CultureInfo.InvariantCulture);
            return dateTime.ToString(ConstantHelper.ddMMyyyy_FORWARD_SLASH_SEPARATOR);
            */
            var tmp = CultureInfo.CurrentCulture.DateTimeFormat.ToString();
            var another = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            var systemDateTimeFormat = $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} {CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern}";

            DateTime dateTime = DateTime.Parse(value);
            return dateTime.ToString(ConstantHelper.ddMMyyyyHHMM_DASH_SEPARATOR);
            /*
            DateTime dateTime = DateTime.ParseExact(value, systemDateTimeFormat, CultureInfo.InvariantCulture);
            return dateTime.ToString(ConstantHelper.ddMMyyyy_FORWARD_SLASH_SEPARATOR);
            */
        }
    }
}