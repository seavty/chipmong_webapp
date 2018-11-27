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
using ClosedXML.Excel;
using ChipMongWebApp.Utils.Helpers;
using ChipMongWebApp.Utils.Extension;
using System.Linq.Dynamic;
using ChipMongWebApp.Models.DTO.User;
using System.Web.Mvc;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace ChipMongWebApp.Utils.Handlers
{
    public class SaleOrderHandler
    {
        private readonly ChipMongEntities db;

        public SaleOrderHandler()
        {
            db = new ChipMongEntities();
            db.Database.CommandTimeout = ConstantHelper.DB_TIMEOUT;

        }

        public async Task<String> unlockRec(int id)
        {
            var session = HttpContext.Current.Session;
            UserViewDTO user = (UserViewDTO)session["user"];
            var tbl = await db.tblSaleOrders.FirstOrDefaultAsync(x => x.id == id);
            if (tbl == null)
                return "Sale Order not found !";

            if (user.user_Profile != 1)
            {
                if (tbl.slor_LockBy != null)
                    if (tbl.slor_LockBy != user.id)
                    {
                        var _u = await db.tblUsers.FirstOrDefaultAsync(x => x.id == tbl.slor_LockBy);
                        if (_u != null)
                        {
                            return "Record is currently in use by " + _u.userName + " !";
                        }
                        return "Record is currently in use !";
                    }
            }
            tbl.slor_LockBy = null;
            tbl.slor_LockOn = null;
            await db.SaveChangesAsync();
            return "True";
        }

        public async Task<String> lockRec(int id)
        {
            var session = HttpContext.Current.Session;
            UserViewDTO user = (UserViewDTO)session["user"];
            var tbl = await db.tblSaleOrders.FirstOrDefaultAsync(x => x.id == id);
            if (tbl == null)
                return "Sale Order not found !";

            if (tbl.slor_LockBy != null)
                if (tbl.slor_LockBy != user.id)
                {
                    var _u = await db.tblUsers.FirstOrDefaultAsync(x => x.id == tbl.slor_LockBy);
                    if (_u != null)
                    {
                        return "Record is currently in use by " + _u.userName + " !";
                    }
                }
            tbl.slor_LockBy = user.id;
            tbl.slor_LockOn = DateTime.Now;
            await db.SaveChangesAsync();
            return "True";
        }

        //-> SelectByID
        public async Task<SaleOrderViewDTO> SelectByID(int id, bool _lock = false)
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
            else
            {
                return new SaleOrderViewDTO();
            }

            var record = await db.tblSaleOrders.FirstOrDefaultAsync(x => x.deleted == null && x.id == id
                //var record = await db.vSaleOrders.FirstOrDefaultAsync(x => x.slor_Deleted == null && x.slor_SaleOrderID == id &&
                //&&(_user == -1 ? 1 == 1 : _user == x.cust_UserID)
                );
            if (_user != -1)
            {
                var cust = await db.tblCustomers.FirstOrDefaultAsync(x => x.id == record.customerID);
                if (cust != null)
                {
                    if (cust.cust_UserID != _user)
                        return new SaleOrderViewDTO();
                }
            }
            if (record == null)
                return new SaleOrderViewDTO();
            //throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");



            if (_lock)
            {
                if (record.slor_LockBy != null)
                    if (record.slor_LockBy != user.id)
                    {
                        var tt = new SaleOrderViewDTO();
                        tt.status = "_inuse";
                        tt.code = record.code;
                        var _u = await db.tblUsers.FirstOrDefaultAsync(x => x.id == record.slor_LockBy);
                        if (_u != null)
                            tt.product = _u.userName;
                        return tt;
                    }

                record.slor_LockBy = user.id;
                record.slor_LockOn = DateTime.Now;
                await db.SaveChangesAsync();
            }

            var saleOrderDTO = MappingHelper.MapDBClassToDTO<tblSaleOrder, SaleOrderViewDTO>(record);
            saleOrderDTO.customer = await new CustomerHandler().SelectByID(int.Parse(record.customerID.ToString()));
            saleOrderDTO.items = await GetLineItems(id);
            return saleOrderDTO;
        }

        public async Task<vSaleOrderDTO> SelectByIDList(int id)
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
            else
            {
                return new vSaleOrderDTO();
            }

            var record = await db.vSaleOrders.FirstOrDefaultAsync(x => x.slor_Deleted == null && x.slor_SaleOrderID == id
                //var record = await db.vSaleOrders.FirstOrDefaultAsync(x => x.slor_Deleted == null && x.slor_SaleOrderID == id &&
                //&&(_user == -1 ? 1 == 1 : _user == x.cust_UserID)
                );
            if (record == null)
                return new vSaleOrderDTO();
            //throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");



            var saleOrderDTO = MappingHelper.MapDBClassToDTO<vSaleOrder, vSaleOrderDTO>(record);
            //saleOrderDTO.customer = await new CustomerHandler().SelectByID(int.Parse(record.customerID.ToString()));
            //saleOrderDTO.items = await GetLineItems(id);
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
                    var session = HttpContext.Current.Session;
                    UserViewDTO user = (UserViewDTO)session["user"];
                    if (user != null)
                    {
                        newDTO.slor_UserID = user.id;
                    }
                    newDTO = StringHelper.TrimStringProperties(newDTO);
                    newDTO.date = newDTO.date.ToDBDate();
                    newDTO.requiredDate = newDTO.requiredDate.ToDBDate();
                    var record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderNewDTO, tblSaleOrder>(newDTO, new tblSaleOrder());
                    record.createdDate = DateTime.Now;
                    record.createdBy = user.id;

                    db.tblSaleOrders.Add(record);
                    await db.SaveChangesAsync();
                    var lineItems = await SaveLineItem(record.id, newDTO);
                    record.total = lineItems.Sum(item => item.total);
                    await db.SaveChangesAsync();
                    db.Entry(record).Reload();
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

        public async Task<String> QEdit(int id, string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    var record = await db.tblSaleOrders.FirstOrDefaultAsync(r => r.deleted == null && r.id == id);
                    if (record == null)
                        return "Sale Order not found !";



                    //newDTO.requiredDate = newDTO.requiredDate.ToDBDate();
                    
                    record.updatedDate = DateTime.Now;
                    //record.slor_DocNo = p1;
                    record.requiredDate = DateTime.ParseExact(p1, ConstantHelper.ddMMyyyy_DASH_SEPARATOR, CultureInfo.InvariantCulture);
                    record.sourceOfSupplyID = int.Parse(p2);
                    record.slor_TruckNo = p3;
                    record.slor_TruckDriverPhoneNo = p4;
                    record.slor_DocNo = p5;
                    record.slor_ShipmentNo = p6;
                    //record.slor_ShipmentNo = p2;
                    record.status = p8;
                    record.slor_LockBy = null;
                    record.slor_LockOn = null;
                    var session = HttpContext.Current.Session;
                    UserViewDTO user = (UserViewDTO)session["user"];
                    if (user != null)
                    {
                        record.updatedBy = user.id;
                    }
                    await db.SaveChangesAsync();
                    transaction.Commit();

                    
                    return "True";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

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
                    editDTO.requiredDate = editDTO.requiredDate.ToDBDate();
                    var record = await db.tblSaleOrders.FirstOrDefaultAsync(r => r.deleted == null && r.id == editDTO.id);
                    if (record == null)
                        throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
                    editDTO.updatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    record = (tblSaleOrder)MappingHelper.MapDTOToDBClass<SaleOrderEditDTO, tblSaleOrder>(editDTO, record);
                    record.updatedDate = DateTime.Now;
                    var session = HttpContext.Current.Session;
                    UserViewDTO user = (UserViewDTO)session["user"];
                    if (user != null)
                    {
                        record.updatedBy = user.id;
                    }
                    record.slor_LockBy = null;
                    record.slor_LockOn = null;
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
        public async Task<GetListDTO<vSaleOrderDTO>> GetList(SaleOrderFindDTO findDTO)
        {
            /*
            IQueryable<tblSaleOrder> records = from s in db.tblSaleOrders
                                               join c in db.tblCustomers on s.customerID equals c.id
                                               where s.deleted == null
                                               && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1 : s.code.Contains(findDTO.code))
                                               && (string.IsNullOrEmpty(findDTO.status) ? 1 == 1 : s.status == findDTO.status)
                                               && (string.IsNullOrEmpty(findDTO.customer) ? 1 == 1 : c.firstName.Contains(findDTO.customer))
                                               orderby s.id ascending
                                               select s;
            
            return await Listing(findDTO.currentPage, records);
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
            IQueryable<SaleOrderFindResultDTO> query = from s in db.vSaleOrders
                                                       //join c in db.tblCustomers on s.cust_CustomerID equals c.id
                                                       where s.slor_Deleted == null
                                                       && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1 : s.cust_Code.Contains(findDTO.code))
                                                       && (string.IsNullOrEmpty(findDTO.status) ? 1 == 1 : s.slor_Status == findDTO.status)
                                                       && (string.IsNullOrEmpty(findDTO.customer) ? 1 == 1 : s.cust_FirstName.Contains(findDTO.customer))
                                                       && (findDTO.customerID == 0 ? 1 == 1 : s.cust_CustomerID == findDTO.customerID)
                                                       //&& (_user == -1 ? 1 == 1 : c.cust_UserID == _user)
                                                       select new SaleOrderFindResultDTO
                                                       {
                                                           id = s.slor_SaleOrderID,
                                                           code = s.slor_Code,
                                                           date = s.slor_Date,
                                                           firstName = s.cust_FirstName,
                                                           total = s.slor_Total,
                                                           status = s.slor_Status
                                                       };
            */
            //return await Listing(findDTO.currentPage, records);

            
            var query = db.vSaleOrders.Where(x => x.slor_Deleted == null
                && (string.IsNullOrEmpty(findDTO.code) ? 1 == 1                    : x.cust_Code.Contains(findDTO.code))
                && (string.IsNullOrEmpty(findDTO.status) ? 1 == 1                  : x.slor_Status == findDTO.status)
                && (string.IsNullOrEmpty(findDTO.customer) ? 1 == 1                : x.cust_FirstName.Contains(findDTO.customer))
                && (string.IsNullOrEmpty(findDTO.slor_SourceOfSupply) ? 1 == 1     : x.slor_SourceOfSupply.ToString() == findDTO.slor_SourceOfSupply)
                && (string.IsNullOrEmpty(findDTO.slor_TruckNo) ? 1 == 1            : x.slor_TruckNo.Contains(findDTO.slor_TruckNo))
                && (string.IsNullOrEmpty(findDTO.slor_DocNo) ? 1 == 1              : x.slor_DocNo.Contains(findDTO.slor_DocNo))
                && (string.IsNullOrEmpty(findDTO.slor_ShipmentNo) ? 1 == 1         : x.slor_ShipmentNo.Contains(findDTO.slor_ShipmentNo))
                && (string.IsNullOrEmpty(findDTO.slor_TruckDriverPhoneNo) ? 1 == 1 : x.slor_TruckDriverPhoneNo.Contains(findDTO.slor_TruckDriverPhoneNo))
                && (findDTO.customerID == 0 ? 1 == 1                               : x.cust_CustomerID == findDTO.customerID)
            );


            DateTime? fromDate = null;
            DateTime? toDate = null;


            if (!string.IsNullOrEmpty(findDTO.slor_RequiredDate_From) && !string.IsNullOrEmpty(findDTO.slor_RequiredDate_To))
            {

                fromDate = DateTime.ParseExact(findDTO.slor_RequiredDate_From, ConstantHelper.ddMMyyyy_DASH_SEPARATOR, CultureInfo.InvariantCulture);
                toDate = DateTime.ParseExact(findDTO.slor_RequiredDate_To, ConstantHelper.ddMMyyyy_DASH_SEPARATOR, CultureInfo.InvariantCulture);

            }

            if (!string.IsNullOrEmpty(findDTO.slor_RequiredDate_From) && !string.IsNullOrEmpty(findDTO.slor_RequiredDate_To))
            {
                query = query.Where(x => DbFunctions.TruncateTime(x.slor_RequiredDate) >= DbFunctions.TruncateTime(fromDate) && DbFunctions.TruncateTime(x.slor_RequiredDate) <= DbFunctions.TruncateTime(toDate));
            }
            

           


            
            return await ListingHandler(findDTO.currentPage, query.AsQueryable().OrderBy($"{findDTO.orderBy} {findDTO.orderDirection}"));
        }

        //-> Listing
        public async Task<GetListDTO<vSaleOrderDTO>> ListingHandler(int currentPage, IQueryable<dynamic> query)
        {
            int totalRecord = await query.CountAsync();
            var records = await query.Page(currentPage).ToListAsync();

            //var records = await query.Skip(0).Take(10).ToListAsync();
                //source.Skip((currentPage - 1) * PaginationHelper.PAGE_SIZE).Take(PaginationHelper.PAGE_SIZE);

            var myList = new List<vSaleOrderDTO>();
            foreach (var record in records)
            {
                vSaleOrderDTO tmp = await SelectByIDList(record.slor_SaleOrderID);
                //if(tmp.id !=null)
                myList.Add(tmp);
            }
            var getList = new GetListDTO<vSaleOrderDTO>();
            getList.metaData = PaginationHelper.GetMetaData(currentPage, totalRecord);
            getList.metaData.numberOfColumn = 15; // need to change number of column
            getList.items = myList;
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

        //-> UploadExcel
        public async Task<int> UploadExcel(SaleOrderUploadExcel uploadExcel)
        {
            if (uploadExcel.ExcelFile.ContentLength < 0)
                throw new HttpException((int)HttpStatusCode.BadRequest, "Not a valid file");

            if (!uploadExcel.ExcelFile.FileName.EndsWith(".xlsx"))
                throw new HttpException((int)HttpStatusCode.BadRequest, "Only .xlsx is allowed");

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    XLWorkbook Workbook = new XLWorkbook(uploadExcel.ExcelFile.InputStream);
                    IXLWorksheet WorkSheet = null;
                    WorkSheet = Workbook.Worksheet("sheet1");
                    if (!DocumentHelper.SaveExcelFile(uploadExcel.ExcelFile))
                        throw new HttpException((int)HttpStatusCode.BadRequest, "Error saving file.");

                    WorkSheet.FirstRow().Delete();//delete header column
                    int countUpdateRecord = 0;
                    foreach (var row in WorkSheet.RowsUsed())
                    {
                        var soNumber = row.Cell(1).Value.ToString().Trim().Replace(" ", string.Empty);//Get ist cell. 1 represent column number
                        var status = row.Cell(2).Value.ToString().Trim().Replace(" ", string.Empty); ;

                        var record = await db.tblSaleOrders.FirstOrDefaultAsync(x => x.deleted == null && x.code == soNumber);
                        if (record != null)
                        {
                            countUpdateRecord++;
                            record.status = status;
                            await db.SaveChangesAsync();
                        }
                    }
                    transaction.Commit();
                    return countUpdateRecord++;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<String> sendLine(string _ids)
        {
            string re = "True";
            Dictionary<string, string> datas = new Dictionary<string, string>();
            List<int> ids = new List<int>();
            foreach(var _id in _ids.Split(','))
            {
                ids.Add(int.Parse(_id));
            }
            //foreach (var id in ids)
            {
                var records = db.tblSaleOrders.Where(x => ids.Contains(x.id));
                foreach (var record in records)
                {
                    
                    var cust = await db.tblCustomers.FirstOrDefaultAsync(x => x.id == record.customerID);
                    if (cust != null)
                    {
                        
                        if (datas.FirstOrDefault(x=>x.Key.ToLower() == cust.cust_LineID.ToLower()).Key == null)
                        {
                            //add
                            datas.Add(cust.cust_LineID, "PO : " + record.code +
                                 "\\nDoc No : " + record.slor_DocNo +
                                "\\nShipment No : " + record.slor_ShipmentNo +
                                "\\nStatus : " + record.status + "\\n");
                        }
                        else
                        {
                            //update
                            datas[cust.cust_LineID] = datas[cust.cust_LineID] + "\\n" +
                                "PO : " + record.code +
                                 "\\nDoc No : " + record.slor_DocNo +
                                "\\nShipment No : " + record.slor_ShipmentNo +
                                "\\nStatus : " + record.status + "\\n";
                        }
                    }
                }
            }
            HttpClient client = new HttpClient();
            foreach (var data in datas)
            {
                //re = re + data.Key + " : " + data.Value + "<br/>";
                var json =
                    "{" +
                        //"\"to\":\"Uc79987d199ba2a75399aefb58239a7bc\"," +
                        "\"to\":\"" + data.Key + "\"," +
                        "\"messages\": [{" +
                            "\"type\":\"text\"," +
                            "\"text\":\"" + data.Value +
                            "\"" +
                        "}]" +
                    "}"
                ;

                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/push");

                requestMessage.Content = stringContent;
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                requestMessage.Headers.TryAddWithoutValidation("Authorization",
                    ("Bearer {" +
                        //"UCy4WuQi+M9MuTV9TP/cGHSL981T6OSo5t9VOr/JG5EFLVnEWMjADpHawV9dyswYVdkOmxPXQM2WnrnuUwP0EdrofrezfWoBfPH2u3ex+kFNsx9vG9qmvlETvkYjtwchB+GZkUNv3NcithZ9h7fsgAdB04t89/1O/w1cDnyilFU=" +
                        System.Configuration.ConfigurationManager.AppSettings["token"] +
                        "}")
                );

                HttpResponseMessage response = await client.SendAsync(requestMessage);
                
            }

            return re;
        }

    }
}
