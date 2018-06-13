using ChipMongWebApp.Helpers;
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

        public CustomerHandler()
        {
            db = new ChipMongEntities();
        }

        //-> SelectByID
        public async Task<CustomerViewDTO> SelectByID(int id)
        {
            var customer = await db.tblCustomers.FirstOrDefaultAsync(c => c.deleted == null && c.id == id);
            if (customer == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return MappingHelper.MapDBClassToDTO<tblCustomer, CustomerViewDTO>(customer);
        }

        //-> Create
        public async Task<CustomerViewDTO> Create(CustomerNewDTO cusotmerDTO)
        {
            cusotmerDTO = StringHelper.TrimStringProperties(cusotmerDTO);
            var customer = (tblCustomer)MappingHelper.MapDTOToDBClass<CustomerNewDTO, tblCustomer>(cusotmerDTO, new tblCustomer());
            customer.createdDate = DateTime.Now;
            db.tblCustomers.Add(customer);
            await db.SaveChangesAsync();
            return await SelectByID(customer.id);
        }

        //-> Save
        public async Task<CustomerViewDTO> Edit(CustomerViewDTO viewDTO)
        {
            //--> big mistake need to change no need to map DTO 2 two from view to edit -> just map it when post from form is ok
            var customer = await db.tblCustomers.FirstOrDefaultAsync(c => c.deleted == null && c.id == viewDTO.id);
            if (customer == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            viewDTO = StringHelper.TrimStringProperties(viewDTO);
            var editDTO = new CustomerEditDTO();
            editDTO  = (CustomerEditDTO)MappingHelper.MapDTOToDTO<CustomerViewDTO, CustomerEditDTO>(viewDTO, editDTO);

            customer = (tblCustomer)MappingHelper.MapDTOToDBClass<CustomerEditDTO, tblCustomer>(editDTO, customer);
            customer.updatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(customer.id);

        }
        
        //-> GetList
        public async Task<GetListDTO<CustomerViewDTO>> GetList(CustomerFindDTO findDTO)
        {
            //--seem like search sql not dynamic -> should write one helper function or interface to do dynamic search
            IQueryable<tblCustomer> customers = from c in db.tblCustomers
                                              where c.deleted == null
                                              && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1 : c.code.Contains(findDTO.code))
                                              && (string.IsNullOrEmpty(findDTO.firstName) ? 1 == 1 : c.firstName.Contains(findDTO.firstName))
                                              && (string.IsNullOrEmpty(findDTO.lastName) ? 1 == 1 : c.lastName == findDTO.lastName)
                                              orderby c.id ascending
                                              select c;
            return await Listing(findDTO.currentPage, customers);
        }

        internal Task<JsonResult> SSA(object findDTO)
        {
            throw new NotImplementedException();
        }

        //-> SSA
        public async Task<GetSSADTO<CustomerSSADTO>> SSA(string search)
        {
            IQueryable<tblCustomer> customers = from c in db.tblCustomers
                                                where c.deleted == null
                                                && (string.IsNullOrEmpty(search) ? 1 == 1 : c.firstName.StartsWith(search))
                                                orderby c.id ascending
                                                select c;
            var list = await Listing(1, customers);
            
            var customerList = new List<CustomerSSADTO>();
            foreach (var item in list.items)
            {
                customerList.Add((CustomerSSADTO)MappingHelper.MapDTOToDTO<CustomerViewDTO, CustomerSSADTO>(item, new CustomerSSADTO()));

            }
            var ssa = new GetSSADTO<CustomerSSADTO>();
            ssa.results = customerList;

            return ssa;
        }





        //*** private method ***/
        private async Task<GetListDTO<CustomerViewDTO>> Listing(int currentPage, IQueryable<tblCustomer> customers, string search = null)
        {
            var customerList = new List<CustomerViewDTO>();
            foreach (var customer in PaginationHelper.GetList(currentPage, customers))
            {
                /*
                var checkLoanRequest = new CheckLoanRequestViewDTO();
                checkLoanRequest = await SelectByID(account.id);
                checkLoanRequests.Add(checkLoanRequest);
                */
                customerList.Add(await SelectByID(customer.id));
            }

            var getList = new GetListDTO<CustomerViewDTO>();
            getList.metaData = await PaginationHelper.GetMetaData(currentPage, customers, "id", "asc", search);
            getList.metaData.numberOfColumn = 6;
            getList.items = customerList;
            return getList;
        }
    }
}