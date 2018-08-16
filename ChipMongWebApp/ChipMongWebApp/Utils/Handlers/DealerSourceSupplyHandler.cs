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
    public class DealerSourceSupplyHandler
    {
        private ChipMongEntities db = null;

        public DealerSourceSupplyHandler()
        {
            db = new ChipMongEntities();
            db.Database.CommandTimeout = ConstantHelper.DB_TIMEOUT;
        }

        //-> Edit
        public async Task<string> Edit(int id, int sourceSupplyID)
        {
            var record = await db.tblDealerSourceOfSupplies.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.sourceOfSupplyID = sourceSupplyID;
            await db.SaveChangesAsync();
            return "ok";
        }


        //-> Delete
        public async Task<Boolean> Delete(int id)
        {
            var record = await db.tblDealerSourceOfSupplies.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }
    }
}