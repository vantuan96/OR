using System.ComponentModel;

namespace BMS.Contract.Article
{
    public enum ArticleType
    {
        [Description("Bài viết")]
        BaseArticle = 1,

        [Description("Bài viết trang tĩnh")]
        StaticPageArticle = 2,

        [Description("Bài viết trang động")]
        DynamicPageArticle = 3
    }
}
