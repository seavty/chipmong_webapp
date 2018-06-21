﻿using ChipMongWebApp.Helpers;
using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.Customer;
using ChipMongWebApp.Models.DTO.SSA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Handlers
{
    public class CustomerHandler
    {
        private ChipMongEntities db = null;

        public CustomerHandler() { db = new ChipMongEntities(); }

        //-> SelectByID
        public async Task<CustomerViewDTO> SelectByID(int id)
        {
            var record = await db.tblCustomers.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return MappingHelper.MapDBClassToDTO<tblCustomer, CustomerViewDTO>(record);
        }

        //-> New
        public async Task<CustomerViewDTO> New(CustomerNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var record = (tblCustomer)MappingHelper.MapDTOToDBClass<CustomerNewDTO, tblCustomer>(newDTO, new tblCustomer());
            record.createdDate = DateTime.Now;
            db.tblCustomers.Add(record);
            await db.SaveChangesAsync();
            return await SelectByID(record.id);
        }

        //-> Edit
        public async Task<CustomerViewDTO> Edit(CustomerEditDTO editDTO)
        {
            var record = await db.tblCustomers.FirstOrDefaultAsync(x => x.deleted == null && x.id == editDTO.id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            editDTO = StringHelper.TrimStringProperties(editDTO);
            record = (tblCustomer)MappingHelper.MapDTOToDBClass<CustomerEditDTO, tblCustomer>(editDTO, record);
            record.updatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(record.id);
        }

        //-> GetList
        public async Task<GetListDTO<CustomerViewDTO>> GetList(CustomerFindDTO findDTO)
        {
            //--seem like search sql not dynamic -> should write one helper function or interface to do dynamic search
            IQueryable<tblCustomer> records = from x in db.tblCustomers
                                                where x.deleted == null
                                                && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1 : x.code.Contains(findDTO.code))
                                                && (string.IsNullOrEmpty(findDTO.firstName) ? 1 == 1 : x.firstName.Contains(findDTO.firstName))
                                                && (string.IsNullOrEmpty(findDTO.lastName) ? 1 == 1 : x.lastName == findDTO.lastName)
                                                orderby x.id ascending
                                                select x;
            return await Listing(findDTO.currentPage, records);
        }

        //-> Listing
        private async Task<GetListDTO<CustomerViewDTO>> Listing(int currentPage, IQueryable<tblCustomer> records, string search = null)
        {
            var recordList = new List<CustomerViewDTO>();
            foreach (var record in PaginationHelper.GetList(currentPage, records))
            {
                recordList.Add(await SelectByID(record.id));
            }
            var getList = new GetListDTO<CustomerViewDTO>();
            getList.metaData = await PaginationHelper.GetMetaData(currentPage, records, "id", "asc", search);
            getList.metaData.numberOfColumn = 6; // need to change number of column
            getList.items = recordList;
            return getList;
        }

        //-> SSA
        public async Task<GetSSADTO<CustomerSSADTO>> SSA(string search)
        {
            IQueryable<tblCustomer> records = from x in db.tblCustomers
                                                where x.deleted == null
                                                && (string.IsNullOrEmpty(search) ? 1 == 1 : x.firstName.StartsWith(search))
                                                orderby x.id ascending
                                                select x;
            var list = await Listing(1, records);
            var customerList = new List<CustomerSSADTO>();
            foreach (var item in list.items)
            {
                customerList.Add((CustomerSSADTO)MappingHelper.MapDTOToDTO<CustomerViewDTO, CustomerSSADTO>(item, new CustomerSSADTO()));
            }
            var ssa = new GetSSADTO<CustomerSSADTO>();
            ssa.results = customerList;
            return ssa;
        }

        //-> Delete
        public async Task<Boolean> Delete(int id)
        {
            var record = await db.tblCustomers.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");

            var checkRecord = await db.tblSaleOrders.FirstOrDefaultAsync(x => x.deleted == null && x.customerID == id);
            if (checkRecord != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Cannot this record because it is currently in used!");
            record.deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }
    }
}