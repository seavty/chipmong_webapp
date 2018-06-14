using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO.Item;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ChipMongWebApp.Handlers
{
    public class ItemHandler
    {
        private ChipMongEntities db = null;

        public ItemHandler() { db = new ChipMongEntities(); }

        //-> SelectByID
        public async Task<ItemViewDTO> SelectByID(int id)
        {
            var record = await db.tblItems.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return MappingHelper.MapDBClassToDTO<tblItem, ItemViewDTO>(record);
        }

        //-> Create
        public async Task<ItemViewDTO> Create(ItemNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var record = (tblItem)MappingHelper.MapDTOToDBClass<ItemNewDTO, tblItem>(newDTO, new tblItem());
            record.createdDate = DateTime.Now;
            db.tblItems.Add(record);
            await db.SaveChangesAsync();
            return await SelectByID(record.id);
        }
    }
}