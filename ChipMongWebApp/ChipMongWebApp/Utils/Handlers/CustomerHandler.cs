using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.Customer;
using ChipMongWebApp.Models.DTO.DealerSourceSupply;
using ChipMongWebApp.Models.DTO.SaleOrder;
using ChipMongWebApp.Models.DTO.SSA;
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
using ChipMongWebApp.Models.DTO.User;

namespace ChipMongWebApp.Utils.Handlers
{
    public class CustomerHandler
    {
        private readonly ChipMongEntities db;

        public CustomerHandler()
        {
            db = new ChipMongEntities();
            db.Database.CommandTimeout = ConstantHelper.DB_TIMEOUT;
        }

        //-> SelectByID
        public async Task<CustomerViewDTO> SelectByID(int id)
        {
            var session = HttpContext.Current.Session;
            UserViewDTO user = (UserViewDTO)session["user"];
            int _user = 0;
            if (user != null)
            {
                _user = user.id;
                if (user.user_Profile == 1)
                    _user = -1;
            }

            var record = await db.tblCustomers.FirstOrDefaultAsync(x => x.deleted == null && x.id == id
                && (_user == -1? 1 == 1 : _user == x.cust_UserID)
                );
            if (record == null)
                return new CustomerViewDTO();
            return MappingHelper.MapDBClassToDTO<tblCustomer, CustomerViewDTO>(record);
        }

        //-> New
        public async Task<CustomerViewDTO> New(CustomerNewDTO newDTO)
        {
            var session = HttpContext.Current.Session;
            UserViewDTO user = (UserViewDTO)session["user"];
            if (user != null)
            {
                newDTO.cust_UserID = user.id;
            }
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var record = (tblCustomer)MappingHelper.MapDTOToDBClass<CustomerNewDTO, tblCustomer>(newDTO, new tblCustomer());
            record.createdDate = DateTime.Now;
            db.tblCustomers.Add(record);

            await db.SaveChangesAsync();
            db.Entry(record).Reload();
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
            var session = HttpContext.Current.Session;
            UserViewDTO user = (UserViewDTO)session["user"];
            int _user = 0;
            if (user != null)
            {
                _user = user.id;
                if(user.user_Profile == 1)
                    _user = -1;
            }
            
            IQueryable<tblCustomer> query = db.tblCustomers.Where(x => x.deleted == null);

            if (!string.IsNullOrEmpty(findDTO.code))        query = query.Where(x => x.code.StartsWith(findDTO.code));
            if (!string.IsNullOrEmpty(findDTO.firstName))   query = query.Where(x => x.firstName.StartsWith(findDTO.firstName));
            if (!string.IsNullOrEmpty(findDTO.lastName))    query = query.Where(x => x.lastName.StartsWith(findDTO.lastName));
            if (_user != -1) { query = query.Where(x => x.cust_UserID == _user); }
            query = query.AsQueryable().OrderBy($"{findDTO.orderBy} {findDTO.orderDirection}");
            
            return await ListingHandler(findDTO.currentPage, query);
        }

        //-> ListingHandler
        private async Task<GetListDTO<CustomerViewDTO>> ListingHandler(int currentPage, IQueryable<tblCustomer> query)
        {
            int totalRecord = await query.CountAsync();
            List<tblCustomer> records = await query.Page(currentPage).ToListAsync();

            var myList = new List<CustomerViewDTO>();
            foreach (var record in records)
            {
                myList.Add(await SelectByID(record.id));
            }
            var getList = new GetListDTO<CustomerViewDTO>();
            getList.metaData = PaginationHelper.GetMetaData(currentPage, totalRecord);
            getList.metaData.numberOfColumn = 6; // need to change number of column
            getList.items = myList;
            return getList;
        }

        //-> SSA
        public async Task<GetSSADTO<CustomerViewDTO>> SSA(string search)
        {
            IQueryable<tblCustomer> query = db.tblCustomers.Where(x => x.deleted == null);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.firstName.StartsWith(search));
            query = query.OrderBy(x => x.firstName);

            var ssa = new GetSSADTO<CustomerViewDTO>();
            ssa.results = (await ListingHandler(1, query)).items;

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

        //-> GetList SaleOrderTabPaging
        public async Task<GetListDTO<vSaleOrderDTO>> SaleOrderTabPaging(int customerID, int currentPage)
        {
            /*
            IQueryable<tblSaleOrder> records = from s in db.tblSaleOrders
                                               join c in db.tblCustomers on s.customerID equals c.id
                                               where s.deleted == null && s.customerID == customerID
                                               orderby s.id ascending
                                               select s;
            var saleOrderHandler = new SaleOrderHandler();
            return await saleOrderHandler.Listing(currentPage, records);
            */
            var session = HttpContext.Current.Session;
            UserViewDTO user = (UserViewDTO)session["user"];
            int _user = 0;
            if (user != null)
            {
                _user = user.id;
                if (user.user_Profile == 1)
                    _user = -1;
            }
            /*
            IQueryable<tblSaleOrder> records = from s in db.tblSaleOrders
                                               join c in db.tblCustomers on s.customerID equals c.id
                                               where s.deleted == null && s.customerID == customerID
                                               && (_user == -1 ? 1==1 : _user == s.slor_UserID)
                                               orderby s.id ascending
                                               select s;*/
            var records = db.vSaleOrders.Where(x => x.slor_Deleted == null
                && (_user == -1 ? 1 == 1 : _user == x.slor_UserID)).OrderBy(x=>x.slor_SaleOrderID);
            var saleOrderHandler = new SaleOrderHandler();
            return await saleOrderHandler.ListingHandler(currentPage, records);

        }

        //-> GetList SourceSupplyTabPaging
        public List<DealerSourceSupplyDTO> SourceSupplyTabPaging(int customerID)
        {

            var records = from s in db.tblSourceOfSupplies
                          join d in db.tblDealerSourceOfSupplies on s.id equals d.sourceOfSupplyID
                          join c in db.tblCustomers on d.dealerID equals c.id
                          where s.deleted == null && d.deleted == null && c.id == customerID
                          orderby s.name ascending
                          select
                          (
                           new DealerSourceSupplyDTO
                           {
                               id = d.id,
                               sourceSupplyID = s.id,
                               name = s.name

                           });
            return records.ToList(); ;

            //var myTest = await records.ToListAsync();
            //return await records.ToListAsync();

        }


        //=> GetList SourceSupplyTabPaging
        public List<tblRetailer> RetailerTabPaging(int customerID)
        {

            var records = from x in db.tblRetailers
                          where x.retl_Deleted == null && x.retl_CustomerID == customerID
                          orderby x.retl_Name ascending
                          select x;
                          
            return records.ToList(); ;

            //var myTest = await records.ToListAsync();
            //return await records.ToListAsync();

        }




        //-> New
        public async Task<bool> AddSourceSupply(int customerID, int sourceSupplyID)
        {
            var record = new tblDealerSourceOfSupply();
            record.dealerID = customerID;
            record.sourceOfSupplyID = sourceSupplyID;
            db.tblDealerSourceOfSupplies.Add(record);
            await db.SaveChangesAsync();
            return true;
        }
    }
}