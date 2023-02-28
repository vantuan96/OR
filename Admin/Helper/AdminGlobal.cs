using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using VG.Common;
using Admin.Resource;

namespace Admin.Helper
{
    /// <summary>
    /// Base class for holding application wide used values;
    /// CONST for unchangeable values;
    /// STATIC for application changeable values;
    /// </summary>
    public abstract class AdminGlobal
    {
        /// <summary>
        /// Contains some default values for some data type such as DateTime, integer...
        /// </summary>
        public class DefaultValue
        {
            /// <summary>
            /// MinDateTime = new DateTime(1900,1,1)
            /// </summary>
            public static DateTime DateTimeMin = new DateTime(1900, 1, 1);

            /// <summary>
            /// DateTime.Now
            /// </summary>
            public static DateTime DefaultTodate
            {
                get { return DateTime.Now.AddTimeToTheEndOfDay(); }
            }

            /// <summary>
            /// DateTime.Now.AddDays(-29)
            /// </summary>
            public static DateTime DefaultFromdate
            {
                get
                {
                    return DateTime.Now.AddDays(-29).AddTimeToTheStartOfDay();
                }
            }

            /// <summary>
            /// ID thành phố mặc định là Hà Nội
            /// </summary>
            public static int CityIdDefault = 4;

            /// <summary>
            /// Used for text area validation max length
            /// </summary>
            public static int DescriptionMaxLength = 500;

            /// <summary>
            /// Used for text area validation min length
            /// </summary>
            public static int DescriptionMinLength = 1;

            /// <summary>
            /// Id of photo & content department in database
            /// </summary>
            public const int PhotoAndContentDepartmentId = 64;
            /// <summary>
            /// Value from web config
            /// </summary>
            public static readonly int PageSizeDefault = AdminConfiguration.Paging_PageSize;

            /// <summary>
            /// Nodes of same merchant photos search
            /// </summary>
            public const int SameMerchantPhotoPageNodes = 3;

            /// <summary>
            /// Value from web config
            /// </summary>
            public static readonly int CountShowPageDefault = AdminConfiguration.Paging_ShowPage;

            /// <summary>
            /// C sharp datetime default value is "01/01/0001"
            /// </summary>
            public const int CSharpDateTimeDefaultYear = 1;

            /// <summary>
            /// Value = 50. This is category id of travel category in category table
            /// </summary>
            public const int TravelCategoryId = 50;

            /// <summary>
            /// Param for manual group link
            /// </summary>
            public const string ManualGroupParam = "g";

            /// <summary>
            /// "When set PageIndex=1 and PagaeSize=-1,Respective store procedure will handle this case to get all records instated of paging."
            /// </summary>
            public const int DefaultPageIndexToGetAllRecords = 1;

            /// <summary>
            /// "When set PageIndex=1 and PagaeSize=-1,Respective store procedure will handle this case to get all records instated of paging."
            /// </summary>
            public const int DefaultPageSizeToGetAllRecords = -1;

            /// <summary>
            /// -1 is Default status let store proc eliminate deals which has photo and content in QcCompleted status
            /// </summary>
            public const int ExceptQcCompletedStatus = -1;
            /// <summary>
            /// "0"
            /// </summary>
            public const string DefaultStatusId = "0";

            /// <summary>
            /// Used for valid user password min length
            /// </summary>
            public const int RequiredUserPasswordMinLength = 12;

            /// <summary>
            /// Used for valid user password max length
            /// </summary>
            public const int RequiredUserPasswordMaxLength = 50;

            /// <summary>
            /// Password must contain at least one letter, at least one number, at least one special letter and be longer than 12 charaters.
            /// </summary>
            public const string PasswordRegExpression = @"^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)(?=.*[\W_]+.*)[0-9a-zA-Z\W_]{12,}$";

            /// <summary>
            /// Default banner size max is 10MB
            /// </summary>
            public const int MaxBannerPhotoSize = 10485760;

            /// <summary>
            /// Default manual banner max size is 5MB
            /// </summary>
            public const int MaxManualBannerSize = 5242880;

            /// <summary>
            /// Default manual banner max width is 1240
            /// </summary>
            public const int MaxManualBannerWidth = 1240;

            /// <summary>
            /// Default manual banner max heigh is 360
            /// </summary>
            public const int MaxManualBannerHeigh = 360;

            /// <summary>
            /// Default manual banner max mobile width is 1440
            /// </summary>
            public const int MaxManualBannerMobileWidth = 1440;

            /// <summary>
            /// Default manual banner max mobile heigh is 720
            /// </summary>
            public const int MaxManualBannerMobileHeigh = 720;

            /// <summary>
            /// Default max import excel file is 5MB
            /// </summary>
            public const int DefaultMaxImportExcelSize = 5000000;

            /// <summary>
            /// Default max import excel file is 5MB
            /// </summary>
            public const int DefaultMaxImportExcelSizeDisplay = 5;

            /// <summary>
            /// id of DealList page in ADR_BackLink function of LinkExtensions class.
            /// </summary>
            public const int DefaultReturnIdOfPhotoUploadPage = 16;

            /// <summary>
            /// Use for valid if imported internal link excel file has less than 500 records 
            /// </summary>
            public const int MaxImportInternalLinkRecords = 500;

            /// <summary>
            /// Default max deal price menu file is 5MB
            /// </summary>
            public const int DefaultDealPriceMenuSize = 5000000;

            /// <summary>
            /// Default max deal price menu file is 5MB for message
            /// </summary>
            public const int DefaultDealPriceMenuSizeText = 5;

            /// <summary>
            /// Default manual icon menu max width is 24
            /// </summary>
            public const int MaxManualIconWidth = 24;

            /// <summary>
            /// Default manual icon menu max heigh is 18
            /// </summary>
            public const int MaxManualIconHeigh = 18;

            /// <summary>
            /// Date string min date convention
            /// </summary>
            public const string MinDate = "01/01/2015";

            /// <summary>
            /// Date string max date convention
            /// </summary>
            public const string MaxDate = "01/01/2100";

            /// <summary>
            /// định dạng ngày trong code
            /// </summary>
            public const string SystemDateFormat = "yyyy-MM-dd";

            /// <summary>
            /// định dạng ngày trong code js
            /// </summary>
            public const string SystemDateFormatJs = "yyyy-mm-dd";

            /// <summary>
            /// định dạng ngày hiển thị
            /// </summary>
            public const string DisplayDateFormat = "dd/MM/yyyy";

            /// <summary>
            /// DateShortFormat = "MM/dd/yyyy"
            /// </summary>
            public const string EnglishDateShortFormat = "MM/dd/yyyy";

            /// <summary>
            /// "ddMMyyyyhhmmss" Used for adding current datetime to report export file name;
            /// </summary>
            public const string ReportDateTimeFormat = "ddMMyyyyhhmmss";



            /// <summary>
            /// Default minimum string length for name, note, description (value = 2)
            /// </summary>
            public const int NoteDescMinLength = 2;

            /// <summary>
            /// Size 180KB
            /// </summary>
            public const int Size180KB = 184320;

            /// <summary>
            /// Size 400KB
            /// </summary>
            public const int Size400KB = 409600;

            

            public static SelectListItem DefaultSelectListItem = new SelectListItem()
            {
                Text = LayoutResource.Shared_SelectOpt_All,
                Value = "0",
            };

            public static SelectListItem RequireSelectListItem = new SelectListItem()
            {
                Text = LayoutResource.Shared_SelectOpt_Require,
                Value = "0",
            };
            public static SelectListItem DefaultStateSelectListItem = new SelectListItem()
            {
                Text = LayoutResource.Shared_SelectOpt_All,
                Value = "-1",
            };
        }

        #region Arguments

        /// <summary>
        /// rpp
        /// </summary>
        public const string PageSize = "rpp";
        /// <summary>
        /// p
        /// </summary>
        public const string Page = "p";
        /// <summary>
        /// org
        /// </summary>
        public const string Organization = "org";
        /// <summary>
        /// kw
        /// </summary>
        public const string Keyword = "kw";
        /// <summary>
        /// ran
        /// </summary>
        public const string Rank = "ran";
        /// <summary>
        /// opener
        /// </summary>
        public const string OpenerUrl = "opener";
        /// <summary>
        /// dct
        /// </summary>
        public const string DateCombineType = "dct";
        /// <summary>
        /// staid
        /// </summary>
        public const string StatusId = "staid";
        /// <summary>
        /// photoStaId
        /// </summary>
        public const string PhotoStatusId = "photoStaId";
        /// <summary>
        /// For photo & content produce search param 
        /// </summary>
        public const string ReusedPhotoContent = "reuspct";
        /// <summary>
        /// photoStaId
        /// </summary>
        public const string ProductionUserId = "prduid";
        /// <summary>
        /// labelStatusId
        /// </summary>
        public const string LabelStatusId = "labelId";
        /// <summary>
        /// Photo type id
        /// </summary>
        public const string PhotoTypeId = "photoTypeId";

        /// <summary>
        /// Deal id
        /// </summary>
        public const string DealId = "dealId";

        /// <summary>
        /// Deal idview
        /// </summary>
        public const string DidView = "didview";

        /// <summary>
        /// Deal idview
        /// </summary>
        public const string Rewrite = "rewt";
        
        /// <summary>
        /// typid
        /// </summary>
        public const string TypeId = "typid";
        /// <summary>
        /// rdop
        /// </summary>
        public const string RedirectOption = "rdop";
        /// <summary>
        /// merchant id
        /// </summary>
        public const string MerchantId = "mid";
        /// <summary>
        /// price option id
        /// </summary>
        public const string PriceOptionId = "proid";
        /// <summary>
        /// label id
        /// </summary>
        public const string LabelId = "lbid";
        /// <summary>
        /// frda
        /// </summary>
        public const string FromDate = "frda";
        /// <summary>
        /// toda
        /// </summary>
        public const string ToDate = "toda";
        /// <summary>
        /// cuid
        /// </summary>
        public const string CreatedUserId = "cuid";
        /// <summary>
        /// catid
        /// </summary>
        public const string CategoryId = "catid";
        /// <summary>
        /// cityid
        /// </summary>
        public const string CityId = "cityid";
        /// <summary>
        /// Deal in process photo and content request param
        /// </summary>
        public const string PhotoAndContentInProcess = "prc";
        /// <summary>
        /// Param from photo content produce dashboard only
        /// </summary>
        public const string FromDashboard = "frdasb";
        /// <summary>
        /// Manual deal group id
        /// </summary>
        public const string ManualDealGroupId = "mndgid";
        /// <summary>
        /// Landingpage group id
        /// </summary>
        public const string LandingPageGroupId = "ldpgid";
        /// <summary>
        /// Flash Sale Status Id
        /// </summary>
        public const string FlashSaleStatusId = "fssid";
        /// <summary>
        /// ditype
        /// </summary>
        public const string DealInformType = "ditype";
        /// <summary>
        /// start date from
        /// </summary>
        public const string StartFromDate = "sfrd";
        /// <summary>
        /// start date to
        /// </summary>
        public const string StartToDate = "stod";
        /// <summary>
        /// end date from
        /// </summary>
        public const string EndFromDate = "efrd";
        /// <summary>
        /// end date to
        /// </summary>
        public const string EndToDate = "etod";
        /// <summary>
        /// Contract group id for SXHA&ND search param
        /// </summary>
        public const string ContractGroupId = "ctgrid";
        /// <summary>
        /// The constant to nameming for temp data pass from controller to view
        /// </summary>
        public const string DealGlobalNotify = "GlobalDealNotify";
        /// <summary>
        /// Used for temp data key
        /// </summary>
        public const string ActionMessage = "ActionMsg";
        /// <summary>
        /// Table name query
        /// </summary>
        public const string TableName = "tbn";
        /// <summary>
        /// Is save filter deal list
        /// </summary>
        public const string ProductionIsSaveFilter = "prd_isf";
        /// <summary>
        /// Saved filter deal list cookiename
        /// </summary>
        public const string ProductionSavedFilterCookieName = "adr_ck_prd_isf";
        /// <summary>
        /// Is save filter deal list
        /// </summary>
        public const string DealIsSaveFilter = "deal_isf";
        /// <summary>
        /// Saved filter deal list cookiename
        /// </summary>
        public const string DealSavedFilterCookieName = "adr_ck_deal_isf";
        /// <summary>
        /// Dealtype
        /// </summary>
        public const string DealType = "dt";

        /// <summary>
        /// Used for temp data template
        /// </summary>
        public const string NotSecureContentError = "NotSecureContentError";
        /// <summary>
        /// Used for temp data
        /// </summary>
        public const string MaxLengthError = "MaxLengthError";
        /// <summary>
        /// Used for temp data
        /// </summary>
        public const string MinLengthError = "MinLengthError";
        /// <summary>
        /// Used for temp data
        /// </summary>
        public const string ErrorMessage = "ErrorMessage";
        /// <summary>
        /// Used for temp data
        /// </summary>
        public const string SaveSuccessfull = "SaveSuccessfull";
        /// <summary>
        /// "DealRestoreError" Used for temp data
        /// </summary>
        public const string DealRestoreError = "DealRestoreError";
        /// <summary>
        /// "ResendSuccessfully" used for temp data
        /// </summary>
        public const string ResendSuccessfully = "ResendSuccessfully";
        /// <summary>
        /// "IsPostback" used for temp data
        /// </summary>
        public const string IsPostback = "IsPostback";
        /// <summary>
        /// "ProductionRetunMessage" used for temp data
        /// </summary>
        public const string ProductionRetunMessage = "ProductionRetunMessage";
        /// <summary>
        /// "CommnetContent" used for temp data
        /// </summary>
        public const string CommnetContent = "CommnetContent";
        /// <summary>
        /// "DealDemoCommentId" used for temp data
        /// </summary>
        public const string DealDemoCommentId = "DealDemoCommentId";
        /// <summary>
        /// "RoleMessage" used for temp data
        /// </summary>
        public const string RoleMessage = "RoleMessage";
        /// <summary>
        /// User for keeping previous uri
        /// </summary>
        public const string SessionPreviousUri = "SessionPreviousUri";
        /// <summary>
        /// ^[^<>]+$
        /// </summary>
        public const string NotContainOpenCloseTagHtmlCharReg = @"^(?:(?!(<[\w\d\/\?]+(.|\n)*?>))(.|\n))*$";//"^[^<>]+$";

        /// <summary>
        /// Regex check a string that not contain script tag
        /// </summary>
        public const string NotContainScriptTag = @"^(?:(?!(<script(.|\n)*?))(.|\n))*$";//"^[^<>]+$";

        /// <summary>
        /// Regex check a string that not contain script or style tag
        /// </summary>
        public const string NotContainScriptOrStyleTag = @"^(?:(?!(<(script|style)(.|\n)*?))(.|\n))*$";//"^[^<>]+$";

        /// <summary>
        /// /^[^<>]+$/
        /// </summary>
        public const string SpacialCharReg = @"^(<[^\s])|(?:(?!(<[\w\d\/\?]+(.|\n)*?>)|(<[^\s]))(.|\n))*$";//"/^[^<>]+$/";

        /// <summary>
        /// ^[a-zA-Z0-9\-]+$
        /// </summary>
        public const string LetterCharacterReg = @"^[a-zA-Z0-9\-]+$";

        /// <summary>
        /// Pattern for detach domain name 
        /// </summary>
        public const string DomainNamePattern = @"://(?<host>([a-z\\d][-a-z\\d]*[a-z\\d]\\.)*[a-z][-a-z\\d]+[a-z])";

        /// <summary>
        /// ^[a-zA-Z0-9\-]+$
        /// </summary>
        public const string LetterCharacterAndWhitespaceReg = @"^[^~@#$%^*;\\|)(,:<>{}\[\]\+`'""!=?]+$";

        /// <summary>
        /// ^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$
        /// </summary>
        public const string UrlReg = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

        public const string UrlRegJavasctip = @"(http|ftp|https)://[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:/~+#-]*[\w@?^=%&amp;/~+#-])?";

        public const string UrlRegEndWithSlash = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?/$";

        public const string UrlParamReg = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$";

        public const string OnlyNumberReg = @"[0-9]*$";

        public const string DecimalNumberReg = @"^[0-9]\d*(\.\d+)?$";

        public const string KwValidRegex = @"^.*[a-zA-Z0-9]+.*$";

        /// <summary>
        /// This param is used for reconize which button in report page submit (search, export excel,...)
        /// </summary>
        public const string ReportTypeSubmit = "rptsm";

        /// <summary>
        /// "ddMMyyyyhhmmss" Used for adding current datetime to report export file name;
        /// </summary>
        public const string ReportDateTimeFormat = "ddMMyyyyhhmmss";

        /// <summary>
        /// "ddMMyyyy" Used for adding current datetime to report export file name;
        /// </summary>
        public const string ReportDateTimeNoTimeFormat = "ddMMyyyy";

        
        /// <summary>
        /// định dạng ngày trong code
        /// </summary>
        public const string SystemDateFormat = "yyyy-MM-dd";

        /// <summary>
        /// định dạng ngày trong code js
        /// </summary>
        public const string SystemDateFormatJs = "yyyy-mm-dd";

        /// <summary>
        /// định dạng ngày hiển thị
        /// </summary>
        public const string DisplayDateFormat = "dd/MM/yyyy";

        /// <summary>
        /// DateShortFormat = "MM/dd/yyyy"
        /// </summary>
        public const string EnglishDateShortFormat = "MM/dd/yyyy";


        /// <summary>
        /// Trim khoang rong cua CK div editor
        /// </summary>
        public const string CKBlankTrim = "<p><br></p>";

        /// <summary>
        /// param for previous page id
        /// </summary>
        public const string ReturnId = "rid";

         /// <summary>
        /// param for is my data
        /// </summary>
        public const string IsMy = "im";
        

        /// <summary>
        /// Notify popup position
        /// </summary>
        public const string NotifyPosition = "bottomRight";
        

         /// <summary>
        /// Browser History Key
        /// </summary>
        public const string BrowserHistoryKey = "user.history";

        /// <summary>
        /// IsBackHistoryKey
        /// </summary>
        public const string IsBackHistoryKey = "ibh";
        #endregion Arguments

        public const string SignInToken = "adr_cms2_client_tk";

        /// <summary>
        /// Internal link field
        /// </summary>
        public const string InternalLinkKeywordField = "KEYWORD";

        /// <summary>
        /// Internal link field
        /// </summary>
        public const string InternalLinkLinkField = "LINK";

        /// <summary>
        /// Internal link field
        /// </summary>
        public const string InternalLinkDescriptionField = "DESCRIPTION";

        public const string DefaultEditBtnClass = "fa fa-pencil";

        public const string AllowUploadImageExt = ".png,.jpg,.jpeg";

        public const string AllowUploadFileExt = ".gif,.jpg,.jpeg,.png,.doc,.docx,.xls,.xlsx,.pdf,.rar,.zip";
        public const string SignInTokenVisitOR = "vm_visitAnesthesiaInfo";
        //public const string TokenVisitORLink = "vm_visitORAnesthLink";

        /// <summary>
        /// Size 3MB
        /// </summary>
        public const int MaxImageSize = 3 * 1024 * 1024;

        public const int MaxMediaFileSize = 30 * 1024 * 1024;

        /// <summary>
        /// lấy icon file trừ hình ảnh
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string GetFileExtIcon(string filePath)
        {
            var ext = string.IsNullOrEmpty(filePath) ? "" : filePath.Substring(filePath.LastIndexOf('.') + 1);
            string src = "";
            switch (ext)
            {
                case "pdf":
                    src = "-pdf-o";
                    break;
                case "rar":
                case "zip":
                    src = "-archive-o";
                    break;
                case "doc":
                case "docx":
                    src = "-word-o";
                    break;
                case "xls":
                case "xlsx":
                    src = "-excel-o";
                    break;
                case "gif":
                case "jpg":
                case "jpeg":
                case "png":
                    src = "-image-o";
                    break;
                default:
                    src = "-o";
                    break;

            }
            return src;

        }

        public class DbResponseText
        {
            /// <summary>
            /// "RANK_INCREASE"
            /// </summary>
            public const string RankIncrease = "RANK_INCREASE";
        }

        public class MemberMappingFileType
        {
            public static readonly string[] AllowList = { ".xls", ".xlsx" };
        }

        public class TrimModelBinder : DefaultModelBinder
        {
            protected override void SetProperty(ControllerContext controllerContext,
              ModelBindingContext bindingContext,
              System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
            {
                if (propertyDescriptor.PropertyType == typeof(string))
                {
                    var stringValue = (string)value;
                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        stringValue = stringValue.Trim();
                        stringValue = Regex.Replace(stringValue, @" ( )+", " ");
                    }

                    value = stringValue;
                }                               

                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
            }            
        }
    }
}