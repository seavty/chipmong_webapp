using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ChipMongWebApp.Utils.Handlers
{
    public class RetailerHandler
    {
        private readonly ChipMongEntities db;

        public RetailerHandler()
        {
            db = new ChipMongEntities();
            db.Database.CommandTimeout = ConstantHelper.DB_TIMEOUT;
        }


        //-> Delete
        public async Task<bool> Delete(int id)
        {
            var record = await db.tblRetailers.FirstOrDefaultAsync(x => x.retl_Deleted == null && x.retl_RetailerID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.retl_Deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }
    }
}