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
    public class SubCustomerHandler
    {
        private readonly ChipMongEntities db;

        public SubCustomerHandler()
        {
            db = new ChipMongEntities();
            db.Database.CommandTimeout = ConstantHelper.DB_TIMEOUT;
        }

        //-> Delete
        public async Task<bool> Delete(int id)
        {
            var record = await db.tblSubCustomers.FirstOrDefaultAsync(x => x.sbcs_Deleted == null && x.sbcs_SubCustomerID == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.sbcs_Deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }
    }
}