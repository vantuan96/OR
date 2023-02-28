using System;
using System.Collections.Generic;
using BMS.Business.Article;
using BMS.Contract.Article;
using BMS.Contract.ArticleCategory;
using BMS.Contract.Shared;
using BMS.Contract.Tag;

namespace BMS.Caching.Article
{
    public interface IArticleMngtCaching
    {
        #region Tag

        IEnumerable<TagContract> GetListTag(string name, int approvalStatus);

        CUDReturnMessage CreateUpdateTag(CreateUpdateTagContract tagData);

        TagContract GetTagById(int tagId);

        #endregion Tag

        #region ArticleCategory

        List<ArticleCategoryContract> GetListArticleCategory(bool? isOnsite, int approvalStatus);

        ArticleCategoryDetailContract GetArticleCategoryDetail(int id);

        ArticleCategoryContentContract GetArticleCategoryContentById(int contentId);

        CUDReturnMessage CreateUpdateArticleCategoryContent(ArticleCategoryContentContract ac);

        CUDReturnMessage UpdateArticleCategoryStatus(int id, int status);

        CUDReturnMessage UpdateArticleCategory(ArticleCategoryUpdateContract ac);

        CUDReturnMessage UpdateCategorySort(List<UpdateCategorySortContract> categoryItems);

        #endregion ArticleCategory

        #region Article

        PagedList<ArticleContract> GetListArticle(string title, int[] types, int[] statuses, int[] tags, int adminTagId,
            int cateId, DateTime? startDate, DateTime? endDate, int msId, int pageSize, int page, string key);

        ArticleContract GetArticle(int id);

        ArticleContentContract GetArticleContent(int id);

        ArticleDetailContract GetArticleDetail(int id);

        CUDReturnMessage CreateUpdateArticleContent(ArticleContentContract ac);

        CUDReturnMessage UpdateArticle(ArticleUpdateContract ac);

        CUDReturnMessage UpdateArticlePosition(int adminTagId, List<ArticlePositionUpdateContract> updateData);

        #endregion Article

        #region Article FE

        /// <summary>
        /// Lấy article cho FE
        /// </summary>
        /// <param name="msId"></param>
        /// <param name="articleId"></param>
        /// <param name="lang"></param>
        /// <param name="articleKey"></param>
        /// <returns></returns>
        Contract.FE.ArticleContract GetArticle(int msId, int articleId, string lang, string articleKey);

        /// <summary>
        /// lấy danh sách bài viết
        /// </summary>
        /// <returns></returns>
        List<Contract.FE.ArticleItemContract> GetListArticleItem(string articleCategoryKey, string adminTagName, string lang, int msId);

        /// <summary>
        /// lấy danh sách article key để tạo routing
        /// </summary>
        /// <returns></returns>
        List<Contract.FE.ArticleKeyContract> GetListArticleKey(int articleType);

        #endregion Article FE
    }

    public class ArticleMngtCaching : BaseCaching, IArticleMngtCaching
    {
        private Lazy<ArticleMngtBusiness> lazyLogBusiness;

        public ArticleMngtCaching(/*string appid, int uid*/)  
        {
            lazyLogBusiness = new Lazy<ArticleMngtBusiness>(() =>
            {
                var instance = new ArticleMngtBusiness(appid, uid);
                return instance;
            });
        }

        #region Tag

        public IEnumerable<TagContract> GetListTag(string name, int approvalStatus)
        {
            return lazyLogBusiness.Value.GetListTag(name, approvalStatus);
        }

        public CUDReturnMessage CreateUpdateTag(CreateUpdateTagContract tagData)
        {
            return lazyLogBusiness.Value.CreateUpdateTag(tagData);
        }

        public TagContract GetTagById(int tagId)
        {
            return lazyLogBusiness.Value.GetTagById(tagId);
        }

        #endregion Tag

        #region ArticleCategory

        public List<ArticleCategoryContract> GetListArticleCategory(bool? isOnsite, int approvalStatus)
        {
            return lazyLogBusiness.Value.GetListArticleCategory(isOnsite, approvalStatus);
        }

        public ArticleCategoryDetailContract GetArticleCategoryDetail(int id)
        {
            return lazyLogBusiness.Value.GetArticleCategoryDetail(id);
        }

        public ArticleCategoryContentContract GetArticleCategoryContentById(int contentId)
        {
            return lazyLogBusiness.Value.GetArticleCategoryContentById(contentId);
        }

        public CUDReturnMessage CreateUpdateArticleCategoryContent(ArticleCategoryContentContract ac)
        {
            return lazyLogBusiness.Value.CreateUpdateArticleCategoryContent(ac);
        }

        public CUDReturnMessage UpdateArticleCategoryStatus(int id, int status)
        {
            return lazyLogBusiness.Value.UpdateArticleCategoryStatus(id, status);
        }

        public CUDReturnMessage UpdateArticleCategory(ArticleCategoryUpdateContract ac)
        {
            return lazyLogBusiness.Value.UpdateArticleCategory(ac);
        }

        public CUDReturnMessage UpdateCategorySort(List<UpdateCategorySortContract> categoryItems)
        {
            return lazyLogBusiness.Value.UpdateCategorySort(categoryItems);
        }

        #endregion ArticleCategory

        #region Article

        public PagedList<ArticleContract> GetListArticle(string title, int[] types, int[] statuses, int[] tags,
            int adminTagId, int cateId, DateTime? startDate, DateTime? endDate, int msId, int pageSize, int page, string key)
        {
            return lazyLogBusiness.Value.GetListArticle(title, types, statuses, tags, adminTagId, cateId, startDate,
                endDate, null, msId, pageSize, page, key);
        }

        public ArticleContract GetArticle(int id)
        {
            return lazyLogBusiness.Value.GetArticle(id);
        }

        public ArticleContentContract GetArticleContent(int id)
        {
            return lazyLogBusiness.Value.GetArticleContent(id);
        }

        public ArticleDetailContract GetArticleDetail(int id)
        {
            return lazyLogBusiness.Value.GetArticleDetail(id);
        }

        public CUDReturnMessage CreateUpdateArticleContent(ArticleContentContract ac)
        {
            return lazyLogBusiness.Value.CreateUpdateArticleContent(ac);
        }

        public CUDReturnMessage UpdateArticle(ArticleUpdateContract ac)
        {
            return lazyLogBusiness.Value.UpdateArticle(ac);
        }

        public CUDReturnMessage UpdateArticlePosition(int adminTagId, List<ArticlePositionUpdateContract> updateData)
        {
            return lazyLogBusiness.Value.UpdateArticlePosition(adminTagId, updateData);
        }

        #endregion Article

        #region Article FE

        /// <summary>
        ///  Lấy article cho FE
        /// </summary>
        /// <param name="msId"></param>
        /// <param name="articleId"></param>
        /// <param name="lang"></param>
        /// <param name="articleKey"></param>
        /// <returns></returns>
        public Contract.FE.ArticleContract GetArticle(int msId, int articleId, string lang, string articleKey)
        {
            return lazyLogBusiness.Value.GetArticle(msId, articleId, lang, articleKey);
        }

        /// <summary>
        /// lấy danh sách bài viết
        /// </summary>
        /// <returns></returns>
        public List<Contract.FE.ArticleItemContract> GetListArticleItem(string articleCategoryKey, string adminTagName, string lang, int msId)
        {
            return lazyLogBusiness.Value.GetListArticleItem(articleCategoryKey, adminTagName, lang, msId);
        }

        /// <summary>
        /// lấy danh sách article key để tạo routing
        /// </summary>
        /// <returns></returns>
        public List<Contract.FE.ArticleKeyContract> GetListArticleKey(int articleType)
        {
            return lazyLogBusiness.Value.GetListArticleKey(articleType);
        }

        #endregion Article FE
    }
}