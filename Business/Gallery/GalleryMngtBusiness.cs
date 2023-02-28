using System;
using System.Collections.Generic;
using System.Linq;
using BMS.Contract.Gallery;
using BMS.Contract.Image;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;

namespace BMS.Business.Gallery
{
    public interface IGalleryMngtBusiness
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

    public class GalleryMngtBusiness : BaseBusiness, IGalleryMngtBusiness
    {
        private Lazy<IGalleryDataAccess> lazyGalleryDataAccess;
        private Lazy<IImageDataAccess> lazyImageDataAccess;

        public GalleryMngtBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyGalleryDataAccess = new Lazy<IGalleryDataAccess>(() => new GalleryDataAccess(appid, uid));
            lazyImageDataAccess = new Lazy<IImageDataAccess>(() => new ImageDataAccess(appid, uid));
        }

        public CUDReturnMessage CreateUpdateGalleryContent(GalleryContentContract model)
        {
            if (model.GalleryId == 0)
            {
                model.LangShortName = defaultLanguageCode;
                return lazyGalleryDataAccess.Value.CreateGallery(model);
            }
            else if (model.GalleryContentId == 0)
                return lazyGalleryDataAccess.Value.CreateGalleryContent(model);
            else
                return lazyGalleryDataAccess.Value.UpdateGalleryContent(model);
        }

        public CUDReturnMessage AddGalleryImage(int galleryId, string imageUrl)
        {
            var image = new ImageContract { ImageUrl = imageUrl, TargetUrl = "" };
            var listImage = lazyImageDataAccess.Value.CreateImages(new List<ImageContract> { image });
            if (listImage == null || listImage.Count == 0) return new CUDReturnMessage(ResponseCode.Error);

            var result = lazyGalleryDataAccess.Value.CreateGalleryImages(galleryId, listImage);
            result.ReturnData = listImage[0].ImageId.ToString();

            return result;
        }

        public CUDReturnMessage CreateGalleryImages(int galleryId, List<ImageContract> images)
        {
            var listImage = lazyImageDataAccess.Value.CreateImages(images);
            if (listImage == null || listImage.Count == 0) return new CUDReturnMessage(ResponseCode.Error);

            var result = lazyGalleryDataAccess.Value.CreateGalleryImages(galleryId, listImage);
            result.ReturnData = listImage[0].ImageId.ToString();

            return result;
        }

        public List<ImageContentContract> GetImageContent(int imageId)
        {
            List<ImageContentContract> result = new List<ImageContentContract>();
            var query = lazyImageDataAccess.Value.GetImageContent(imageId);

            var listContent = query.Select(r => new ImageContentContract
            {
                AltText = r.AltText,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                Description = r.Description,
                ImageContentId = r.ImageContentId,
                ImageId = r.ImageId,
                LangShortName = r.LangShortName,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate,
                MetaDescription = r.MetaDescription,
                MetaKeyword = r.MetaKeyword,
                MetaTitle = r.MetaTitle,
                Title = r.Title,
                ShortDescription = r.ShortDescription,
                TargetUrl = r.TargetUrl
            }).ToList();

            foreach (var lang in listApprovedLanguage)
            {
                var first = listContent.Where(r => r.LangShortName == lang).FirstOrDefault();
                if (first == null)
                {
                    result.Add(new ImageContentContract
                    {
                        AltText = "",
                        CreatedBy = uid,
                        CreatedDate = DateTime.Now,
                        Description = "",
                        ImageContentId = 0,
                        ImageId = imageId,
                        LangShortName = lang,
                        LastUpdatedBy = uid,
                        LastUpdatedDate = DateTime.Now,
                        MetaDescription = "",
                        MetaKeyword = "",
                        MetaTitle = "",
                        Title = "",
                        ShortDescription = "",
                        TargetUrl = ""
                    });
                }
                else
                {
                    result.Add(first);
                }
            }

            return result;
        }

        public CUDReturnMessage CreateUpdateImageContent(List<ImageContentContract> imageContents)
        {
            CUDReturnMessage result = new CUDReturnMessage();
            var contentToCreate = imageContents.Where(r => r.ImageContentId == 0 && r.IsNotSetValue == false).ToList();
            if (contentToCreate != null && contentToCreate.Any())
            {
                result = lazyImageDataAccess.Value.CreateImageContent(contentToCreate);
                if (result.Id != (int)ResponseCode.GalleryMngt_SuccessUpdateImage)
                    return result;
            }

            var contentToUpdate = imageContents.Where(r => r.ImageContentId > 0).ToList();
            if (contentToUpdate != null && contentToUpdate.Any())
            {
                result = lazyImageDataAccess.Value.UpdateImageContent(contentToUpdate);
            }

            return result;
        }

        public CUDReturnMessage DeleteGallery(int galleryId)
        {
            return lazyGalleryDataAccess.Value.DeleteGallery(galleryId);
        }

        public CUDReturnMessage DeleteGalleryImage(int galleryId, int imageId)
        {
            return lazyGalleryDataAccess.Value.DeleteGalleryImage(galleryId, imageId);
        }

        public GalleryDetailContract GetGalleryById(int galleryId)
        {
            var query = lazyGalleryDataAccess.Value.GetGalleryById(galleryId);

            var detail = query.Select(item => new GalleryDetailContract
            {
                ApprovalStatus = item.ApprovalStatus,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                GalleryId = item.GalleryId,
                IsOnsite = item.IsOnsite,
                LastUpdatedBy = item.LastUpdatedBy,
                LastUpdatedDate = item.LastUpdatedDate,
                MsId = item.MsId,                
                IsPredefined = item.IsPredefined,
                Key  = item.Key,
                Name = item.GalleryContents.FirstOrDefault(x=>x.LangShortName==defaultLanguageCode).Name,
                Images = item.GalleryImageMappings.Where(r => r.IsDeleted == false && r.Image.IsDeleted == false)
                .OrderBy(x => x.Sort).ThenBy(x => x.Id)
                .Select(r => new ImageContract
                {
                    ApprovalStatus = r.Image.ApprovalStatus,
                    CreatedBy = r.Image.CreatedBy,
                    CreatedDate = r.Image.CreatedDate,
                    ImageId = r.ImageId,
                    ImageUrl = r.Image.ImageUrl,
                    IsOnsite = r.Image.IsOnsite,
                    MediaType = r.Image.MediaType,
                    VideoUrl=r.Image.VideoUrl,
                    LastUpdatedBy = r.Image.LastUpdatedBy,
                    LastUpdatedDate = r.Image.LastUpdatedDate,
                    TargetUrl = r.Image.TargetUrl
                }).ToList()
            }).SingleOrDefault();

            if (detail == null) return null;

            detail.ListContent = new List<GalleryContentContract>();
            foreach (var lang in listApprovedLanguage)
            {
                var content = query.Select(item => item.GalleryContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang))).First();
                if (content == null)
                {
                    detail.ListContent.Add(new GalleryContentContract
                    {
                        GalleryId = detail.GalleryId,
                        GalleryContentId = 0,
                        Description = "",
                        LangShortName = lang,
                        MetaDescription = "",
                        MetaKeyword = "",
                        MetaTitle = "",
                        MsId = detail.MsId,
                        Name = "",
                        RewriteUrl = "",
                        ShortDescription = "",
                        CreatedBy = uid,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = uid,
                        LastUpdatedDate = DateTime.Now
                    });
                }
                else
                {
                    detail.ListContent.Add(new GalleryContentContract
                    {
                        GalleryId = content.GalleryId,
                        GalleryContentId = content.GalleryContentId,
                        Description = content.Description,
                        LangShortName = content.LangShortName,
                        MetaDescription = content.MetaDescription,
                        MetaKeyword = content.MetaKeyword,
                        MetaTitle = content.MetaTitle,
                        MsId = content.Gallery.MsId,
                        Name = content.Name,
                        RewriteUrl = content.RewriteUrl,
                        ShortDescription = content.ShortDescription,
                        CreatedBy = content.CreatedBy,
                        CreatedDate = content.CreatedDate,
                        LastUpdatedBy = content.LastUpdatedBy,
                        LastUpdatedDate = content.LastUpdatedDate
                    });
                }
            }

            return detail;
        }

        public GalleryContentContract GetGalleryContentById(int galleryContentId)
        {
            var query = lazyGalleryDataAccess.Value.GetGalleryContentById(galleryContentId);

            return query.Select(content => new GalleryContentContract
            {
                GalleryId = content.GalleryId,
                GalleryContentId = content.GalleryContentId,
                Description = content.Description,
                LangShortName = content.LangShortName,
                MetaDescription = content.MetaDescription,
                MetaKeyword = content.MetaKeyword,
                MetaTitle = content.MetaTitle,
                MsId = content.Gallery.MsId,
                Name = content.Name,
                RewriteUrl = content.RewriteUrl,
                ShortDescription = content.ShortDescription,
                CreatedBy = content.CreatedBy,
                CreatedDate = content.CreatedDate,
                LastUpdatedBy = content.LastUpdatedBy,
                LastUpdatedDate = content.LastUpdatedDate
            }).SingleOrDefault();
        }

        public PagedList<GalleryContract> GetListGallery(string keyword, string galleryKey, int MsId, int page, int pageSize)
        {
            var query = lazyGalleryDataAccess.Value.GetListGallery(keyword, galleryKey, null, MsId);
            var result = new PagedList<GalleryContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderBy(r => r.GalleryId).Skip((page - 1) * pageSize).Take(pageSize).Select(gallery => new GalleryContract
                {
                    ApprovalStatus = gallery.ApprovalStatus,
                    CreatedBy = gallery.CreatedBy,
                    CreatedDate = gallery.CreatedDate,
                    GalleryId = gallery.GalleryId,
                    Key = gallery.Key,
                    IsOnsite = gallery.IsOnsite,
                    LastUpdatedBy = gallery.LastUpdatedBy,
                    LastUpdatedDate = gallery.LastUpdatedDate,
                    MsId = gallery.MsId,
                    IsPredefined = gallery.IsPredefined,
                    Name = gallery.GalleryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                    Images = gallery.GalleryImageMappings.Where(r => r.IsDeleted == false && r.Image.IsDeleted == false)
                    .OrderBy(x => x.Sort).ThenBy(x => x.Id)
                    .Select(r => new ImageContract
                        {
                            ApprovalStatus = r.Image.ApprovalStatus,
                            CreatedBy = r.Image.CreatedBy,
                            CreatedDate = r.Image.CreatedDate,
                            ImageId = r.ImageId,
                            ImageUrl = r.Image.ImageUrl,
                            IsOnsite = r.Image.IsOnsite,
                            MediaType = r.Image.MediaType,
                            VideoUrl = r.Image.VideoUrl,
                            LastUpdatedBy = r.Image.LastUpdatedBy,
                            LastUpdatedDate = r.Image.LastUpdatedDate,
                            TargetUrl = r.Image.TargetUrl
                        }).ToList()
                }).ToList();
            }

            return result;
        }

        public CUDReturnMessage UpdateGalleryApprovalStatus(int galleryId, int Status, bool isOnsite, string key)
        {
            return lazyGalleryDataAccess.Value.UpdateGalleryApprovalStatus(galleryId, Status, isOnsite, key);
        }

        public CUDReturnMessage UpdateGalleryContent(GalleryContentContract model)
        {
            return lazyGalleryDataAccess.Value.UpdateGalleryContent(model);
        }

        public CUDReturnMessage UpdateGalleryIsOnsite(int galleryId, bool isOnsite)
        {
            return lazyGalleryDataAccess.Value.UpdateGalleryIsOnsite(galleryId, isOnsite);
        }

        public CUDReturnMessage UpdateImage(ImageContract image)
        {
            return lazyImageDataAccess.Value.UpdateImage(image);
        }

        /// <summary>
        /// Cập nhật thứ tự ảnh/video trong gallery
        /// </summary>
        /// <param name="galleryId"></param>
        /// <param name="listImages"></param>
        /// <returns></returns>
        public CUDReturnMessage UpdateGalleryItemOrder(int galleryId, List<ImageContract> listImages)
        {            
            return lazyGalleryDataAccess.Value.UpdateGalleryItemOrder(galleryId, listImages);
        }

        #region FE

        /// <summary>
        /// lấy danh sách ảnh trong gallery để hiển thị FE
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Contract.FE.GalleryContract GetGallery(int id, string key, string lang, int MsId)
        {
            IQueryable<DataAccess.Models.Gallery> query;

            if (id > 0)
                query = lazyGalleryDataAccess.Value.GetGalleryById(id);
            else
                query = lazyGalleryDataAccess.Value.GetListGallery(null, null, key, MsId);                


            var gallery = query.Select(r => new Contract.FE.GalleryContract
            {
                GalleryId = r.GalleryId,
                Content = r.GalleryContents.Where(c => c.IsDeleted == false && c.LangShortName == lang).Select(c => new Contract.FE.GalleryContentContract
                {
                    Name = c.Name,
                    Description = c.Description,
                    ShortDescription = c.ShortDescription,
                    MetaTitle = c.MetaTitle,
                    MetaDescription = c.MetaDescription,
                    MetaKeyword = c.MetaKeyword
                }).FirstOrDefault(),
            }).FirstOrDefault();

            if (gallery != null)
            {
                gallery.ListItem = lazyGalleryDataAccess.Value.GetListImage(gallery.GalleryId).OrderBy(r => r.Sort).Select(r => new Contract.FE.GalleryItemContract
                {
                    ImageId = r.ImageId,
                    ImageUrl = r.Image.ImageUrl,
                    TargetUrl = r.Image.TargetUrl,
                    VideoUrl = r.Image.VideoUrl,
                    MediaType = r.Image.MediaType,
                    Content = r.Image.ImageContents.Where(c => c.IsDeleted == false && c.LangShortName == lang).Select(c => new Contract.FE.GalleryItemContentContract
                    {
                        Title = c.Title,
                        Description = c.Description,
                        ShortDescription = c.ShortDescription,
                        MetaTitle = c.MetaTitle,
                        MetaDescription = c.MetaDescription,
                        MetaKeyword = c.MetaKeyword,
                        TargetUrl = c.TargetUrl,
                        AltText = c.AltText
                    }).FirstOrDefault()
                }).ToList();
            }

            return gallery;            
        }

        #endregion FE
    }
}