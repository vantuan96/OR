using Contract.Microsite;
using Contract.Shared;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using VG.Common;

namespace Business.Core
{
    public interface IMicrositeMngtBusiness
    {
        List<MicrositeContract> GetListMicrosite(int userId);

        MicrositeDetailContract GetMicrositeDetail(int msId);

        CUDReturnMessage CreateUpdateMicrosite(MicrositeContentContract ms);

        CUDReturnMessage UpdateMicrositeStatus(int msId, int status);

        CUDReturnMessage DeleteMicrosite(int msId);
    }

    public class MicrositeMngtBusiness : BaseBusiness, IMicrositeMngtBusiness
    {
        private Lazy<IMicrositeDataAccess> lazyMicrositeDataAccess;

        public MicrositeMngtBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyMicrositeDataAccess = new Lazy<IMicrositeDataAccess>(() => new MicrositeDataAccess(appid, uid));
        }

        public List<MicrositeContract> GetListMicrosite(int userId)
        {
            var query = lazyMicrositeDataAccess.Value.GetListMicrosite(null, 0, null, null, 0, null, null, userId);

            return query.Select(r => new MicrositeContract
            {
                MsId = r.MsId,
                Title = r.Name,
                Status = r.Status,
                IsRootSite = r.IsRootSite,
                ReferenceCode = r.ReferenceCode,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate
            }).ToList();
        }

        public MicrositeDetailContract GetMicrositeDetail(int msId)
        {
            var ms = lazyMicrositeDataAccess.Value.GetMicrositeById(msId);

            if (ms != null)
            {
                var detail = new MicrositeDetailContract
                {
                    MsId = ms.MsId,
                    Status = ms.Status,
                    IsRootSite = ms.IsRootSite,
                    ReferenceCode = ms.ReferenceCode,

                    CreatedBy = ms.CreatedBy,
                    CreatedDate = ms.CreatedDate,
                    LastUpdatedBy = ms.LastUpdatedBy,
                    LastUpdatedDate = ms.LastUpdatedDate,
                    ListMicrositeContent = new List<MicrositeContentContract>()
                };

                return detail;
            }

            return null;
        }

        public CUDReturnMessage CreateUpdateMicrosite(MicrositeContentContract ms)
        {
            if (string.IsNullOrEmpty(ms.Rewrite))
            {
                ms.Rewrite = ms.Title.UnicodeToKoDauAndGach();
            }

            //Update thông tin microsite
            if (ms.MsId > 0)
            {
                return lazyMicrositeDataAccess.Value.UpdateMicrosite(ms);
            }

            if (string.IsNullOrEmpty(ms.LangShortName))
            {
                ms.LangShortName = defaultLanguageCode;
            }

            //tạo mới microsite
            var galleryKey = "MicrositeGallery";
            var articleKeys = new string[] { "Page_BusinessMeeting", "page_loyalty" };

            ms.GalleryKey = galleryKey; // Gallery mặc định của mỗi microsite
            int msId = 0;
            var result = lazyMicrositeDataAccess.Value.CreateMicrosite(ms, out msId);

            return result;
        }

        public CUDReturnMessage UpdateMicrositeStatus(int msId, int status)
        {
            return lazyMicrositeDataAccess.Value.UpdateMicrositeStatus(msId, status);
        }

        public CUDReturnMessage DeleteMicrosite(int msId)
        {
            return lazyMicrositeDataAccess.Value.DeleteMicrosite(msId);
        }
    }
}