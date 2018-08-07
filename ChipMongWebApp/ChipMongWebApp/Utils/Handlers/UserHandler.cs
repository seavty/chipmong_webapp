using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO;
using ChipMongWebApp.Models.DTO.User;
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
    public class UserHandler
    {
        private ChipMongEntities db = null;

        public UserHandler()
        {
            db = new ChipMongEntities();
        }

        //-> SelectByID
        public async Task<UserViewDTO> SelectByID(int id)
        {
            var record = await db.tblUsers.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            return MappingHelper.MapDBClassToDTO<tblUser, UserViewDTO>(record);
        }

        //-> New
        public async Task<UserViewDTO> New(UserNewDTO newDTO)
        {
            newDTO = StringHelper.TrimStringProperties(newDTO);
            var checkRecord = await db.tblUsers.FirstOrDefaultAsync(x => x.deleted == null && x.userName == newDTO.userName);
            if (checkRecord != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.LOGIN_NAME_EXIST);

            var record = (tblUser)MappingHelper.MapDTOToDBClass<UserNewDTO, tblUser>(newDTO, new tblUser());
            record.createdDate = DateTime.Now;
            record.password = CryptingHelper.EncryptString("123");
            db.tblUsers.Add(record);
            await db.SaveChangesAsync();
            db.Entry(record).Reload();
            return await SelectByID(record.id);
        }

        //-> Edit
        public async Task<UserViewDTO> Edit(UserEditDTO editDTO)
        {
            editDTO = StringHelper.TrimStringProperties(editDTO);
            var checkRecord = await db.tblUsers.FirstOrDefaultAsync(x => x.deleted == null && x.userName == editDTO.userName && x.id != editDTO.id);
            if (checkRecord != null)
                throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.LOGIN_NAME_EXIST);

            var record = await db.tblUsers.FirstOrDefaultAsync(x => x.deleted == null && x.id == editDTO.id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            
            record = (tblUser)MappingHelper.MapDTOToDBClass<UserEditDTO, tblUser>(editDTO, record);
            //record.updatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return await SelectByID(record.id);
        }

        //-> GetList
        public async Task<GetListDTO<UserViewDTO>> GetList(UserFindDTO findDTO)
        {
            //--seem like search sql not dynamic -> should write one helper function or interface to do dynamic search
            IQueryable<tblUser> records = from x in db.tblUsers
                                                    where x.deleted == null
                                                    && (string.IsNullOrEmpty(findDTO.userName) ? 1 == 1 : x.userName.Contains(findDTO.userName))
                                                    && (string.IsNullOrEmpty(findDTO.firstName) ? 1 == 1 : x.firstName.Contains(findDTO.firstName))
                                                    && (string.IsNullOrEmpty(findDTO.lastName) ? 1 == 1 : x.userName.Contains(findDTO.lastName))
                                                    orderby x.firstName ascending
                                                    select x;
            return await Listing(findDTO.currentPage, records);
        }

        //-> Delete
        public async Task<Boolean> Delete(int id)
        {
            var record = await db.tblUsers.FirstOrDefaultAsync(x => x.deleted == null && x.id == id);
            if (record == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");
            record.deleted = 1;
            await db.SaveChangesAsync();
            return true;
        }

        //-> Listing
        public async Task<GetListDTO<UserViewDTO>> Listing(int currentPage, IQueryable<tblUser> records, string search = null)
        {
            var recordList = new List<UserViewDTO>();
            foreach (var record in PaginationHelper.GetList(currentPage, records))
            {
                recordList.Add(await SelectByID(record.id));
            }
            var getList = new GetListDTO<UserViewDTO>();
            getList.metaData = await PaginationHelper.GetMetaData(currentPage, records, "id", "asc", search);
            getList.metaData.numberOfColumn = 6; // need to change number of column
            getList.items = recordList;
            return getList;
        }

        //-> Change Password
        public async Task<UserViewDTO> ChangePassword(UserChangePasswordDTO changePasswordDTO)
        {
            var user = (UserViewDTO)HttpContext.Current.Session["user"];

            var password = CryptingHelper.EncryptString(changePasswordDTO.password);
            var checkRecord = await db.tblUsers.FirstOrDefaultAsync(x => x.deleted == null && x.id == user.id && x.password == password);
            if (checkRecord == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.INCORRECT_PASSWORD);

            if(changePasswordDTO.newPassword != changePasswordDTO.comfirmPassword)
                throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.PASSWORD_DOES_NOT_MATCH);

            checkRecord.password = CryptingHelper.EncryptString(changePasswordDTO.newPassword);
            
            await db.SaveChangesAsync();
            return await SelectByID(checkRecord.id);
        }

    }
}