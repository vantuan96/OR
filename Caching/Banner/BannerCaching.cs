using System;
using System.Collections.Generic;
using BMS.Business.Banner;
using BMS.Contract.Banner;
using BMS.Contract.Shared;

namespace BMS.Caching.Banner
{
    public interface IBannerCaching
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

    public class BannerCaching : BaseCaching, IBannerCaching
    {
        private Lazy<BannerBusiness> lazy;

        public BannerCaching(/*string appid, int uid*/)  
        {
            lazy = new Lazy<BannerBusiness>(() =>
            {
                var instance = new BannerBusiness(appid, uid);
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
            return lazy.Value.Get(name, startDate, endDate, bannerGroupId, positionId, msId, statuses, pageSize, pageNumber);
        }

        /// <summary>
        /// Tìm banner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msId"></param>
        /// <returns></returns>
        public BannerContract Find(int id, int msId)
        {
            return lazy.Value.Find(id, msId);
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
            return lazy.Value.FindBannerContent(id);
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
            return lazy.Value.Get(name, bannerGroupId, positionId, positionName, msId, lang);
        }
        #endregion FE
    }
}