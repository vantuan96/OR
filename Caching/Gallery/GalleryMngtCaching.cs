using System;
using System.Collections.Generic;
using BMS.Business.Gallery;
using BMS.Contract.Gallery;
using BMS.Contract.Image;
using BMS.Contract.Shared;

namespace BMS.Caching.Gallery
{
    public interface IGalleryMngtCaching
    {
        PagedList<GalleryContract> GetListGallery(string keyword, string galleryKey, int MsId, int page, int pageSize);

        GalleryDetailContract GetGalleryById(int galleryId);

        GalleryContentContract GetGalleryContentById(int galleryContentId);

        CUDReturnMessage CreateUpdateGalleryContent(GalleryContentContract model);

        CUDReturnMessage UpdateGalleryApprovalStatus(int galleryId, int Status, bool isOnsite, string key);

        CUDReturnMessage UpdateGalleryIsOnsite(int galleryId, bool isOnsite);

        CUDReturnMessage DeleteGallery(int galleryId);

        CUDReturnMessage AddGalleryImage(int galleryId, string imageUrl);

        CUDReturnMessage CreateGalleryImages(int galleryId, List<ImageContract> images);

        CUDReturnMessage DeleteGalleryImage(int galleryId, int imageId);

        CUDReturnMessage UpdateImage(ImageContract image);

        List<ImageContentContract> GetImageContent(int imageId);

        CUDReturnMessage CreateUpdateImageContent(List<ImageContentContract> imageContents);

        /// <summary>
        /// Cập nhật thứ tự ảnh/video trong gallery
        /// </summary>
        /// <param name="galleryId"></param>
        /// <param name="listImages"></param>
        /// <returns></returns>
        CUDReturnMessage UpdateGalleryItemOrder(int galleryId, List<ImageContract> listImages);

        #region FE

        /// <summary>
        /// lấy danh sách ảnh trong gallery để hiển thị FE
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Contract.FE.GalleryContract GetGallery(int id, string key, string lang, int MsId);

        #endregion FE
    }

    public class GalleryMngtCaching : BaseCaching, IGalleryMngtCaching
    {
        private Lazy<IGalleryMngtBusiness> lazyGalleryBusiness;

        public GalleryMngtCaching(/*string appid, int uid*/)  
        {
            lazyGalleryBusiness = new Lazy<IGalleryMngtBusiness>(() => new GalleryMngtBusiness(appid, uid));
        }

        public CUDReturnMessage AddGalleryImage(int galleryId, string imageUrl)
        {
            return lazyGalleryBusiness.Value.AddGalleryImage(galleryId, imageUrl);
        }

        public CUDReturnMessage CreateGalleryImages(int galleryId, List<ImageContract> images)
        {
            return lazyGalleryBusiness.Value.CreateGalleryImages(galleryId, images);
        }

        public CUDReturnMessage CreateUpdateGalleryContent(GalleryContentContract model)
        {
            return lazyGalleryBusiness.Value.CreateUpdateGalleryContent(model);
        }

        public List<ImageContentContract> GetImageContent(int imageId)
        {
            return lazyGalleryBusiness.Value.GetImageContent(imageId);
        }

        public CUDReturnMessage CreateUpdateImageContent(List<ImageContentContract> imageContents)
        {
            return lazyGalleryBusiness.Value.CreateUpdateImageContent(imageContents);
        }

        public CUDReturnMessage DeleteGallery(int galleryId)
        {
            return lazyGalleryBusiness.Value.DeleteGallery(galleryId);
        }

        public CUDReturnMessage DeleteGalleryImage(int galleryId, int imageId)
        {
            return lazyGalleryBusiness.Value.DeleteGalleryImage(galleryId, imageId);
        }

        public GalleryDetailContract GetGalleryById(int galleryId)
        {
            return lazyGalleryBusiness.Value.GetGalleryById(galleryId);
        }

        public GalleryContentContract GetGalleryContentById(int galleryContentId)
        {
            return lazyGalleryBusiness.Value.GetGalleryContentById(galleryContentId);
        }

        public PagedList<GalleryContract> GetListGallery(string keyword, string galleryKey, int MsId, int page, int pageSize)
        {
            return lazyGalleryBusiness.Value.GetListGallery(keyword, galleryKey, MsId, page, pageSize);
        }

        public CUDReturnMessage UpdateGalleryApprovalStatus(int galleryId, int Status, bool isOnsite, string key)
        {
            return lazyGalleryBusiness.Value.UpdateGalleryApprovalStatus(galleryId, Status, isOnsite, key);
        }

        public CUDReturnMessage UpdateGalleryIsOnsite(int galleryId, bool isOnsite)
        {
            return lazyGalleryBusiness.Value.UpdateGalleryIsOnsite(galleryId, isOnsite);
        }

        public CUDReturnMessage UpdateImage(ImageContract image)
        {
            return lazyGalleryBusiness.Value.UpdateImage(image);
        }

        /// <summary>
        /// Cập nhật thứ tự ảnh/video trong gallery
        /// </summary>
        /// <param name="galleryId"></param>
        /// <param name="listImages"></param>
        /// <returns></returns>
        public CUDReturnMessage UpdateGalleryItemOrder(int galleryId, List<ImageContract> listImages)
        {
            return lazyGalleryBusiness.Value.UpdateGalleryItemOrder(galleryId, listImages);
        }

        #region FE

        /// <summary>
        /// lấy danh sách ảnh trong gallery để hiển thị FE
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Contract.FE.GalleryContract GetGallery(int id, string key, string lang, int MsId)
        {
            return lazyGalleryBusiness.Value.GetGallery(id, key, lang, MsId);
        }

        #endregion FE
    }
}