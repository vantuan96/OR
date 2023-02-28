using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Microsite;
using Contract.Shared;
using Contract.User;

namespace DataAccess.DAO
{
    public interface IMicrositeDataAccess
    {
        /// <summary>
        /// Lấy danh sách microsite
        /// </summary>
        /// <param name="status"></param>
        /// <param name="categoryArticleKey">cate bài viết</param>
        /// <param name="categoryProductKey">cate sản phẩm</param>
        /// <returns></returns>
        IQueryable<Models.Microsite> GetListMicrosite(bool? isRootsite, int status, 
            string categoryArticleKey, string categoryProductKey, int micrositeTypeId, 
            string kwLocation, string kwProduct, int micrositeUserId);

        /// <summary>
        /// Lấy microsite theo id
        /// </summary>
        /// <param name="MsId"></param>
        /// <returns></returns>
        Models.Microsite GetMicrositeById(int MsId);        

        /// <summary>
        /// tạo microsite
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        CUDReturnMessage CreateMicrosite(MicrositeContentContract ms, out int msId);

        /// <summary>
        /// Chỉnh sửa microsite
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        CUDReturnMessage UpdateMicrosite(MicrositeContentContract ms);
        
        CUDReturnMessage UpdateMicrositeStatus(int msId, int status);

        /// <summary>
        /// xóa microsite
        /// </summary>
        /// <param name="msId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteMicrosite(int msId);
        
    }

    public class MicrositeDataAccess : BaseDataAccess, IMicrositeDataAccess
    {
        public MicrositeDataAccess(string appid, int uid) : base(appid, uid)
        {
        }

        public IQueryable<Models.Microsite> GetListMicrosite(bool? isRootsite, int status, 
            string categoryArticleKey, string categoryProductKey, int micrositeTypeId, 
            string kwLocation, string kwProduct, int micrositeUserId)
        {
            IQueryable<Models.Microsite> query = DbContext.Microsites.Where(r => r.IsDeleted == false);

            if (isRootsite.HasValue)
            {
                query = query.Where(r => r.IsRootSite == isRootsite.Value);
            }

            if (status != 0)
            {
                query = query.Where(r => r.Status == status);
            }           

           

            if (micrositeUserId > 0)
            {
                query = query.Where(r => r.AdminUser_Microsite.Any(us => us.UId == micrositeUserId));
            }

            return query;
        }

        public Models.Microsite GetMicrositeById(int MsId)
        {
            return DbContext.Microsites.SingleOrDefault(r => r.MsId == MsId && r.IsDeleted == false);
        }


        public CUDReturnMessage CreateMicrosite(MicrositeContentContract ms, out int msId)
        {

                Models.Microsite microsite = new Models.Microsite
                {
                    Name = ms.Title,
                    Status = (int)ObjectStatus.Onsite,
                    IsRootSite = false,
                    ReferenceCode = ms.ReferenceCode,
                    CreatedBy = uid,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    LastUpdatedBy = uid,
                    LastUpdatedDate = DateTime.Now,
                    GalleryKey = ms.GalleryKey
                };

                DbContext.Microsites.Add(microsite);
                DbContext.SaveChanges();                

                // lay danh sach user admin va suppper admin
                var adminUser = DbContext.AdminUsers.Where(r => r.AdminUser_Role.Where(x => x.RId == (int)AdminRole.SuperAdmin
                                                                                          || x.RId == (int)AdminRole.Admin).Any() && r.IsDeleted == false);
                if (adminUser.Any())
                {
                    adminUser.ToList().ForEach(u => u.AdminUser_Microsite.Add(new Models.AdminUser_Microsite()
                    {
                        UId = u.UId,
                        MsId = microsite.MsId,
                        CreatedBy = uid,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    }));
                }

                DbContext.SaveChanges();

                //#region Add Gallery
                //// Thêm gallery mặc định cho microsite

                //Models.Gallery gallery = new Models.Gallery
                //{
                //    ApprovalStatus = (int)ApprovalStatus.New,
                //    IsOnsite = false,
                //    MsId = microsite.MsId,
                //    CreatedBy = uid,
                //    CreatedDate = DateTime.Now,
                //    LastUpdatedBy = uid,
                //    LastUpdatedDate = DateTime.Now,
                //    IsPredefined = true,
                //    IsDeleted = false,
                //    Key = GalleryKey
                //};

                //DbContext.Galleries.Add(gallery);
                //DbContext.SaveChanges();

                //Models.GalleryContent content = new Models.GalleryContent
                //{
                //    GalleryId = gallery.GalleryId,
                //    Name = "Gallery Microsite",
                //    ShortDescription = "",
                //    Description = "",
                //    LangShortName = ms.LangShortName,
                //    CreatedBy = uid,
                //    CreatedDate = DateTime.Now,
                //    LastUpdatedBy = uid,
                //    LastUpdatedDate = DateTime.Now,
                //    IsDeleted = false
                //};

                //DbContext.GalleryContents.Add(content);
                //DbContext.SaveChanges();

                //#endregion Add Gallery
                msId = microsite.MsId;
                return new CUDReturnMessage(ResponseCode.MicrositeMngt_SuccessCreate);
           
        }


        public CUDReturnMessage UpdateMicrosite(MicrositeContentContract ms)
        {
            var microsite = DbContext.Microsites.SingleOrDefault(x => x.MsId == ms.MsId);

          
            microsite.ReferenceCode = ms.ReferenceCode;
            microsite.Name = ms.Title;
            microsite.LastUpdatedBy = uid;
            microsite.LastUpdatedDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.MicrositeMngt_SuccessUpdate);

        }
        
        public CUDReturnMessage UpdateMicrositeStatus(int msId, int status)
        {
            var ms = DbContext.Microsites.SingleOrDefault(r => r.MsId == msId && r.Status != status);

            if (ms == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            ms.Status = status;
            ms.LastUpdatedBy = uid;
            ms.LastUpdatedDate = DateTime.Now;

            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.MicrositeMngt_SuccessUpdate);

        }

        /// <summary>
        /// xóa microsite
        /// </summary>
        /// <param name="msId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteMicrosite(int msId)
        {
            var ms = DbContext.Microsites.SingleOrDefault(r => r.MsId == msId && r.IsDeleted == false);

            if (ms == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            ms.IsDeleted = true;
            ms.LastUpdatedBy = uid;
            ms.LastUpdatedDate = DateTime.Now;

            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.MicrositeMngt_SuccessDelete);
        }        
    }
}
