using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.Item;
using ChipMongWebApp.Models.DTO.SaleOrder;
using ChipMongWebApp.Models.DTO.SaleOrderItem;
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

            var saleOrderDTO = MappingHelper.MapDBClassToDTO<tblSaleOrder, SaleOrderViewDTO>(record);
            saleOrderDTO.customer = await new CustomerHandler().SelectByID(int.Parse(record.customerID.ToString()));
            saleOrderDTO.items = await GetLineItems(id);


            IQueryable<tblItem> records = from x in db.tblItems
                                          where x.deleted == null
                                          orderby x.id ascending
                                          select x;
            var items = await records.ToListAsync();
            var recordList = new List<ItemViewDTO>();
            foreach (var item in items)
            {
                recordList.Add((ItemViewDTO)MappingHelper.MapDBClassToDTO<tblItem, ItemViewDTO>(item));

            }
            saleOrderDTO.itemSelection = recordList;
            return saleOrderDTO;
        }


        private async Task<List<SaleOrderItemViewDTO>> GetLineItems(int masterID)
        {
            var items = new List<SaleOrderItemViewDTO>();

            IQueryable<tblSaleOrderItem> records = from x in db.tblSaleOrderItems
                                                   where x.deleted == null && x.saleOrderID == masterID
                                                   orderby x.id ascending
                                                   select x;
            var listing = await records.ToListAsync();
            foreach (var item in listing)
            {
                var mappingDTO = MappingHelper.MapDBClassToDTO<tblSaleOrderItem, SaleOrderItemViewDTO>(item);
                mappingDTO.item = await (new ItemHandler().SelectByID(int.Parse(item.itemID.ToString())));
                //items.Add(MappingHelper.MapDBClassToDTO<tblSaleOrderItem, SaleOrderItemViewDTO>(item));
                items.Add(mappingDTO);
            }
            return items;
        }


        //-> Create
        public async Task<SaleOrderViewDTO> Create(SaleOrderNewDTO newDTO)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    newDTO = StringHelper.TrimStringProperties(newDTO);
                    var record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderNewDTO, tblSaleOrder>(newDTO, new tblSaleOrder());
                    record.createdDate = DateTime.Now;
                    db.tblSaleOrders.Add(record);
                    await db.SaveChangesAsync();
                    var lineItems = await SaveLineItem(record.id, newDTO);
                    record.total = lineItems.Sum(item => item.total);
                    await db.SaveChangesAsync();

                    transaction.Commit();
                    return await SelectByID(record.id);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        //-> save line item 
        private async Task<List<tblSaleOrderItem>> SaveLineItem(int mastetID, SaleOrderNewDTO newDTO)
        {
            var list = new List<tblSaleOrderItem>();
            if (newDTO.items != null)
            {
                foreach (var item in newDTO.items)
                {
                    var record = (tblSaleOrderItem)MappingHelper.MapDTOToDBClass<SaleOrderItemNewDTO, tblSaleOrderItem>(item, new tblSaleOrderItem());
                    record.createdDate = DateTime.Now;

                    record.total = record.quantity * record.price;
                    record.saleOrderID = mastetID;
                    db.tblSaleOrderItems.Add(record);
                    await db.SaveChangesAsync();

                    list.Add(record);
                }
            }
            return list;
        }

        //-> Save
        public async Task<SaleOrderViewDTO> Edit(SaleOrderEditDTO editDTO)
        {
            /*
            
            //--> big mistake need to change no need to map DTO 2 time from view to edit -> just map it when post from form is ok
            var record = await db.tblSaleOrders.FirstOrDefaultAsync(r => r.deleted == null && r.id == editDTO.id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            editDTO = StringHelper.TrimStringProperties(editDTO);
            record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderEditDTO, tblSaleOrder>(editDTO, record);
            record.updatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(record.id);
            */
            //--> big mistake need to change no need to map DTO 2 time from view to edit -> just map it when post from form is ok
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    editDTO = StringHelper.TrimStringProperties(editDTO);
                    var record = await db.tblSaleOrders.FirstOrDefaultAsync(r => r.deleted == null && r.id == editDTO.id);
                    if (record == null)
                        throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
                    record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderEditDTO, tblSaleOrder>(editDTO, record);
                    record.updatedDate = DateTime.Now;
                    var lineItems = await EditLineItem(record.id, editDTO);
                    record.total = lineItems.Sum(item => item.total);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return await SelectByID(record.id);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

        }


        //-> EditLineItem
        private async Task<List<tblSaleOrderItem>> EditLineItem(int mastetID, SaleOrderEditDTO editDTO)
        {
            var list = new List<tblSaleOrderItem>();
            if (editDTO.items != null)
            {
                foreach (var item in editDTO.items)
                {
                    //var record = (tblSaleOrderItem)MappingHelper.MapDTOToDBClass<SaleOrderItemEditDTO, tblSaleOrderItem>(item, new tblSaleOrderItem());
                    var record = new tblSaleOrderItem();

                    if (item.id == null)
                    {
                        record = (tblSaleOrderItem)MappingHelper.MapDTOToDBClass<SaleOrderItemEditDTO, tblSaleOrderItem>(item, new tblSaleOrderItem());
                        record.createdDate = DateTime.Now;
                    }
                    else
                    {
                        record = await db.tblSaleOrderItems.FirstOrDefaultAsync(x => x.deleted == null && x.id == item.id);
                        record = (tblSaleOrderItem)MappingHelper.MapDTOToDBClass<SaleOrderItemEditDTO, tblSaleOrderItem>(item, record);
                        record.updatedDate = DateTime.Now;
                    }

                    record.total = record.quantity * record.price;
                    record.saleOrderID = mastetID;
                    if (item.id == null)
                        db.tblSaleOrderItems.Add(record);
                    await db.SaveChangesAsync();
                    list.Add(record);
                }
            }
            if (!string.IsNullOrEmpty(editDTO.deleteLineItemID))
            {
                var ids = editDTO.deleteLineItemID.Split(',');
                foreach (var id in ids)
                {
                    var itemID = int.Parse(id);
                    var item = await db.tblSaleOrderItems.FirstOrDefaultAsync(x => x.deleted == null && x.id == itemID);
                    if (item != null)
                    {
                        item.deleted = 1;
                        await db.SaveChangesAsync();
                    }
                }
            }
            return list;
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

        //--> Item Selection 

        public async Task<SaleOrderViewDTO> newDTO()
        {
            var newDTO = new SaleOrderViewDTO();
            IQueryable<tblItem> records = from x in db.tblItems
                                                where x.deleted == null
                                                orderby x.id ascending
                                                select x;
            var items = await records.ToListAsync();
            var recordList = new List<ItemViewDTO>();
            foreach (var item in items)
            {
                recordList.Add((ItemViewDTO)MappingHelper.MapDBClassToDTO<tblItem, ItemViewDTO>(item));

            }
            newDTO.itemSelection = recordList;
            return newDTO;
        }
    }
}