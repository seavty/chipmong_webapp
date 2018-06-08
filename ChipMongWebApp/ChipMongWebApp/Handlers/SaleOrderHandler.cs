using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.SaleOrder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ChipMongWebApp.Handlers
{
    public class SaleOrderHandler
    {
        private ChipMongEntities db = null;

        public SaleOrderHandler()
        {
            db = new ChipMongEntities();
        }

        //-> SelectByID
        public async Task<SaleOrderViewDTO> SelectByID(int id)
        {
            var record = await db.tblSaleOrders.FirstOrDefaultAsync(r => r.deleted == null && r.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return MappingHelper.MapDBClassToDTO<tblSaleOrder, SaleOrderViewDTO>(record);
        }

        //-> Create
        public async Task<SaleOrderViewDTO> Create(SaleOrderNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderNewDTO, tblSaleOrder>(newDTO, new tblSaleOrder());
            record.createdDate = DateTime.Now;
            db.tblSaleOrders.Add(record);
            await db.SaveChangesAsync();
            return await SelectByID(record.id);
        }

        //-> Save
        public async Task<SaleOrderViewDTO> Edit(SaleOrderEditDTO editDTO)
        {
            //--> big mistake need to change no need to map DTO 2 two from view to edit -> just map it when post from form is ok
            var record = await db.tblSaleOrders.FirstOrDefaultAsync(r => r.deleted == null && r.id == editDTO.id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            editDTO = StringHelper.TrimStringProperties(editDTO);
            record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderEditDTO, tblSaleOrder>(editDTO, record);
            record.updatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(record.id);

        }



        //-> GetList
        public async Task<GetListDTO<SaleOrderViewDTO>> GetList(SaleOrderFindDTO findDTO)
        {
            //--seem like search sql not dynamic -> should write one helper function or interface to do dynamic search
            IQueryable<tblSaleOrder> records = from r in db.tblSaleOrders
                                                where r.deleted == null
                                                && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1 : r.code.Contains(findDTO.code))
                                                orderby r.id ascending
                                                select r;
            return await Listing(findDTO.currentPage, records);
        }

        //*** private method ***/
        private async Task<GetListDTO<SaleOrderViewDTO>> Listing(int currentPage, IQueryable<tblSaleOrder> records, string search = null)
        {
            var customerList = new List<SaleOrderViewDTO>();
            foreach (var customer in PaginationHelper.GetList(currentPage, records))
            {
                customerList.Add(await SelectByID(customer.id));
            }

            var getList = new GetListDTO<SaleOrderViewDTO>();
            getList.metaData = await PaginationHelper.GetMetaData(currentPage, records, "id", "asc", search);
            getList.metaData.numberOfColumn = 6;
            getList.items = customerList;
            return getList;
        }
    }
}