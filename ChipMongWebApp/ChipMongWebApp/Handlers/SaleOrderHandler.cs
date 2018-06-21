using ChipMongWebApp.Extension;
using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.Item;
using ChipMongWebApp.Models.DTO.SaleOrder;
using ChipMongWebApp.Models.DTO.SaleOrderItem;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ChipMongWebApp.Handlers
{
    public class SaleOrderHandler
    {
        private ChipMongEntities db = null;

        public SaleOrderHandler() { db = new ChipMongEntities(); }

        //-> SelectByID
        public async Task<SaleOrderViewDTO> SelectByID(int id, int mode)
        {
            var record = await db.tblSaleOrders.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var saleOrderDTO = MappingHelper.MapDBClassToDTO<tblSaleOrder, SaleOrderViewDTO>(record);
            saleOrderDTO.customer = await new CustomerHandler().SelectByID(int.Parse(record.customerID.ToString()));
            saleOrderDTO.items = await GetLineItems(id);
            saleOrderDTO.mode = mode;

            return saleOrderDTO;
        }

        //-> GetLineItems
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
                items.Add(mappingDTO);
            }
            return items;
        }


        //-> New
        public async Task<SaleOrderViewDTO> New(SaleOrderNewDTO newDTO)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    newDTO = StringHelper.TrimStringProperties(newDTO);
                    newDTO.date = newDTO.date.ToDBDate();
                    var record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderNewDTO, tblSaleOrder>(newDTO, new tblSaleOrder());
                    record.createdDate = DateTime.Now;
                    db.tblSaleOrders.Add(record);
                    await db.SaveChangesAsync();
                    var lineItems = await SaveLineItem(record.id, newDTO);
                    record.total = lineItems.Sum(item => item.total);
                    await db.SaveChangesAsync();

                    transaction.Commit();
                    return await SelectByID(record.id, ConstantHelper.MODE_NEW);
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
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    editDTO = StringHelper.TrimStringProperties(editDTO);
                    editDTO.date = editDTO.date.ToDBDate();
                    var record = await db.tblSaleOrders.FirstOrDefaultAsync(r => r.deleted == null && r.id == editDTO.id);
                    if (record == null)
                        throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
                    record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderEditDTO, tblSaleOrder>(editDTO, record);
                    record.updatedDate = DateTime.Now;
                    var lineItems = await EditLineItem(record.id, editDTO);
                    record.total = lineItems.Sum(item => item.total);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return await SelectByID(record.id, ConstantHelper.MODE_EDIT);
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
            IQueryable<tblSaleOrder> records = from s in db.tblSaleOrders
                                               join c in db.tblCustomers on s.customerID equals c.id
                                               where s.deleted == null
                                               && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1 : s.code.Contains(findDTO.code))
                                               && (string.IsNullOrEmpty(findDTO.status) ? 1 == 1 : s.status == findDTO.status)
                                               && (string.IsNullOrEmpty(findDTO.customer) ? 1 == 1 : c.firstName.Contains(findDTO.customer))
                                               orderby s.id ascending
                                               select s;
            return await Listing(findDTO.currentPage, records);
        }

        //-> Listing
        public async Task<GetListDTO<SaleOrderViewDTO>> Listing(int currentPage, IQueryable<tblSaleOrder> records, string search = null)
        {
            var customerList = new List<SaleOrderViewDTO>();
            foreach (var customer in PaginationHelper.GetList(currentPage, records))
            {
                customerList.Add(await SelectByID(customer.id, ConstantHelper.MODE_VIEW));
            }

            var getList = new GetListDTO<SaleOrderViewDTO>();
            getList.metaData = await PaginationHelper.GetMetaData(currentPage, records, "id", "asc", search);
            getList.metaData.numberOfColumn = 6;
            getList.items = customerList;
            return getList;
        }

        //-> Delete
        public async Task<Boolean> Delete(int id)
        {
            var record = await db.tblSaleOrders.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }
    }
}