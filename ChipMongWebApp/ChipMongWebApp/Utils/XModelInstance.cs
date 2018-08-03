using ChipMongWebApp.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Utils
{
    public sealed class XModelInstance
    {
        private static ChipMongEntities edmx = null;
        private static readonly object padlock = new object();

        private XModelInstance() { }

        public static ChipMongEntities Edmx
        {
            get
            {
                lock (padlock)
                {
                    if (edmx == null)
                    {
                        edmx = new ChipMongEntities();
                    }
                    return edmx;
                }
            }
        }
    }
}