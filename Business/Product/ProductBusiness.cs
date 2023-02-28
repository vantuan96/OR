using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VG.Common;
using BMS.Contract.Image;
using BMS.Contract.Product;
using BMS.Contract.ProductCategory;
using BMS.Contract.ProductType;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;

namespace BMS.Business.Product
{
    public interface IProductBusiness
    {
        #region Product attribute

        /// <summary>
        /// Tìm kiếm thuộc tính sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ProductAttributeContract> GetProductAttribute(string name, int[] statuses);

        /// <summary>
        /// Tìm 1 thuộc tính
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msId"></param>
        /// <returns></returns>
        ProductAttributeContract FindProductAttribute(int id);

        /// <summary>
        /// Tạo thuộc tính sản phẩm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertOrUpdateProductAttribute(ProductAttributeContract model);

        /// <summary>
        /// Xem nội dung thuộc tính sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProductAttributeContentContract FindProductAttributeContent(int id);

        /// <summary>
        /// Tạo và cập nhật nội dung thuộc tính sản phẩm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertOrUpdateProductAttributeContent(ProductAttributeContentContract model);

        #endregion Product attribute

        #region ProductCategory
        List<ProductCategoryContract> GetListProductCategory(bool? isOnsite, int approvalStatus);
        ProductCategoryDetailContract GetProductCategoryDetail(int id);
        ProductCategoryContentContract GetProductCategoryContentById(int contentId);
        CUDReturnMessage CreateUpdateProductCategoryContent(ProductCategoryContentContract pc);
        CUDReturnMessage UpdateProductCategoryStatus(int id, int status);
        CUDReturnMessage UpdateProductCategory(ProductCategoryUpdateContract pc);
        CUDReturnMessage UpdateProductCategorySort(List<UpdateProductCategorySortContract> categoryItems);

        #endregion ProductCategory

        #region ProductType

        List<ProductTypeContract> GetListProductType(int MsId, int status);

        #endregion

        #region Product

        PagedList<ProductItemContract> GetListProduct(int msid, int status, int productTypeId, int productCategoryId, string searchText, int page, int pageSize);

        CUDReturnMessage CreateProduct(ProductCreateContract model);

        CUDReturnMessage DeleteProduct(int productId);

        CUDReturnMessage UpdateProduct(ProductUpdateContract model);

        ProductDetailContract GetProductDetail(int productId);

        CreateUpdateProductContentContract GetProductContentById(int productContentId);

        CUDReturnMessage CreateUpdateProductContent(CreateUpdateProductContentContract model);

        CUDReturnMessage CreateUpdateProductAttributeMapping(int productId, List<ProductAttributeMappingContract> list);

        #endregion

        #region FE

        /// <summary>
        /// danh sách san pham
        /// </summary>
        /// <param name="msId"></param>
        /// <param name="lang"></param>
        /// <param name="productTypeId"></param>
        /// <param name="productCategoryKey"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        List<Contract.FE.ProductContract> GetListProduct(int msId, string lang, int productTypeId, string productCategoryKey, string searchText);
        #endregion FE
    }

    public class ProductBusiness : BaseBusiness, IProductBusiness
    {
        private Lazy<IProductDataAccess> lazyProductDataAccess;
        private Lazy<IProductCategoryDataAccess> lazyProductCategoryDataAccess;
        private Lazy<IProductTypeDataAccess> lazyProductTypeDataAccess;
        private Lazy<IImageDataAccess> lazyImageDataAccess;

        public ProductBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyProductDataAccess = new Lazy<IProductDataAccess>(() => new ProductDataAccess(appid, uid));
            lazyProductCategoryDataAccess = new Lazy<IProductCategoryDataAccess>(() => new ProductCategoryDataAccess(appid, uid));
            lazyProductTypeDataAccess = new Lazy<IProductTypeDataAccess>(() => new ProductTypeDataAccess(appid, uid));
            lazyImageDataAccess = new Lazy<IImageDataAccess>(() => new ImageDataAccess(appid, uid));
        }

        #region Product attribute

        /// <summary>
        /// Tìm kiếm thuộc tính sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProductAttributeContract> GetProductAttribute(string name, int[] statuses)
        {
            var data = lazyProductDataAccess.Value.GetProductAttribute(name, statuses);

            return data.Select(b => new ProductAttributeContract
            {
                AttrId = b.AttrId,
                ApprovalStatus = b.ApprovalStatus,
                ImageUrl = b.ImageUrl,
                IsThumbnail = b.IsThumbnail,
                IsRequired = b.IsRequired,
                Sort = b.Sort,
                Name = b.ProductAttributeContents
                            .SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Name,
                IsOnsite = b.IsOnsite,
                CreatedBy = b.CreatedBy,
                CreatedDate = b.CreatedDate,
                ProductAttributeContents = new List<ProductAttributeContentContract>()
            });
        }


        /// <summary>
        /// Tìm 1 thuộc tính
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msId"></param>
        /// <returns></returns>
        public ProductAttributeContract FindProductAttribute(int id)
        {
            var data = lazyProductDataAccess.Value.FindProductAttribute(id);

            var result = new ProductAttributeContract()
            {
                AttrId = data.AttrId,
                ApprovalStatus = data.ApprovalStatus,
                ImageUrl = data.ImageUrl,
                IsThumbnail = data.IsThumbnail,
                IsRequired = data.IsRequired,
                Sort = data.Sort,
                Name = data.ProductAttributeContents
                            .SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Name,
                IsOnsite = data.IsOnsite,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate,
                ProductAttributeContents = new List<ProductAttributeContentContract>()
            };

            foreach (var lang in listApprovedLanguage)
            {
                var content = data.ProductAttributeContents
                    .FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang));
                if (content == null)
                {
                    result.ProductAttributeContents.Add(new ProductAttributeContentContract
                    {
                        AttrContentId = 0,
                        AttrId = data.AttrId,
                        Name = "",
                        LangShortName = lang,
                        ApprovalStatus = 0
                    });
                }
                else
                {
                    result.ProductAttributeContents.Add(new ProductAttributeContentContract
                    {
                        AttrContentId = content.AttrContentId,
                        AttrId = content.AttrId,
                        Name = content.Name,
                        LangShortName = content.LangShortName,
                        ApprovalStatus = content.ApprovalStatus,
                        CreatedBy = content.CreatedBy,
                        CreatedDate = content.CreatedDate,
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Tạo thuộc tính sản phẩm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdateProductAttribute(ProductAttributeContract model)
        {
            return lazyProductDataAccess.Value.InsertOrUpdateProductAttribute(model);
        }

        /// <summary>
        /// Xem nội dung thuộc tính sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductAttributeContentContract FindProductAttributeContent(int id)
        {
            var data = lazyProductDataAccess.Value.FindProductAttributeContent(id);

            var result = new ProductAttributeContentContract()
            {
                AttrContentId = data.AttrContentId,
                AttrId = data.AttrId,
                Name = data.Name,
                LangShortName = data.LangShortName,
                ApprovalStatus = data.ApprovalStatus,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate
            };

            return result;
        }

        /// <summary>
        /// Tạo và cập nhật nội dung thuộc tính sản phẩm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdateProductAttributeContent(ProductAttributeContentContract model)
        {
            return lazyProductDataAccess.Value.InsertOrUpdateProductAttributeContent(model);
        }

        #endregion Product attribute


        #region ProductCategory

        public List<ProductCategoryContract> GetListProductCategory(bool? isOnsite, int approvalStatus)
        {
            var query = lazyProductCategoryDataAccess.Value.GetListProductCategory(isOnsite, approvalStatus);

            if (query != null)
            {

                var listParentId = query.Where(r => r.ParentId > 0).Distinct().Select(r => r.ParentId).ToList();


                var listParent = query.Where(r => listParentId.Contains(r.Id)).Select(r => new { Id = r.Id, Name = r.ProductCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault() }).ToDictionary(r => r.Id);


                var listCategory = query.Select(r => new ProductCategoryContract
                {
                    Id = r.Id,
                    //MsId = r.MsId,
                    Name = r.ProductCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                    ApprovalStatus = r.ApprovalStatus,
                    IsOnsite = r.IsOnsite,
                    ParentId = r.ParentId,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    LastUpdatedBy = r.LastUpdatedBy,
                    LastUpdatedDate = r.LastUpdatedDate,
                    Key = r.Key,
                    IsPredefined = r.IsPredefined
                }).ToList();

                listCategory.Where(r => r.ParentId != 0).ToList().ForEach(
                   r => r.ParentName = listParent.ContainsKey((int)r.ParentId) ? listParent[(int)r.ParentId].Name : ""
                   );


                return listCategory;
            }
            return null;

        }

        public ProductCategoryDetailContract GetProductCategoryDetail(int id)
        {
            var pc = lazyProductCategoryDataAccess.Value.GetProductCategoryById(id);

            if (pc != null)
            {
                var detail = new ProductCategoryDetailContract
                {
                    Id = pc.Id,
                    //MsId = pc.MsId,
                    ApprovalStatus = pc.ApprovalStatus,
                    IsOnsite = pc.IsOnsite,
                    ParentId = pc.ParentId,
                    CreatedBy = pc.CreatedBy,
                    CreatedDate = pc.CreatedDate,
                    LastUpdatedBy = pc.LastUpdatedBy,
                    LastUpdatedDate = pc.LastUpdatedDate,
                    ListProductCategoryContent = new List<ProductCategoryContentContract>()
                };

                foreach (var lang in listApprovedLanguage)
                {
                    var content = pc.ProductCategoryContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang));
                    if (content == null)
                    {
                        detail.ListProductCategoryContent.Add(new ProductCategoryContentContract
                        {
                            LangShortName = lang,
                            ProductCategoryId = pc.Id,
                            ContentId = 0,
                            //MsId = pc.MsId
                        });
                    }
                    else
                    {
                        detail.ListProductCategoryContent.Add(new ProductCategoryContentContract
                        {
                            //MsId = pc.MsId,
                            ContentId = content.ContentId,
                            ProductCategoryId = content.ProductCategoryId,
                            Name = content.Name,
                            Rewrite = content.Rewrite,
                            CategoryUrl = content.CategoryUrl,
                            ShortDescription = content.ShortDescription,
                            Description = content.Description,
                            LangShortName = content.LangShortName,
                            ApprovalStatus = content.ApprovalStatus,
                            CreatedBy = content.CreatedBy,
                            CreatedDate = content.CreatedDate,
                            LastUpdatedBy = content.LastUpdatedBy,
                            LastUpdatedDate = content.LastUpdatedDate
                        });
                    }
                }

                return detail;
            }

            return null;
        }

        public ProductCategoryContentContract GetProductCategoryContentById(int contentId)
        {
            var pcContent = lazyProductCategoryDataAccess.Value.GetProductCategoryContentById(contentId);

            if (pcContent != null)
            {
                var content = new ProductCategoryContentContract
                {
                    ContentId = pcContent.ContentId,
                    ProductCategoryId = pcContent.ProductCategoryId,
                    Name = pcContent.Name,
                    Rewrite = pcContent.Rewrite,
                    CategoryUrl = pcContent.CategoryUrl,
                    ShortDescription = pcContent.ShortDescription,
                    Description = pcContent.Description,
                    LangShortName = pcContent.LangShortName,
                    ApprovalStatus = pcContent.ApprovalStatus,
                    CreatedBy = pcContent.CreatedBy,
                    CreatedDate = pcContent.CreatedDate,
                    LastUpdatedBy = pcContent.LastUpdatedBy,
                    LastUpdatedDate = pcContent.LastUpdatedDate
                };

                return content;
            }
            return null;
        }


        public CUDReturnMessage CreateUpdateProductCategoryContent(ProductCategoryContentContract pc)
        {
            if (pc.ContentId > 0)
            {
                return lazyProductCategoryDataAccess.Value.UpdateProductCategoryContent(pc);
            }

            if (pc.ProductCategoryId > 0)
            {
                return lazyProductCategoryDataAccess.Value.CreateProductCategoryContent(pc);
            }

            if (string.IsNullOrEmpty(pc.LangShortName))
            {
                pc.LangShortName = defaultLanguageCode;
            }

            return lazyProductCategoryDataAccess.Value.CreateProductCategory(pc);

        }

        public CUDReturnMessage UpdateProductCategoryStatus(int id, int status)
        {
            return lazyProductCategoryDataAccess.Value.UpdateProductCategoryStatus(id, status);
        }

        public CUDReturnMessage UpdateProductCategory(ProductCategoryUpdateContract pc)
        {
            return lazyProductCategoryDataAccess.Value.UpdateProductCategory(pc);
        }

        public CUDReturnMessage UpdateProductCategorySort(List<UpdateProductCategorySortContract> categoryItems) {
            return lazyProductCategoryDataAccess.Value.UpdateProductCategorySort(categoryItems);
        }

        #endregion ProductCategory


        #region ProductType

        public List<ProductTypeContract> GetListProductType(int MsId, int status)
        {
            return lazyProductTypeDataAccess.Value.GetListProductType(MsId, status).Select(r => new ProductTypeContract
            {
                ProductTypeId = r.ProductTypeId,
                Status = r.Status,
                Name = r.ProductTypeContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate
            }).ToList();
        }

        #endregion

        #region Product

        public PagedList<ProductItemContract> GetListProduct(int msid, int status, int productTypeId, int productCategoryId, string searchText, int page, int pageSize)
        {
            var query = lazyProductDataAccess.Value.GetListProduct(msid, status, productCategoryId, searchText);
            var result = new PagedList<ProductItemContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.ProductId).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new ProductItemContract
                {
                    MsId = r.MsId,
                    ProductId = r.ProductId,
                    ProductCategoryId = r.ProductCategoryId,
                    ProductCategoryName = r.ProductCategory.ProductCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                    GalleryId = r.GalleryId,
                    ImageUrl = r.ImageUrl,
                    Status = r.Status,
                    NameLine1 = r.ProductContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.NameLine1).FirstOrDefault(),
                    NameLine2 = r.ProductContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.NameLine2).FirstOrDefault(),
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    LastUpdatedBy = r.LastUpdatedBy,
                    LastUpdatedDate = r.LastUpdatedDate
                }).ToList();
            }

            return result;
        }

        public CUDReturnMessage CreateProduct(ProductCreateContract model)
        {
            model.LangShortName = defaultLanguageCode;
            if (string.IsNullOrEmpty(model.RewriteUrl))
            {
                model.RewriteUrl = model.NameLine1.UnicodeToKoDauAndGach();
            }

            return lazyProductDataAccess.Value.CreateProduct(model);
        }

        public CUDReturnMessage DeleteProduct(int productId)
        {
            return lazyProductDataAccess.Value.DeleteProduct(productId);
        }

        public CUDReturnMessage UpdateProduct(ProductUpdateContract model)
        {   
            return lazyProductDataAccess.Value.UpdateProduct(model);
        }

        public ProductDetailContract GetProductDetail(int productId)
        {
            var query = lazyProductDataAccess.Value.GetProductById(productId);
            var detail = query.Select(r => new ProductDetailContract
            {
                ProductId = r.ProductId,
                Status = r.Status,
                ProductCategoryId = r.ProductCategoryId,
                ProductCategoryName = r.ProductCategory.ProductCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                MsId = r.MsId,
                GalleryId = r.GalleryId,
                ImageUrl = r.ImageUrl,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate
            }).SingleOrDefault();

            if (detail == null) return null;

            detail.ListAttribute = GetProductAttributes(productId);
            detail.ListContent = GetProductContent(query, productId);

            return detail;
        }

        private List<ProductAttributeMappingContract> GetProductAttributes(int productId)
        {
            var query = lazyProductDataAccess.Value.GetListAttributeByProductId(productId, null);
            var usingAttrs = query.Select(r => new ProductAttributeMappingContract
            {
                AttrId = r.AttrId,
                AttrName = r.ProductAttribute.ProductAttributeContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                ProductAttributeMappingId = r.Id,
                IsDelete = r.IsDeleted,
                LangShortName = r.LangShortName,
                Value = r.Value
            }).ToList();

            var allAttrs = lazyProductDataAccess.Value.GetProductAttribute(null, new int[] { (int)ApprovalStatus.Approved }).Select(r => new ProductAttributeMappingContract
            {
                AttrId = r.AttrId,
                AttrName = r.ProductAttributeContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                ProductAttributeMappingId = 0,
                IsDelete = true,
                LangShortName = "",
                Value = ""
            }).ToList();

            var productAttrs = new List<ProductAttributeMappingContract>();

            foreach (var attr in allAttrs)
            {
                foreach (var lang in listApprovedLanguage)
                {
                    var first = usingAttrs.Where(r => r.AttrId == attr.AttrId && r.LangShortName == lang).FirstOrDefault();

                    if (first == null)
                    {
                        productAttrs.Add(new ProductAttributeMappingContract
                        {
                            AttrId = attr.AttrId,
                            AttrName = attr.AttrName,
                            IsDelete = true,
                            LangShortName = lang,
                            ProductAttributeMappingId = 0,
                            Value = ""
                        });
                    }
                    else
                    {
                        productAttrs.Add(first);
                    }
                }
            }

            return productAttrs;
        }

        private List<ProductContentContract> GetProductContent(IQueryable<DataAccess.Models.Product> query, int productId)
        {
            var listContent = new List<ProductContentContract>();

            foreach (var lang in listApprovedLanguage)
            {
                var content = query.Select(item => item.ProductContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang))).First();
                if (content == null)
                {
                    listContent.Add(new ProductContentContract
                    {
                        ProductId = productId,
                        ProductContentId = 0,
                        Description = "",
                        LangShortName = lang,
                        MetaDescription = "",
                        MetaKeyword = "",
                        MetaTitle = "",
                        NameLine1 = "",
                        NameLine2 = "",
                        Body = "",
                        RewriteUrl = "",
                        ShortDescription = "",
                        CreatedBy = uid,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = uid,
                        LastUpdatedDate = DateTime.Now,
                        ApprovalStatus = 0
                    });
                }
                else
                {
                    listContent.Add(new ProductContentContract
                    {
                        ProductId = content.ProductId,
                        ProductContentId = content.ProductContentId,
                        Description = content.Description,
                        LangShortName = content.LangShortName,
                        MetaDescription = content.MetaDescription,
                        MetaKeyword = content.MetaKeyword,
                        MetaTitle = content.MetaTitle,
                        NameLine1 = content.NameLine1,
                        NameLine2 = content.NameLine2,
                        Body = content.Body,
                        RewriteUrl = content.RewriteUrl,
                        ShortDescription = content.ShortDescription,
                        CreatedBy = content.CreatedBy,
                        CreatedDate = content.CreatedDate,
                        LastUpdatedBy = content.LastUpdatedBy,
                        LastUpdatedDate = content.LastUpdatedDate,
                        ApprovalStatus = content.ApprovalStatus
                    });
                }
            }

            return listContent;
        }

        public CreateUpdateProductContentContract GetProductContentById(int productContentId)
        {
            return lazyProductDataAccess.Value.GetProductContentById(productContentId).Select(r => new CreateUpdateProductContentContract
            {
                ProductId = r.ProductId,
                ProductContentId = r.ProductContentId,
                NameLine1 = r.NameLine1,
                NameLine2 = r.NameLine2,
                ShortDescription = r.ShortDescription,
                Description = r.Description,
                RewriteUrl = r.RewriteUrl,
                LangShortName = r.LangShortName,
                ApprovalStatus = r.ApprovalStatus,
                Body = r.Body,
                MetaTitle = r.MetaTitle,
                MetaKeyword = r.MetaKeyword,
                MetaDescription = r.MetaDescription
            }).SingleOrDefault();
        }

        public CUDReturnMessage CreateUpdateProductContent(CreateUpdateProductContentContract model)
        {
            if (string.IsNullOrEmpty(model.RewriteUrl))
            {
                model.RewriteUrl = model.NameLine1.UnicodeToKoDauAndGach();
            }

            if (model.ProductContentId == 0)
            {
                return lazyProductDataAccess.Value.CreateProductContent(model);
            }
            else
            {
                return lazyProductDataAccess.Value.UpdateProductContent(model);
            }
        }

        public CUDReturnMessage CreateUpdateProductAttributeMapping(int productId, List<ProductAttributeMappingContract> list)
        {
            var listToAdd = list.Where(r => r.IsDelete == false && r.ProductAttributeMappingId == 0).ToList();
            if (listToAdd != null && listToAdd.Any())
            {
                var res = lazyProductDataAccess.Value.CreateProductAttributeMapping(productId, listToAdd);
                if (res.Id != (int)ResponseCode.ProductMngt_SuccessUpdate)
                    return res;
            }

            var listToUpdate = list.Where(r => r.ProductAttributeMappingId > 0).ToList();
            if (listToUpdate != null && listToUpdate.Any())
            {
                return lazyProductDataAccess.Value.UpdateProductAttributeMapping(listToUpdate);
            }

            return new CUDReturnMessage(ResponseCode.ProductMngt_SuccessUpdate);
        }

        #endregion


        #region FE

        public List<Contract.FE.ProductContract> GetListProduct(int msId, string lang, int productTypeId, string productCategoryKey, string searchText)
        {
            var productCategoryId = 0;
            if (!string.IsNullOrEmpty(productCategoryKey))
            {
                productCategoryId = lazyProductCategoryDataAccess.Value.GetProductCategory(productCategoryKey).Id;
            }
            var query = lazyProductDataAccess.Value.GetListProduct(msId, (int)ApprovalStatus.Approved, productCategoryId, searchText);
      
            var detail = query.Select(r => new Contract.FE.ProductContract
            {
                ProductId = r.ProductId,
                Status = r.Status,
                ProductCategoryId = r.ProductCategoryId,
                ProductCategoryName = r.ProductCategory.ProductCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                MsId = r.MsId,
                GalleryId = r.GalleryId,
                ImageUrl = r.ImageUrl,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate,
                ListAttribute = r.ProductAttributeMappings.Where(m=>m.IsDeleted == false 
                                                                    && m.ProductAttribute.IsDeleted == false
                                                                    && m.ProductAttribute.IsOnsite == true
                                                                    && m.LangShortName == lang
                                                                ).Select(m=> new Contract.FE.ProductAttributeContract {
                    AttrId = m.AttrId,
                    ApprovalStatus = m.ProductAttribute.ApprovalStatus,
                    IsOnsite = m.ProductAttribute.IsOnsite,
                    IsThumbnail = m.ProductAttribute.IsThumbnail,
                    IsRequired = m.ProductAttribute.IsRequired,
                    ImageUrl = m.ProductAttribute.ImageUrl,
                    Sort = m.ProductAttribute.Sort,
                    Name = m.ProductAttribute.ProductAttributeContents.Where(t => t.IsDeleted == false && t.ApprovalStatus == (int)ApprovalStatus.Approved && t.LangShortName == lang).Select(t=>t.Name).FirstOrDefault(),
                    Value = m.Value,
                    Key = m.ProductAttribute.Key
                }).ToList(),
                Content = r.ProductContents.Where(c=> c.LangShortName == lang && c.ApprovalStatus == (int)ApprovalStatus.Approved).Select(c=> new Contract.FE.ProductContentContract {
                    ProductContentId = c.ProductContentId,
                    NameLine1 = c.NameLine1,
                    ProductId = c.ProductId,
                    LangShortName = c.LangShortName,
                    ShortDescription = c.ShortDescription,
                    Description = c.Description,
                    Body = c.Body,
                    RewriteUrl = c.RewriteUrl,
                    MetaTitle = c.MetaTitle,
                    MetaDescription = c.MetaDescription,
                    MetaKeyword = c.MetaKeyword,
                    ApprovalStatus = c.ApprovalStatus,
                    NameLine2 = c.NameLine2
                }).FirstOrDefault()
            }).ToList();

            return detail;
        }

        #endregion FE


    }
}
