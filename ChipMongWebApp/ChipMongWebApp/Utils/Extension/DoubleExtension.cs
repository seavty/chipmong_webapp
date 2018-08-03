using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Extension
{
    public static class DoubleExtension
    {
        public static string ToTwoDecimalPoint(this double value)
        {
            return value.ToString("0.00");
        }
        
    }
}