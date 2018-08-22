using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.SourceSupply;
using ChipMongWebApp.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Dynamic;
using ChipMongWebApp.Utils.Extension;

namespace ChipMongWebApp.Utils.Handlers
{
    public class SourceSupplyHandler
    {
        private readonly ChipMongEntities db;

        public SourceSupplyHandler()
        {
            db = new ChipMongEntities();
            db.Database.CommandTimeout = ConstantHelper.DB_TIMEOUT;
        }

        //-> SelectByID
        public async Task<SourceSupplyViewDTO> SelectByID(int id)
        {
            var record = await db.tblSourceOfSupplies.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return MappingHelper.MapDBClassToDTO<tblSourceOfSupply, SourceSupplyViewDTO>(record);
        }

        //-> New
        public async Task<SourceSupplyViewDTO> New(SourceSupplyNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var record = (tblSourceOfSupply)MappingHelper.MapDTOToDBClass<SourceSupplyNewDTO, tblSourceOfSupply>(newDTO, new tblSourceOfSupply());
            //record.createdDate = DateTime.Now;
            db.tblSourceOfSupplies.Add(record);
            await db.SaveChangesAsync();
            db.Entry(record).Reload();
            return await SelectByID(record.id);
        }

        //-> Edit
        public async Task<SourceSupplyViewDTO> Edit(SourceSupplyEditDTO editDTO)
        {
            var record = await db.tblSourceOfSupplies.FirstOrDefaultAsync(x => x.deleted == null && x.id == editDTO.id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            editDTO = StringHelper.TrimStringProperties(editDTO);
            record = (tblSourceOfSupply)MappingHelper.MapDTOToDBClass<SourceSupplyEditDTO, tblSourceOfSupply>(editDTO, record);
            //record.updatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(record.id);
        }

        //-> GetList
        public async Task<GetListDTO<SourceSupplyViewDTO>> GetList(SourceSupplyFindDTO findDTO)
        {

            /*
            IQueryable<tblSourceOfSupply> records = from x in db.tblSourceOfSupplies
                                                    where x.deleted == null &&
                                                    (string.IsNullOrEmpty(findDTO.name) ? 1 == 1 : x.name.Contains(findDTO.name))
                                                    select x;
            return await Listing(findDTO.currentPage, records.AsQueryable().OrderBy($"{findDTO.orderBy} {findDTO.orderDirection}"));
            */

            IQueryable<tblSourceOfSupply> query = db.tblSourceOfSupplies.Where(x => x.deleted == null);

            if (!string.IsNullOrEmpty(findDTO.name)) query = query.Where(x => x.name.StartsWith(findDTO.name));
            query = query.AsQueryable().OrderBy($"{findDTO.orderBy} {findDTO.orderDirection}");

            return await ListingHandler(findDTO.currentPage, query);
        }

        //-> ListingHandler
        private async Task<GetListDTO<SourceSupplyViewDTO>> ListingHandler(int currentPage, IQueryable<tblSourceOfSupply> query)
        {
            int totalRecord = await query.CountAsync();
            List<tblSourceOfSupply> records = await query.Page(currentPage).ToListAsync();

            var myList = new List<SourceSupplyViewDTO>();
            foreach (var record in records)
            {
                myList.Add(await SelectByID(record.id));
            }
            var getList = new GetListDTO<SourceSupplyViewDTO>();
            getList.metaData = PaginationHelper.MyTestGetMetaData(currentPage, totalRecord);
            getList.metaData.numberOfColumn = 2; // need to change number of column
            getList.items = myList;
            return getList;
        }



        //-> Delete
        public async Task<Boolean> Delete(int id)
        {
            var record = await db.tblSourceOfSupplies.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var checkRecord = await db.tblSaleOrders.FirstOrDefaultAsync(x => x.deleted == null && x.sourceOfSupplyID == id);
            if (checkRecord != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Cannot this record because it is currently in used!");

            var checkDealerSourceSupply = await db.tblDealerSourceOfSupplies.FirstOrDefaultAsync(x => x.deleted == null && x.sourceOfSupplyID == id);
            if (checkDealerSourceSupply != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Cannot this record because it is currently in used!");

            record.deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }
    }
}