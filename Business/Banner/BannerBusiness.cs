using System;
using System.Collections.Generic;
using System.Linq;
using BMS.Contract.Banner;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;

namespace BMS.Business.Banner
{
    public interface IBannerBusiness
    {
        #region Banner position

        /// <summary>
        /// Lấy tất cả vị trí banner
        /// </summary>
        /// <returns></returns>
        IEnumerable<BannerPositionContract> GetBannerPosition(int msId, int[] statuses);

        /// <summary>
        /// Tìm vị trí banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BannerPositionContract FindBannerPosition(int id, int msId);

        /// <summary>
        /// Thêm mới hoặc cập nhật vị trí banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertOrUpdateBannerPosition(BannerPositionContract model);

        #endregion Banner position

        #region Banner group

        /// <summary>
        /// Lấy tất cả nhóm banner
        /// </summary>
        /// <returns></returns>
        IEnumerable<BannerGroupContract> GetBannerGroup(int msId, int[] statuses);

        /// <summary>
        /// Tìm nhóm banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BannerGroupContract FindBannerGroup(int id, int msId);

        /// <summary>
        /// Thêm mới hoặc cập nhật nhóm banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertOrUpdateBannerGroup(BannerGroupContract model);

        #endregion Banner group

        #region Banner

        /// <summary>
        /// Tìm kiếm banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PagedList<BannerListItemContract> Get(string name, DateTime startDate, DateTime endDate,
            int bannerGroupId, int positionId, int msId, int[] statuses, int pageSize, int pageNumber);


        

        /// <summary>
        /// Tìm banner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msId"></param>
        /// <returns></returns>
        BannerContract Find(int id, int msId);

        /// <summary>
        /// Tạo banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertOrUpdateBanner(BannerContract model);

        /// <summary>
        /// Xem chi tiết banner content
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BannerContentContract FindBannerContent(int id);

        /// <summary>
        /// Tạo và cập nhật nội dung banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertOrUpdateBannerContent(BannerContentContract model);

        #endregion Banner

        #region FE

        /// <summary>
        /// danh sách onsite
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bannerGroupId"></param>
        /// <param name="positionId"></param>
        /// <param name="positionName"></param>
        /// <param name="msId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        List<Contract.FE.BannerContract> Get(string name, int bannerGroupId, int positionId, string positionName, int msId, string lang);

        #endregion FE
    }

    public class BannerBusiness : BaseBusiness, IBannerBusiness
    {
        private Lazy<IBannerDataAccess> lazy;

        public BannerBusiness(string appid, int uid) : base(appid, uid)
        {
            lazy = new Lazy<IBannerDataAccess>(() =>
            {
                var instance = new BannerDataAccess(appid, uid);
                return instance;
            });
        }

        #region Banner position

        /// <summary>
        /// Lấy tất cả vị trí banner
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BannerPositionContract> GetBannerPosition(int msId, int[] statuses)
        {
            return lazy.Value.GetBannerPosition(msId, statuses);
        }

        /// <summary>
        /// Tìm vị trí banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BannerPositionContract FindBannerPosition(int id, int msId)
        {
            return lazy.Value.FindBannerPosition(id, msId);
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật vị trí banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdateBannerPosition(BannerPositionContract model)
        {
            return lazy.Value.InsertOrUpdateBannerPosition(model);
        }

        #endregion Banner position

        #region Banner group

        /// <summary>
        /// Lấy tất cả nhóm banner
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BannerGroupContract> GetBannerGroup(int msId, int[] statuses)
        {
            return lazy.Value.GetBannerGroup(msId, statuses);
        }

        /// <summary>
        /// Tìm nhóm banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BannerGroupContract FindBannerGroup(int id, int msId)
        {
            return lazy.Value.FindBannerGroup(id, msId);
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật nhóm banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdateBannerGroup(BannerGroupContract model)
        {
            return lazy.Value.InsertOrUpdateBannerGroup(model);
        }

        #endregion Banner group

        #region Banner

        /// <summary>
        /// Tìm kiếm banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PagedList<BannerListItemContract> Get(string name, DateTime startDate, DateTime endDate,
            int bannerGroupId, int positionId, int msId, int[] statuses, int pageSize, int pageNumber)
        {
            var banners = lazy.Value.Get(name, startDate, endDate, bannerGroupId, positionId, msId, statuses, null, null);

            var result = new PagedList<BannerListItemContract>();
            result.Count = banners.Count();
            result.List = banners.OrderByDescending(r => r.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(b => new BannerListItemContract
                {
                    Id = b.Id,
                    PositionId = b.PositionId,
                    BannerGroupId = b.BannerGroupId,
                    Name = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Name,
                    TargetUrl = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).TargetUrl,
                    BannerUrl = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).BannerUrl,
                    BannerUrlMobile = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).BannerUrlMobile,
                    ImageWitdh = b.ImageWitdh,
                    ImageHeight = b.ImageHeight,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status,
                    IsOnsite = b.IsOnsite,
                    ImageWidthNatural = b.ImageWidthNatural,
                    ImageHeightNatural = b.ImageHeightNatural,
                    MsId = b.MsId,
                    CreatedBy = b.CreatedBy,
                    CreatedDate = b.CreatedDate,
                    LastUpdatedBy = b.LastUpdatedBy,
                    LastUpdatedDate = b.LastUpdatedDate,
                    BannerGroupName = b.BannerGroup.Name,
                    //BannerPositionName = b.BannerPosition.Name,
                    Sort = b.Sort,
                    PositionIds = b.BannerPositionMappings==null ? new List<int>() { -1 } : b.BannerPositionMappings.Where(p => !p.IsDeleted).Select(p=>p.PositionId).ToList()
                }
                ).ToList();

            return result;
        }

        



        /// <summary>
        /// Tìm banner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msId"></param>
        /// <returns></returns>
        public BannerContract Find(int id, int msId)
        {
            var b = lazy.Value.Find(id, msId);

            var banner = new BannerContract()
            {
                Id = b.Id,
                PositionId = b.PositionId,
                BannerGroupId = b.BannerGroupId,
                Name = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Name,
                TargetUrl = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).TargetUrl,
                BannerUrl = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).BannerUrl,
                BannerUrlMobile = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).BannerUrlMobile,
                Description = b.BannerContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Description,
                ImageWitdh = b.ImageWitdh,
                ImageHeight = b.ImageHeight,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Status = b.Status,
                IsOnsite = b.IsOnsite,
                ImageWidthNatural = b.ImageWidthNatural,
                ImageHeightNatural = b.ImageHeightNatural,
                MsId = b.MsId,
                CreatedBy = b.CreatedBy,
                CreatedDate = b.CreatedDate,
                BannerContents = new List<BannerContentContract>(),
                Sort = b.Sort,
                PositionIds = b.BannerPositionMappings == null ? new List<int>() { -1 } : b.BannerPositionMappings.Where(p => !p.IsDeleted).Select(p => p.PositionId  ).ToList()
            };
            foreach (var lang in listApprovedLanguage)
            {
                var content = b.BannerContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang));
                if (content == null)
                {
                    banner.BannerContents.Add(new BannerContentContract
                    {
                        ContentId = 0,
                        BannerId = banner.Id,
                        Name = "",
                        TargetUrl = "",
                        BannerUrl = "",
                        BannerUrlMobile = "",
                        Description = "",
                        LangShortName = lang,
                        ApprovalStatus = 0
                    });
                }
                else
                {
                    banner.BannerContents.Add(new BannerContentContract
                    {
                        ContentId = content.ContentId,
                        BannerId = content.BannerId ?? 0,
                        Name = content.Name,
                        TargetUrl = content.TargetUrl,
                        BannerUrl = content.BannerUrl,
                        BannerUrlMobile = content.BannerUrlMobile,
                        Description = content.Description,
                        LangShortName = content.LangShortName,
                        ApprovalStatus = content.ApprovalStatus
                    });
                }
            }
            return banner;
        }

        /// <summary>
        /// Tạo banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdateBanner(BannerContract model)
        {
            return lazy.Value.InsertOrUpdateBanner(model);
        }

        /// <summary>
        /// Xem chi tiết banner content
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BannerContentContract FindBannerContent(int id)
        {
            var b = lazy.Value.FindBannerContent(id);

            var bannerContent = new BannerContentContract()
            {
                ContentId = b.ContentId,
                BannerId = b.BannerId ?? 0,
                Name = b.Name,
                TargetUrl = b.TargetUrl,
                BannerUrl = b.BannerUrl,
                BannerUrlMobile = b.BannerUrlMobile,
                Description = b.Description,
                LangShortName = b.LangShortName,
                CreatedBy = b.CreatedBy,
                CreatedDate = b.CreatedDate,
                ApprovalStatus = b.ApprovalStatus
            };

            return bannerContent;
        }

        /// <summary>
        /// Tạo và cập nhật nội dung banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdateBannerContent(BannerContentContract model)
        {
            return lazy.Value.InsertOrUpdateBannerContent(model);
        }

        #endregion Banner


        #region FE

        /// <summary>
        /// danh sách onsite
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bannerGroupId"></param>
        /// <param name="positionId"></param>
        /// <param name="positionName"></param>
        /// <param name="msId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public List<Contract.FE.BannerContract> Get(string name, int bannerGroupId, int positionId, string positionName, int msId, string lang)
        {

            var viewDate = DateTime.Now;
            var banners = lazy.Value.Get(name, DateTime.MinValue, DateTime.MaxValue, bannerGroupId, positionId, msId, new int[]{ (int)ObjectStatus.Onsite }, positionName, null);

            if (banners != null)
            {
                return banners.OrderBy(b => b.Sort).Select(b => new Contract.FE.BannerContract
                {
                    Id = b.Id,
                    PositionId = b.PositionId,
                    BannerGroupId = b.BannerGroupId,
                    ImageWitdh = b.ImageWitdh,
                    ImageHeight = b.ImageHeight,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status,
                    IsOnsite = b.IsOnsite,
                    ImageWidthNatural = b.ImageWidthNatural,
                    ImageHeightNatural = b.ImageHeightNatural,
                    MsId = b.MsId,
                    CreatedBy = b.CreatedBy,
                    CreatedDate = b.CreatedDate,
                    LastUpdatedBy = b.LastUpdatedBy,
                    LastUpdatedDate = b.LastUpdatedDate,
                    BannerGroupName = b.BannerGroup.Name,
                    //BannerPositionName = b.BannerPosition.Name,
                    Sort = b.Sort,
                    BannerContent = b.BannerContents.Where(r => r.IsDeleted == false && r.LangShortName == lang && r.ApprovalStatus == (int)ApprovalStatus.Approved).Select(r => new Contract.FE.BannerContentContract
                    {
                        Name = r.Name,
                        TargetUrl = r.TargetUrl,
                        BannerUrl = r.BannerUrl,
                        BannerUrlMobile = r.BannerUrlMobile,
                        Description = r.Description,
                        Id = r.ContentId,
                        CreatedBy = r.CreatedBy,
                        CreatedDate = r.CreatedDate,
                        ApprovalStatus = r.ApprovalStatus,
                        LastUpdatedBy = r.LastUpdatedBy,
                        LastUpdatedDate = r.LastUpdatedDate,
                        LangShortName = r.LangShortName
                    }).FirstOrDefault()
                }).ToList();
            }

            return new List<Contract.FE.BannerContract>() ;
        }
            

        #endregion
    }
}