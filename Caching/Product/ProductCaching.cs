using System;
using System.Collections.Generic;
using BMS.Business.Product;
using BMS.Contract.Product;
using BMS.Contract.ProductCategory;
using BMS.Contract.ProductType;
using BMS.Contract.Shared;

namespace BMS.Caching.Product
{
    public interface IProductCaching
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
        /// danh sach san pham
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="productCategoryKey"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        List<Contract.FE.ProductContract> GetListProduct(int msId, string lang, int productTypeId, string productCategoryKey, string searchText);
        #endregion FE
    }

    public class ProductCaching : BaseCaching, IProductCaching
    {
        private Lazy<ProductBusiness> lazyProductBusiness;

        public ProductCaching(/*string appid, int uid*/)  
        {
            lazyProductBusiness = new Lazy<ProductBusiness>(() =>
            {
                var instance = new ProductBusiness(appid, uid);
                return instance;
            });
        }

        #region Product attribute

        /// <summary>
        /// Tìm kiếm thuộc tính sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ProductAttributeContract> GetProductAttribute(string name, int[] statuses)
        {
            return lazyProductBusiness.Value.GetProductAttribute(name, statuses);
        }

        /// <summary>
        /// Tìm 1 thuộc tính
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msId"></param>
        /// <returns></returns>
        public ProductAttributeContract FindProductAttribute(int id)
        {
            return lazyProductBusiness.Value.FindProductAttribute(id);
        }

        /// <summary>
        /// Tạo thuộc tính sản phẩm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdateProductAttribute(ProductAttributeContract model)
        {
            return lazyProductBusiness.Value.InsertOrUpdateProductAttribute(model);
        }

        /// <summary>
        /// Xem nội dung thuộc tính sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductAttributeContentContract FindProductAttributeContent(int id)
        {
            return lazyProductBusiness.Value.FindProductAttributeContent(id);
        }

        /// <summary>
        /// Tạo và cập nhật nội dung thuộc tính sản phẩm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdateProductAttributeContent(ProductAttributeContentContract model)
        {
            return lazyProductBusiness.Value.InsertOrUpdateProductAttributeContent(model);
        }

        #endregion Product attribute

        #region ProductCategory
        public List<ProductCategoryContract> GetListProductCategory(bool? isOnsite, int approvalStatus)
        {
            return lazyProductBusiness.Value.GetListProductCategory(isOnsite, approvalStatus);
        }

        public ProductCategoryDetailContract GetProductCategoryDetail(int id)
        {
            return lazyProductBusiness.Value.GetProductCategoryDetail(id);
        }

        public ProductCategoryContentContract GetProductCategoryContentById(int contentId)
        {
            return lazyProductBusiness.Value.GetProductCategoryContentById(contentId);
        }
        public CUDReturnMessage CreateUpdateProductCategoryContent(ProductCategoryContentContract pc)
        {
            return lazyProductBusiness.Value.CreateUpdateProductCategoryContent(pc);
        }
        public CUDReturnMessage UpdateProductCategoryStatus(int id, int status)
        {
            return lazyProductBusiness.Value.UpdateProductCategoryStatus(id, status);
        }
        public CUDReturnMessage UpdateProductCategory(ProductCategoryUpdateContract pc)
        {
            return lazyProductBusiness.Value.UpdateProductCategory(pc);
        }

        public CUDReturnMessage UpdateProductCategorySort(List<UpdateProductCategorySortContract> categoryItems) {
            return lazyProductBusiness.Value.UpdateProductCategorySort(categoryItems);

        }


        #endregion ProductCategory

        #region ProductType

        public List<ProductTypeContract> GetListProductType(int MsId, int status)
        {
            return lazyProductBusiness.Value.GetListProductType(MsId, status);
        }

        #endregion

        #region Product

        public PagedList<ProductItemContract> GetListProduct(int msid, int status, int productTypeId, int productCategoryId, string searchText, int page, int pageSize)
        {
            return lazyProductBusiness.Value.GetListProduct(msid, status, productTypeId, productCategoryId, searchText, page, pageSize);
        }

        public CUDReturnMessage CreateProduct(ProductCreateContract model)
        {
            return lazyProductBusiness.Value.CreateProduct(model);
        }

        public CUDReturnMessage DeleteProduct(int productId)
        {
            return lazyProductBusiness.Value.DeleteProduct(productId);
        }

        public CUDReturnMessage UpdateProduct(ProductUpdateContract model)
        {
            return lazyProductBusiness.Value.UpdateProduct(model);
        }

        public ProductDetailContract GetProductDetail(int productId)
        {
            return lazyProductBusiness.Value.GetProductDetail(productId);
        }

        public CreateUpdateProductContentContract GetProductContentById(int productContentId)
        {
            return lazyProductBusiness.Value.GetProductContentById(productContentId);
        }

        public CUDReturnMessage CreateUpdateProductContent(CreateUpdateProductContentContract model)
        {
            return lazyProductBusiness.Value.CreateUpdateProductContent(model);
        }

        public CUDReturnMessage CreateUpdateProductAttributeMapping(int productId, List<ProductAttributeMappingContract> list)
        {
            return lazyProductBusiness.Value.CreateUpdateProductAttributeMapping(productId, list);
        }

        #endregion

        #region FE
        public List<Contract.FE.ProductContract> GetListProduct(int msId, string lang, int productTypeId, string productCategoryKey, string searchText)
        {
            return lazyProductBusiness.Value.GetListProduct(msId, lang, productTypeId, productCategoryKey, searchText);
        }

        #endregion FE
    }
}