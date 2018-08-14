using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.Item;
using ChipMongWebApp.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Dynamic;

namespace ChipMongWebApp.Utils.Handlers
{
    public class ItemHandler
    {
        private ChipMongEntities db = null;

        public ItemHandler()
        {
            db = new ChipMongEntities();
        }

        //-> SelectByID
        public async Task<ItemViewDTO> SelectByID(int id)
        {
            var record = await db.tblItems.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return MappingHelper.MapDBClassToDTO<tblItem, ItemViewDTO>(record);
        }

        //-> Create
        public async Task<ItemViewDTO> New(ItemNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var record = (tblItem)MappingHelper.MapDTOToDBClass<ItemNewDTO, tblItem>(newDTO, new tblItem());
            record.createdDate = DateTime.Now;
            db.tblItems.Add(record);

            await db.SaveChangesAsync();
            db.Entry(record).Reload();
            return await SelectByID(record.id);
        }

        //-> Edit
        public async Task<ItemViewDTO> Edit(ItemEditDTO editDTO)
        {
            var record = await db.tblItems.FirstOrDefaultAsync(x => x.deleted == null && x.id == editDTO.id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            editDTO = StringHelper.TrimStringProperties(editDTO);
            record = (tblItem)MappingHelper.MapDTOToDBClass<ItemEditDTO, tblItem>(editDTO, record);
            record.updatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(record.id);
        }

        //-> GetList
        public async Task<GetListDTO<ItemViewDTO>> GetList(ItemFindDTO findDTO)
        {
            /*
            //--seem like search sql not dynamic -> should write one helper function or interface to do dynamic search
            IQueryable<tblItem> records = from x in db.tblItems
                                                where x.deleted == null
                                                && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1 : x.code.Contains(findDTO.code))
                                                && (string.IsNullOrEmpty(findDTO.name) ? 1 == 1 : x.name.Contains(findDTO.name))
                                                orderby x.id ascending
                                                select x;
            return await Listing(findDTO.currentPage, records);
            */
            //--seem like search sql not dynamic -> should write one helper function or interface to do dynamic search
            IQueryable<tblItem> records = from x in db.tblItems
                                          where x.deleted == null
                                          && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1 : x.code.Contains(findDTO.code))
                                          && (string.IsNullOrEmpty(findDTO.name) ? 1 == 1 : x.name.Contains(findDTO.name))
                                          select x;
            return await Listing(findDTO.currentPage, records.AsQueryable().OrderBy($"{findDTO.orderBy} {findDTO.orderDirection}"));
        }

        //-> Listing
        private async Task<GetListDTO<ItemViewDTO>> Listing(int currentPage, IQueryable<tblItem> records, string search = null)
        {
            var recordList = new List<ItemViewDTO>();
            foreach (var record in PaginationHelper.GetList(currentPage, records))
            {
                recordList.Add(await SelectByID(record.id));
            }
            var getList = new GetListDTO<ItemViewDTO>();
            getList.metaData = await PaginationHelper.GetMetaData(currentPage, records, "id", "asc", search);
            getList.metaData.numberOfColumn = 4; // need to change number of column
            getList.items = recordList;
            return getList;
        }

        //-> Delete
        public async Task<Boolean> Delete(int id)
        {
            var record = await db.tblItems.FirstOrDefaultAsync(x => x.deleted == null && x.id == id); //-> item table
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var checkRecord = await db.tblSaleOrderItems.FirstOrDefaultAsync(x => x.deleted == null && x.itemID == id); //-> sale order item
            if (checkRecord != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Cannot this record because it is currently in used!");
            record.deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }
    }
}