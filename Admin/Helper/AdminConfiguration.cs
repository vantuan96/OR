using System.Configuration;
using Contract.Shared;

namespace Admin.Helper
{
    public static class AdminConfiguration
    {
        #region Config value

        /// <summary>
        /// Kich thuoc mot trang data
        /// </summary>
        public static int Paging_PageSize
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["Paging.PageSize"]);
            }
        }

        /// <summary>
        /// So nut phan trang duoc hien thi tren giao dien
        /// </summary>
        public static int Paging_ShowPage
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["Paging.ShowPage"]);
            }
        }

        /// <summary>
        /// Lay ten domain de hien thi hinh anh
        /// </summary>
        public static string UploadPhotoURI
        {
            get
            {
                return ConfigurationManager.AppSettings["UploadPhotoURI"];
            }
        }

        /// <summary>
        /// Lay duong dan upload file
        /// </summary>
        public static string UploadFolderPath
        {
            get
            {
                return ConfigurationManager.AppSettings["UploadFolderPath"];
            }
        }

        /// <summary>
        /// Lay ten domain de hien thi hinh anh
        /// </summary>
        public static string DomainPhotoURI
        {
            get
            {
                return ConfigurationManager.AppSettings["DomainPhotoURI"];
            }
        }

        /// <summary>
        /// Duong dan toi file local luu danh muc San pham
        /// </summary>
        public static string Authen_ChangePassURL
        {
            get
            {
                return ConfigurationManager.AppSettings["ChangePassURL"];
            }
        }

        /// <summary>
        /// Session key mac dinh cho user hien tai
        /// </summary>
        public static string CurrentUserSessionKey
        {
            get
            {
                return "adr.User";
            }
        }

        /// <summary>
        /// Cấp danh mục cần xuất JSON Mega menu
        /// </summary>
        public static int MegaMenuCateLevel
        {
            get
            {
                if (ConfigurationManager.AppSettings["ADR.CMS2.MegaMenuCateLevel"] != null)
                {
                    return int.Parse(ConfigurationManager.AppSettings["ADR.CMS2.MegaMenuCateLevel"]);
                }
                else
                {
                    return 3; 
                }
            }
        }

        /// <summary>
        /// AdminDepartmentID
        /// </summary>
        public static int AdminDepartmentID
        {
            get
            {
                if (ConfigurationManager.AppSettings["ADR.CMS2.AdminDepartmentID"] != null)
                {
                    return int.Parse(ConfigurationManager.AppSettings["ADR.CMS2.AdminDepartmentID"]);
                }
                else
                {
                    return 29;
                }
            }
        }

        public static int MaxLengthFullName = 50;

        /// <summary>
        /// Config website dang xay dung
        /// </summary>
        public static bool IsConstructing
        {
            get
            {
                if (ConfigurationManager.AppSettings["ADR.CMS2.IsConstructing"] != null)
                {
                    return ConfigurationManager.AppSettings["ADR.CMS2.IsConstructing"] == "1";
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Url cua server solr
        /// </summary>
        public static string SolrUri
        {
            get
            {
                if (ConfigurationManager.AppSettings["ADR.CMS2.SolrUri"] != null)
                {
                    return ConfigurationManager.AppSettings["ADR.CMS2.SolrUri"];
                }
                else
                {
                    return "http://10.220.32.11:8983/solr";
                }
            }
        }

        /// <summary>
        /// Url cua server solr
        /// </summary>
        public static string QuickSearchCoreName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ADR.CMS2.QuickSearchSolrCoreName"] != null)
                {
                    return ConfigurationManager.AppSettings["ADR.CMS2.QuickSearchSolrCoreName"];
                }
                else
                {
                    return "adr_cms2";
                }
            }
        }

        public static string UnderConstructionCheatKey = "int_test_un";

        public static string NoImagePath = "/Assets/img/noimage.png";
        #endregion Config value

        #region Response message

        /// <summary>
        /// "Trạng thái hiện tại của chương trình không xem được demo."
        /// </summary>
        public static string DealPreviewInvalidStatusMessage
        {
            get
            {
                return "Trạng thái hiện tại của Chương trình không xem được demo.";
            }
        }

        /// <summary>
        /// "Trạng thái hình ảnh hiện tại của chương trình không xem được demo."
        /// </summary>
        public static string DealPreviewInvalidPhotoStatusMessage
        {
            get
            {
                return "Trạng thái hình ảnh hiện tại của Chương trình không xem được demo.";
            }
        }
        
        public const string InvalidString = "{0} chứa ký tự không hợp lệ";

        public const string InvalidFormatString = "{0} không đúng định dạng";
        
        #endregion Response message

        #region Service message

        /// <summary>
        /// Câu thông báo khi không kết nối được đến user service
        /// </summary>
        /// <value>
        /// The can not connect to user service.
        /// </value>
        public static string CanNotConnectToUserService
        {
            get
            {
                return "Không thể kết nối đến User Service";
            }
        }

        /// <summary>
        /// Câu thông báo khi không kết nối được đến deal service
        /// </summary>
        /// <value>
        /// The can not connect to user service.
        /// </value>
        public static string CanNotConnectToDealService
        {
            get
            {
                return "Không thể kết nối đến CMS Service";
            }
        }

        /// <summary>
        /// Không thể kết nối PO Service
        /// </summary>
        public static string CanNotConnectToPOService
        {
            get
            {
                return "Không thể kết nối đến PO Service";
            }
        }

        #endregion Service message

        #region Other

        /// <summary>
        /// Câu thông báo trong phần tạo người dùng khi chưa chọn chức danh
        /// </summary>
        public const string SelectTitleToShowRole = "Chọn chức danh để phân quyền";

        /// <summary>
        /// Câu thông báo khi chức danh chưa có quyền
        /// </summary>
        public const string NoRoleTitle = "Chức danh chưa có quyền nào";

        #endregion Other
    }
}