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

namespace ChipMongWebApp.Utils.Handlers
{
    public class SourceSupplyHandler
    {
        private ChipMongEntities db = null;

        public SourceSupplyHandler()
        {
            //db = new ChipMongEntities();
            db = DBSingleton.GetInstance;
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
            //--seem like search sql not dynamic -> should write one helper function or interface to do dynamic search
            IQueryable<tblSourceOfSupply> records = from x in db.tblSourceOfSupplies
                                              where x.deleted == null &&
                                              (string.IsNullOrEmpty(findDTO.name) ? 1 == 1 : x.name.Contains(findDTO.name))
                                              orderby x.name ascending
                                              select x;
            return await Listing(findDTO.currentPage, records);
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

        //-> Listing
        public async Task<GetListDTO<SourceSupplyViewDTO>> Listing(int currentPage, IQueryable<tblSourceOfSupply> records, string search = null)
        {
            var recordList = new List<SourceSupplyViewDTO>();
            foreach (var record in PaginationHelper.GetList(currentPage, records))
            {
                recordList.Add(await SelectByID(record.id));
            }
            var getList = new GetListDTO<SourceSupplyViewDTO>();
            getList.metaData = await PaginationHelper.GetMetaData(currentPage, records, "id", "asc", search);
            getList.metaData.numberOfColumn = 2; // need to change number of column
            getList.items = recordList;
            return getList;
        }

    }
}