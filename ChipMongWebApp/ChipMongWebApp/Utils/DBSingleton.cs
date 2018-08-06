using ChipMongWebApp.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChipMongWebApp.Utils
{
    public sealed class DBSingleton
    {
        private static ChipMongEntities db = null;
        private static readonly object padlock = new object();

        private DBSingleton() { }

        public static ChipMongEntities GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (db == null)
                    {
                        db = new ChipMongEntities();
                    }
                    return db;
                }
            }
        }
    }
}