using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Extension
{
    public static class StringExtension
    {
        private static readonly string SOURCE_DATE_FORMAT = "dd/MM/yyyy";
        private static readonly string DESTINATION_DATE_FORMAT = "yyyy-MM-dd";

        public static string ToDBDate(this string value)
        {
            DateTime dateTime = DateTime.ParseExact(value, SOURCE_DATE_FORMAT, CultureInfo.InvariantCulture);
            return dateTime.ToString(DESTINATION_DATE_FORMAT);
        }

        public static string ToDisplayDate(this string value)
        {
            DateTime dateTime = DateTime.ParseExact(value.Split(' ')[0], CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.InvariantCulture);
            return dateTime.ToString(SOURCE_DATE_FORMAT);
        }
    }
}