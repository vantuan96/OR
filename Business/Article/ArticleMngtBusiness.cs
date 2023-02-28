using System;
using System.Collections.Generic;
using System.Linq;
using VG.Common;
using BMS.Contract.AdminTag;
using BMS.Contract.Article;
using BMS.Contract.ArticleCategory;
using BMS.Contract.Shared;
using BMS.Contract.Tag;
using BMS.DataAccess;
using BMS.DataAccess.DAO;

namespace BMS.Business.Article
{
    public interface IArticleMngtBusiness
    {
        #region Tag

        /// <summary>
        /// danh sach tag
        /// </summary>
        /// <param name="name"></param>
        /// <param name="approvalStatus"></param>
        /// <returns></returns>
        IEnumerable<TagContract> GetListTag(string name, int approvalStatus);

        /// <summary>
        /// create and update tag
        /// </summary>
        /// <param name="tagData"></param>
        /// <returns></returns>
        CUDReturnMessage CreateUpdateTag(CreateUpdateTagContract tagData);

        /// <summary>
        /// get tag by id
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        TagContract GetTagById(int tagId);

        #endregion Tag

        #region ArticleCategory

        /// <summary>
        /// Lấy danh sách danh mục bài viết
        /// </summary>
        /// <param name="isOnsite"></param>
        /// <param name="approvalStatus"></param>
        /// <returns></returns>
        List<ArticleCategoryContract> GetListArticleCategory(bool? isOnsite, int approvalStatus);

        /// <summary>
        /// Chi tiết danh mục bài viết
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleCategoryDetailContract GetArticleCategoryDetail(int id);

        /// <summary>
        /// Nội dung danh mục bài viết theo Id
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        ArticleCategoryContentContract GetArticleCategoryContentById(int contentId);

        /// <summary>
        /// Thêm và chỉnh sửa nội dung danh mục bài viết
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        CUDReturnMessage CreateUpdateArticleCategoryContent(ArticleCategoryContentContract ac);

        /// <summary>
        /// update trạng thái danh mục bài viết
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        CUDReturnMessage UpdateArticleCategoryStatus(int id, int status);

        /// <summary>
        /// Update thông tin chính danh mục bài viết
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        CUDReturnMessage UpdateArticleCategory(ArticleCategoryUpdateContract ac);

        CUDReturnMessage UpdateCategorySort(List<UpdateCategorySortContract> categoryItems);

        #endregion ArticleCategory

        #region Article

        /// <summary>
        ///  Danh sách bài viết
        /// </summary>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="statuses"></param>
        /// <param name="tags"></param>
        /// <param name="adminTagId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="msId"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        PagedList<ArticleContract> GetListArticle(string title, int[] types, int[] statuses, int[] tags, int adminTagId,
            int cateId, DateTime? startDate, DateTime? endDate, DateTime? viewDate, int msId, int pageSize, int page, string key);

        /// <summary>
        /// Thông tin chính của bài viết
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleContract GetArticle(int id);

        /// <summary>
        /// Nội dung bài viết
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleContentContract GetArticleContent(int id);

        /// <summary>
        /// Chi tiết của bài viết
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleDetailContract GetArticleDetail(int id);

        /// <summary>
        /// Thêm và chỉnh sửa nội dung bài viết
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        CUDReturnMessage CreateUpdateArticleContent(ArticleContentContract ac);

        /// <summary>
        /// Update thông tin chính của bài viết
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
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

    public class ArticleMngtBusiness : BaseBusiness, IArticleMngtBusiness
    {
        private Lazy<ITagDataAccess> lazyAccess;
        private Lazy<IArticleCategoryDataAccess> lazyArticleCategoryDataAccess;
        private Lazy<IArticleDataAccess> lazyArticleDataAccess;
        private Lazy<IAdminTagDataAccess> lazyAdminTagDataAccess;

        public ArticleMngtBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAccess = new Lazy<ITagDataAccess>(() => new TagDataAccess(appid, uid));
            lazyArticleCategoryDataAccess = new Lazy<IArticleCategoryDataAccess>(() => new ArticleCategoryDataAccess(appid, uid));
            lazyArticleDataAccess = new Lazy<IArticleDataAccess>(() => new ArticleDataAccess(appid, uid));
            lazyAdminTagDataAccess = new Lazy<IAdminTagDataAccess>(() => new AdminTagDataAccess(appid, uid));
        }

        #region Tag

        public IEnumerable<TagContract> GetListTag(string name, int approvalStatus)
        {
            var data = lazyAccess.Value.GetListTag(name, approvalStatus);

            if (data != null)
            {
                return data.Select(x => new TagContract()
                {
                    TagId = x.TagId,
                    Name = x.Name,
                    LangShortName = x.LangShortName,
                    //Status = x.Status,
                    RewriteUrl = x.RewriteUrl,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ApprovalStatus = x.ApprovalStatus,
                    LastUpdatedBy = x.LastUpdatedBy,
                    //MsId = x.MsId,
                    LastUpdatedDate = x.LastUpdatedDate
                });
            }

            return null;
        }

        public CUDReturnMessage CreateUpdateTag(CreateUpdateTagContract tagData)
        {
            if (tagData.TagId > 0)
            {
                return lazyAccess.Value.UpdateTag(tagData);
            }
            else
            {
                return lazyAccess.Value.InsertTag(tagData);
            }
        }

        public TagContract GetTagById(int tagId)
        {
            var tag = lazyAccess.Value.GetTagById(tagId);

            if (tag != null)
            {
                return new TagContract()
                {
                    TagId = tag.TagId,
                    Name = tag.Name,
                    LangShortName = tag.LangShortName,
                    //Status = tag.Status,
                    RewriteUrl = tag.RewriteUrl,
                    CreatedBy = tag.CreatedBy,
                    CreatedDate = tag.CreatedDate,
                    ApprovalStatus = tag.ApprovalStatus,
                    LastUpdatedBy = tag.LastUpdatedBy,
                    //MsId = tag.MsId,
                    LastUpdatedDate = tag.LastUpdatedDate
                };
            }
            else
            {
                return null;
            }
        }

        #endregion Tag

        #region ArticleCategory

        public List<ArticleCategoryContract> GetListArticleCategory(bool? isOnsite, int approvalStatus)
        {
            var query = lazyArticleCategoryDataAccess.Value.GetListArticleCategory(null, isOnsite, approvalStatus);

            if (query != null)
            {
                var listParentId = query.Where(r => r.ParentId > 0).Distinct().Select(r => r.ParentId).ToList();

                var listParent = query.Where(r => listParentId.Contains(r.Id)).Select(r => new { Id = r.Id, Name = r.ArticleCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault() }).ToDictionary(r => r.Id);

                var listCate = query.Select(r => new ArticleCategoryContract
                {
                    Id = r.Id,
                    //MsId = r.MsId,
                    Name = r.ArticleCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
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

                listCate.Where(r => r.ParentId != 0).ToList().ForEach(
                    r => r.ParentName = listParent.ContainsKey((int)r.ParentId) ? listParent[(int)r.ParentId].Name : ""
                    );

                return listCate;
            }
            return null;
        }

        public ArticleCategoryDetailContract GetArticleCategoryDetail(int id)
        {
            var ac = lazyArticleCategoryDataAccess.Value.GetArticleCategoryById(id);

            if (ac != null)
            {
                var detail = new ArticleCategoryDetailContract
                {
                    Id = ac.Id,
                    //MsId = ac.MsId,
                    ApprovalStatus = ac.ApprovalStatus,
                    IsOnsite = ac.IsOnsite,
                    ParentId = ac.ParentId,
                    CreatedBy = ac.CreatedBy,
                    CreatedDate = ac.CreatedDate,
                    LastUpdatedBy = ac.LastUpdatedBy,
                    LastUpdatedDate = ac.LastUpdatedDate,
                    ListArticleCategoryContent = new List<ArticleCategoryContentContract>()
                };

                foreach (var lang in listApprovedLanguage)
                {
                    var content = ac.ArticleCategoryContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang));
                    if (content == null)
                    {
                        detail.ListArticleCategoryContent.Add(new ArticleCategoryContentContract
                        {
                            LangShortName = lang,
                            ArticleCategoryId = ac.Id,
                            ContentId = 0,
                            //MsId = ac.MsId
                        });
                    }
                    else
                    {
                        detail.ListArticleCategoryContent.Add(new ArticleCategoryContentContract
                        {
                            //MsId = ac.MsId,
                            ContentId = content.ContentId,
                            ArticleCategoryId = content.ArticleCategoryId,
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

        public ArticleCategoryContentContract GetArticleCategoryContentById(int contentId)
        {
            var acContent = lazyArticleCategoryDataAccess.Value.GetArticleCategoryContentById(contentId);

            if (acContent != null)
            {
                var content = new ArticleCategoryContentContract
                {
                    ContentId = acContent.ContentId,
                    ArticleCategoryId = acContent.ArticleCategoryId,
                    Name = acContent.Name,
                    Rewrite = acContent.Rewrite,
                    CategoryUrl = acContent.CategoryUrl,
                    ShortDescription = acContent.ShortDescription,
                    Description = acContent.Description,
                    LangShortName = acContent.LangShortName,
                    ApprovalStatus = acContent.ApprovalStatus,
                    CreatedBy = acContent.CreatedBy,
                    CreatedDate = acContent.CreatedDate,
                    LastUpdatedBy = acContent.LastUpdatedBy,
                    LastUpdatedDate = acContent.LastUpdatedDate
                };

                return content;
            }
            return null;
        }

        public CUDReturnMessage CreateUpdateArticleCategoryContent(ArticleCategoryContentContract ac)
        {
            if (ac.ContentId > 0)
            {
                return lazyArticleCategoryDataAccess.Value.UpdateArticleCategoryContent(ac);
            }

            if (ac.ArticleCategoryId > 0)
            {
                return lazyArticleCategoryDataAccess.Value.CreateArticleCategoryContent(ac);
            }

            if (string.IsNullOrEmpty(ac.LangShortName))
            {
                ac.LangShortName = defaultLanguageCode;
            }

            return lazyArticleCategoryDataAccess.Value.CreateArticleCategory(ac);
        }

        public CUDReturnMessage UpdateArticleCategoryStatus(int id, int status)
        {
            return lazyArticleCategoryDataAccess.Value.UpdateArticleCategoryStatus(id, status);
        }

        public CUDReturnMessage UpdateArticleCategory(ArticleCategoryUpdateContract ac)
        {
            return lazyArticleCategoryDataAccess.Value.UpdateArticleCategory(ac);
        }

        public CUDReturnMessage UpdateCategorySort(List<UpdateCategorySortContract> categoryItems)
        {
            return lazyArticleCategoryDataAccess.Value.UpdateCategorySort(categoryItems);
        }

        #endregion ArticleCategory

        #region Article

        public PagedList<ArticleContract> GetListArticle(string title, int[] types, int[] statuses, int[] tags,
            int adminTagId,int cateId, DateTime? startDate, DateTime? endDate, DateTime? viewDate, int msId,
            int pageSize, int page, string key)
        {
            var query = lazyArticleDataAccess.Value.GetListArticle(title, types, statuses, tags, cateId, adminTagId,
                startDate, endDate, viewDate, msId, key);
            var result = new PagedList<ArticleContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new ArticleContract
                {
                    Id = r.Id,
                    Title = r.ArticleContents.FirstOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Title,
                    Title2 = r.ArticleContents.FirstOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Title2,
                    ImageUrl = r.ArticleContents.FirstOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).ImageUrl,
                    ImageMobileUrl = r.ArticleContents.FirstOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).ImageMobileUrl,
                    Tags = r.ArticleTagMappings.Where(bc => bc.IsDeleted == false).Select(bc => new TagContract() { TagId = bc.TagId, Name = bc.Tag.Name }).ToList(),
                    AdminTags = r.AdminTagMappings.Where(at => at.IsDeleted == false).Select(at => new AdminTagContract() { Id = at.AdminTagId, Name = at.AdminTag.Name, Sort = at.Sort }).ToList(),
                    IsHot = r.IsHot,
                    IsFocus = r.IsFocus,
                    Status = r.Status,
                    Type = r.Type,
                    IsVipPromotion = r.IsVipPromotion,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    CityId = r.CityId,
                    MsId = r.MsId,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    LastUpdatedBy = r.LastUpdatedBy,
                    LastUpdatedDate = r.LastUpdatedDate,
                    ArticleCategoryId = r.ArticleCategory != null ? r.ArticleCategory.Id : 0,
                    //ArticleCategoryName = r.ArticleCategory != null ? r.ArticleCategory.ArticleCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault() : "",
                    ArticleCategoryName = r.ArticleCategory != null ? r.ArticleCategory.ArticleCategoryContents.FirstOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Name : "",
                    ArticleCategoryOthersId = r.ArticleCategoryOthersId,
                    Key = r.Key
                }).ToList();
            }

            return result;
        }

        public ArticleContract GetArticle(int id)
        {
            var article = lazyArticleDataAccess.Value.GetArticle(0, 0, id, "");

            if (article != null)
            {
                var content = new ArticleContract
                {
                    Id = article.Id,
                    Title = article.ArticleContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Title,
                    Title2 = article.ArticleContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Title2,
                    ImageUrl = article.ArticleContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).ImageUrl,
                    ImageMobileUrl = article.ArticleContents.SingleOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).ImageMobileUrl,
                    Tags = article.ArticleTagMappings.Where(r => r.IsDeleted == false).Select(r => new TagContract() { TagId = r.TagId, Name = r.Tag.Name }).ToList(),
                    AdminTags = article.AdminTagMappings.Where(r => r.IsDeleted == false).Select(r => new AdminTagContract() { Id = r.AdminTagId, Name = r.AdminTag.Name, Sort = r.Sort }).ToList(),
                    IsHot = article.IsHot,
                    IsFocus = article.IsFocus,
                    Status = article.Status,
                    Type = article.Type,
                    IsVipPromotion = article.IsVipPromotion,
                    StartDate = article.StartDate,
                    EndDate = article.EndDate,
                    CityId = article.CityId,
                    MsId = article.MsId,
                    CreatedBy = article.CreatedBy,
                    CreatedDate = article.CreatedDate,
                    LastUpdatedBy = article.LastUpdatedBy,
                    LastUpdatedDate = article.LastUpdatedDate,
                    ArticleCategoryId = article.ArticleCategory.Id,
                    ArticleCategoryName = article.ArticleCategory.ArticleCategoryContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                    ArticleCategoryOthersId = article.ArticleCategoryOthersId
                };

                return content;
            }
            return null;
        }

        public ArticleContentContract GetArticleContent(int id)
        {
            var ac = lazyArticleDataAccess.Value.GetArticleContent(id);

            if (ac != null)
            {
                return new ArticleContentContract()
                {
                    Id = ac.Id,
                    ArticleId = ac.ArticleId,
                    Title = ac.Title,
                    Title2 = ac.Title2,
                    Rewrite = ac.Rewrite,
                    ShortDescription = ac.ShortDescription,
                    ArticleUrl = ac.ArticleUrl,
                    Body = ac.Body,
                    ImageUrl = ac.ImageUrl,
                    TargetLinkImage = ac.TargetLinkImage,
                    ImageMobileUrl = ac.ImageMobileUrl,
                    TargetLinkImageMobile = ac.TargetLinkImageMobile,
                    LangShortName = ac.LangShortName,
                    MetaTitle = ac.MetaTitle,
                    MetaDescription = ac.MetaDescription,
                    MetaKeyword = ac.MetaKeyword,
                    ApprovalStatus = ac.ApprovalStatus,
                    CreatedBy = ac.CreatedBy,
                    CreatedDate = ac.CreatedDate,
                    LastUpdatedBy = ac.LastUpdatedBy,
                    LastUpdatedDate = ac.LastUpdatedDate,
                    IsDeleted = ac.IsDeleted
                };
            }
            return null;
        }

        public ArticleDetailContract GetArticleDetail(int id)
        {
            var ac = lazyArticleDataAccess.Value.GetArticle(0, 0, id, "");

            if (ac != null)
            {
                var detail = new ArticleDetailContract
                {
                    Id = ac.Id,
                    IsHot = ac.IsHot,
                    IsFocus = ac.IsFocus,
                    Status = ac.Status,
                    Type = ac.Type,
                    Key = ac.Key,
                    Tags = ac.ArticleTagMappings.Where(r => r.IsDeleted == false).Select(r => new TagContract() { TagId = r.TagId, Name = r.Tag.Name }).ToList(),
                    //Tags = ac.Tags.Select(r=> new TagContract() { TagId = r.TagId, Name = r.Name }).ToList(),
                    AdminTags = ac.AdminTagMappings.Where(r => r.IsDeleted == false).Select(r => new AdminTagContract() { Id = r.AdminTagId, Name = r.AdminTag.Name }).ToList(),
                    IsVipPromotion = ac.IsVipPromotion,
                    StartDate = ac.StartDate,
                    EndDate = ac.EndDate,
                    CityId = ac.CityId,
                    MsId = ac.MsId,
                    CreatedBy = ac.CreatedBy,
                    CreatedDate = ac.CreatedDate,
                    LastUpdatedBy = ac.LastUpdatedBy,
                    LastUpdatedDate = ac.LastUpdatedDate,
                    ArticleCategoryId = ac.ArticleCategoryId != null ? (int)ac.ArticleCategoryId : 0,
                    ArticleCategoryOthersId = ac.ArticleCategoryOthersId,
                    ListArticleContents = new List<ArticleContentContract>()
                };

                foreach (var lang in listApprovedLanguage)
                {
                    var content = ac.ArticleContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang));
                    if (content == null)
                    {
                        detail.ListArticleContents.Add(new ArticleContentContract
                        {
                            LangShortName = lang,
                            Id = 0,
                            ArticleId = ac.Id,
                        });
                    }
                    else
                    {
                        detail.ListArticleContents.Add(new ArticleContentContract
                        {
                            Id = content.Id,
                            ArticleId = content.ArticleId,
                            Title = content.Title,
                            Title2 = content.Title2,
                            Rewrite = content.Rewrite,
                            ShortDescription = content.ShortDescription,
                            ArticleUrl = content.ArticleUrl,
                            Body = content.Body,
                            ImageUrl = content.ImageUrl,
                            TargetLinkImage = content.TargetLinkImage,
                            ImageMobileUrl = content.ImageMobileUrl,
                            TargetLinkImageMobile = content.TargetLinkImageMobile,
                            LangShortName = content.LangShortName,
                            MetaTitle = content.MetaTitle,
                            MetaDescription = content.MetaDescription,
                            MetaKeyword = content.MetaKeyword,
                            ApprovalStatus = content.ApprovalStatus,
                            CreatedBy = content.CreatedBy,
                            CreatedDate = content.CreatedDate,
                            LastUpdatedBy = content.LastUpdatedBy,
                            LastUpdatedDate = content.LastUpdatedDate,
                            StartDate = ac.StartDate,
                            EndDate = ac.EndDate,
                            Type = ac.Type,
                            MsId = ac.MsId,
                            ArticleCategoryId = ac.ArticleCategoryId != null ? (int)ac.ArticleCategoryId : 0,
                        });
                    }
                }

                return detail;
            }

            return null;
        }

        public CUDReturnMessage CreateUpdateArticleContent(ArticleContentContract ac)
        {
            ac.TitleWithoutAccent = ac.Title.UnicodeToKoDau();
            ac.Title2WithoutAccent = ac.Title2.IsNullOrEmpty() ? string.Empty : ac.Title2.UnicodeToKoDau();
            if (string.IsNullOrEmpty(ac.Rewrite))
            {
                ac.Rewrite = ac.Title.UnicodeToKoDauAndGach();
            }

            // update nội dung bài viết
            if (ac.Id > 0)
            {
                return lazyArticleDataAccess.Value.UpdateArticleContent(ac);
            }

            // tạo nội dung bài viết
            if (ac.ArticleId > 0)
            {
                return lazyArticleDataAccess.Value.CreateArticleContent(ac);
            }

            if (string.IsNullOrEmpty(ac.LangShortName))
            {
                ac.LangShortName = defaultLanguageCode;
            }

            return lazyArticleDataAccess.Value.CreateArticle(ac);
        }

        public CUDReturnMessage UpdateArticle(ArticleUpdateContract ac)
        {
            return lazyArticleDataAccess.Value.UpdateArticle(ac);
        }

        public CUDReturnMessage UpdateArticlePosition(int adminTagId, List<ArticlePositionUpdateContract> updateData)
        {
            return lazyArticleDataAccess.Value.UpdateArticlePosition(adminTagId, updateData);
        }

        #endregion Article

        #region Article FE

        /// <summary>
        /// Lấy article đã duyệt cho FE
        /// </summary>
        /// <param name="msId"></param>
        /// <param name="articleId"></param>
        /// <param name="lang"></param>
        /// <param name="articleKey"></param>
        /// <returns></returns>
        public Contract.FE.ArticleContract GetArticle(int msId, int articleId, string lang, string articleKey)
        {
            var article = lazyArticleDataAccess.Value.GetArticle(msId: msId, status: (int)ObjectStatus.Onsite, id: articleId, articleKey: articleKey);

            return new Contract.FE.ArticleContract()
            {
                Id = article.Id,
                IsHot = article.IsHot,
                IsFocus = article.IsFocus,
                Status = article.Status,
                Type = article.Type,
                IsVipPromotion = article.IsVipPromotion,
                StartDate = article.StartDate,
                EndDate = article.EndDate,
                MsId = article.MsId,
                CreatedBy = article.CreatedBy,
                CreatedDate = article.CreatedDate,
                LastUpdatedBy = article.LastUpdatedBy,
                LastUpdatedDate = article.LastUpdatedDate,
                ArticleCategoryId = article.ArticleCategory != null ? article.ArticleCategory.Id : 0,
                ArticleCategoryName = article.ArticleCategory != null ? article.ArticleCategory.ArticleCategoryContents.FirstOrDefault(bc => bc.LangShortName.Trim().Equals(defaultLanguageCode)).Name : "",
                Tags = article.ArticleTagMappings.Where(bc => bc.IsDeleted == false).Select(bc => new TagContract() { TagId = bc.TagId, Name = bc.Tag.Name }).ToList(),
                AdminTags = article.AdminTagMappings.Where(at => at.IsDeleted == false).Select(at => new AdminTagContract() { Id = at.AdminTagId, Name = at.AdminTag.Name, Sort = at.Sort }).ToList(),
                ArticleContent = article.ArticleContents.Where(c => c.IsDeleted == false && c.LangShortName == lang && c.ApprovalStatus == (int)ApprovalStatus.Approved).Select(c => new Contract.FE.ArticleContentContract
                {
                    Id = c.Id,
                    Title = c.Title,
                    Title2 = c.Title2,
                    Rewrite = c.Rewrite,
                    ShortDescription = c.ShortDescription,
                    ArticleUrl = c.ArticleUrl,
                    Body = c.Body,
                    ImageUrl = c.ImageUrl,
                    TargetLinkImage = c.TargetLinkImage,
                    ImageMobileUrl = c.ImageMobileUrl,
                    TargetLinkImageMobile = c.TargetLinkImageMobile,
                    LangShortName = c.LangShortName,
                    MetaTitle = c.MetaTitle,
                    MetaDescription = c.MetaDescription,
                    MetaKeyword = c.MetaKeyword,
                    ApprovalStatus = c.ApprovalStatus,
                    CreatedBy = c.CreatedBy,
                    CreatedDate = c.CreatedDate,
                    LastUpdatedBy = c.LastUpdatedBy,
                    LastUpdatedDate = c.LastUpdatedDate,
                    TitleWithoutAccent = c.TitleWithoutAccent,
                    Title2WithoutAccent = c.Title2WithoutAccent
                }).FirstOrDefault()
            };
        }

        /// <summary>
        /// lấy danh sách bài viết
        /// </summary>
        /// <returns></returns>
        public List<Contract.FE.ArticleItemContract> GetListArticleItem(string articleCategoryKey, string adminTagName, string lang, int msId)
        {
            int adminTagId = 0, cateId = 0;

            if (string.IsNullOrEmpty(adminTagName) == false)
            {
                var queryTag = lazyAdminTagDataAccess.Value.Get(null, adminTagName);
                adminTagId = queryTag.Select(r => r.Id).FirstOrDefault();
                adminTagId = adminTagId == 0 ? -1 : adminTagId;
            }

            if (string.IsNullOrEmpty(articleCategoryKey) == false)
            {
                var queryCate = lazyArticleCategoryDataAccess.Value.GetListArticleCategory(articleCategoryKey, true, (int)ApprovalStatus.Approved);
                cateId = queryCate.Select(r => r.Id).FirstOrDefault();
                cateId = cateId == 0 ? -1 : cateId;
            }

            var query = lazyArticleDataAccess.Value.GetListArticle(null, null, new int[] { (int)ObjectStatus.Onsite },
                null, cateId, adminTagId, null, null, DateTime.Now, msId, "");

            return query.Select(r => new Contract.FE.ArticleItemContract
            {
                Id = r.Id,
                IsHot = r.IsHot,
                IsFocus = r.IsFocus,
                CreatedDate = r.CreatedDate,
                LastUpdatedDate = r.LastUpdatedDate,
                Content = r.ArticleContents
                    .Where(c => c.IsDeleted == false && c.ApprovalStatus == (int)ApprovalStatus.Approved && c.LangShortName == lang)
                    .Select(c => new Contract.FE.ArticleContentItemContract
                    {
                        Title = c.Title,
                        Title2 = c.Title2,
                        Rewrite = c.Rewrite,
                        ShortDescription = c.ShortDescription,
                        ArticleUrl = c.ArticleUrl,
                        ImageUrl = c.ImageUrl,
                        ImageMobileUrl = c.ImageMobileUrl,
                        MetaTitle = c.MetaTitle,
                        MetaDescription = c.MetaDescription,
                        MetaKeyword = c.MetaKeyword,
                        CreatedDate = c.CreatedDate,
                        LastUpdatedDate = c.LastUpdatedDate
                    }).FirstOrDefault()
            }).ToList();
        }

        public List<Contract.FE.ArticleKeyContract> GetListArticleKey(int articleType)
        {
            var query = lazyArticleDataAccess.Value.GetListArticle(null, new int[] { articleType}, new int[] { (int)ObjectStatus.Onsite },
                null, 0, 0, null, null, DateTime.Now, 0, "");

            return query.Select(r => new Contract.FE.ArticleKeyContract
            {
                Id = r.Id,
                Key = r.Key
            }).ToList();
        }

        #endregion Article FE
    }
}