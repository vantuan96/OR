using System.ComponentModel;

namespace Contract.Shared
{
    public enum ResponseCode
    {
        #region Shared 100000-100999

        [Description("No row changes")]
        NoChanged = 100000,

        [Description("Successed")]
        Successed = 100001,

        [Description("Lookup key not exist")]
        KeyNotExist = 100002,

        [Description("Shared_SystemErrorMessage")]
        Error = 100999,

        #endregion Shared 100000-100999

        #region Login 101000-101999

        Login_Failed = 101000,
        Login_UserNotActived = 101001,
        Login_HaveNotLogonPermit = 101002,

        #endregion Login 101000-101999

        #region ChangePassword 102000-102999

        ChangePassword_Successed = 102000,
        ChangePassword_WrongOldPassword = 102001,

        #endregion ChangePassword 102000-102999

        #region UserMngt 103000-103999

        UserMngt_SuccessCreated = 103000,
        UserMngt_SuccessUpdated = 103001,
        UserMngt_EmailExisted = 103002,
        UserMngt_SuccessResetPassword = 103003,
        UserMngt_SuccessLock = 103004,
        UserMngt_SuccessUnLock = 103005,
        UserMngt_SuccessDelete = 103006,
        UserMngt_UsernameExisted = 103007,
        UserMngt_UsernameNotExisted = 103008,
        UserMngt_InvalidSite = 103009,
        UserMngt_InvalidLocation = 103010,
        UserMngt_NotExisted = 1030104,

        AdminRole_SuccessCreate = 103101,
        AdminRole_SuccessUpdate = 103102,
        AdminRole_DuplicateKey = 103103,
        AdminRole_Accessdenied= -1,

        AdminRoleGroupAction_SuccessUpdate = 103111,

        #endregion UserMngt 103000-103999

        #region Language 104000-104999

        Lang_InsertSuccess = 104000,
        Lang_UpdateSuccess = 104001,

        Lang_DuplicateKey = 104002,

        #endregion Language 104000-104999

        #region MenuMngt 105000-105999

        MenuMngt_SuccessCreate = 105000,
        MenuMngt_SuccessUpdate = 105001,
        MenuMngt_SuccessUpdateStatus = 105002,
        MenuMngt_SuccessDelete = 105003,
        MenuMngt_SuccessUpdateSort = 105004,

        #endregion MenuMngt 105000-105999

        #region Tag 106000-106999

        Tag_InsertSuccess = 106000,
        Tag_UpdateSuccess = 106001,
        Tag_DuplicateKey = 106002,

        #endregion Tag 106000-106999

        #region Banner 107000-107999

        BannerPosition_InsertSuccess = 107000,
        BannerPosition_UpdateSuccess = 107001,
        BannerPosition_DuplicateKey = 107002,

        BannerGroup_InsertSuccess = 107010,
        BannerGroup_UpdateSuccess = 107011,
        BannerGroup_DuplicateKey = 107012,

        Banner_InsertSuccess = 107020,
        Banner_UpdateSuccess = 107021,
        Banner_DuplicateKey = 107022,

        BannerContent_InsertSuccess = 107030,
        BannerContent_UpdateSuccess = 107031,
        BannerContent_DuplicateKey = 107032,

        #endregion Banner 107000-107999

        #region AddressMngt 108000-108999

        AddressMngt_SuccessCreate = 108000,
        AddressMngt_SuccessUpdate = 108001,
        AddressMngt_SuccessUpdateStatus = 108002,
        AddressMngt_SuccessDelete = 108003,

        #endregion AddressMngt 108000-108999

        #region MicrositeMngt 109000-109999

        MicrositeMngt_SuccessCreate = 109000,
        MicrositeMngt_SuccessUpdate = 109001,
        MicrositeMngt_DuplicateKey = 109002,
        MicrositeMngt_SuccessDelete = 109003,

        #endregion MicrositeMngt 109000-109999

        #region Product 110000-110999

        ProductAttribute_InsertSuccess = 110000,
        ProductAttribute_UpdateSuccess = 110001,
        ProductAttribute_DuplicateKey = 110002,

        ProductAttributeContent_InsertSuccess = 110010,
        ProductAttributeContent_UpdateSuccess = 110011,
        ProductAttributeContent_DuplicateKey = 110012,

        #endregion Product 110000-110999

        #region GalleryMngt 111000-111999

        GalleryMngt_SuccessCreate = 111000,
        GalleryMngt_SuccessUpdate = 111001,
        GalleryMngt_SuccessUpdateStatus = 111002,
        GalleryMngt_SuccessDelete = 111003,
        GalleryMngt_SuccessCreateImage = 111004,
        GalleryMngt_SuccessUpdateImage = 111005,
        GalleryMngt_SuccessDeleteImage = 111006,

        #endregion

        #region ProductMngt 112000-112999

        ProductMngt_SuccessCreate = 112000,
        ProductMngt_SuccessDelete = 112001,
        ProductMngt_SuccessUpdate = 112002,

        #endregion

        #region ArticleCategory 129000-129999

        ArticleCategory_SuccessCreate = 129000,
        ArticleCategory_SuccessUpdate = 129001,
        ArticleCategory_DuplicateKey = 129002,
        ArticleCategory_SuccessUpdateSort = 129003,

        #endregion ArticleCategory 129000-129999

        #region ProductCategory 139000-139999

        ProductCategory_SuccessCreate = 139000,
        ProductCategory_SuccessUpdate = 139001,
        ProductCategory_DuplicateKey = 139002,
        ProductCategory_SuccessUpdateSort = 139003,

        #endregion ProductCategory 139000-139999

        #region Article 149000-149999

        Article_SuccessCreate = 149000,
        Article_SuccessUpdate = 149001,
        Article_DuplicateKey = 149002,
        Article_SuccessUpdatePosition = 149003,

        #endregion Article 149000-149999

        #region AdminTag 150000-150999
        AdminTag_InsertSuccess = 150000,
        AdminTag_UpdateSuccess = 150001,
        AdminTag_DuplicateKey = 150002,
        #endregion

        #region Subscribe 151000-151999
        Subscribe_Success = 151000,

        ContactMessage_Success = 151500,
        #endregion

        #region Qcs 161000-161999

        #region SiteZone 161000-161099
        [Description("Thêm thông tin khu vực kiểm tra thành công")]
        Admin_Qcs_SiteZone_InsertSuccess = 161000,
        [Description("Cập nhật thông tin khu vực kiêm tra thành công")]
        Admin_Qcs_SiteZone_UpdateSuccess = 161001,
        [Description("Bị trùng thông tin khi tạo khu vực kiêm tra")]
        Admin_Qcs_SiteZone_DuplicateInfo = 161002,
        [Description("Lỗi khi tạo thông tin khu vực kiểm tra")]
        Admin_Qcs_SiteZone_ErrorInfo = 161003,
        [Description("Khu vực không tồn tại")]
        Admin_Qcs_SiteZone_NoExists = 161004,
        Admin_Qcs_SiteZone_DeleteSuccess = 161005,


        #endregion
        #region EvaluationCalendar 161100-161199
        [Description("Thêm thông tin thành công")]
        Admin_Qcs_EvaluationCalendar_InsertSuccess = 161100,
        [Description("Cập nhật thông tin  thành công")]
        Admin_Qcs_EvaluationCalendar_UpdateSuccess = 161101,
        [Description("Bị trùng thông tin")]
        Admin_Qcs_EvaluationCalendar_DuplicateInfo = 161102,
        [Description("Lỗi khi tạo thông tin kỳ kiểm tra")]
        Admin_Qcs_EvaluationCalendar_ErrorInfo = 161103,
        [Description("Kỳ kiểm tra không tồn tại")]
        Admin_Qcs_EvaluationCalendar_NoExists = 161104,

        Admin_Qcs_EvaluationCalendar_InsertManySuccess = 161110,

        #endregion

        #region EvaluationSiteZone (checklist items) 161200-161299
        [Description("Thêm thông tin đánh giá cho khu vực thành công")]
        Admin_Qcs_EvaluationSiteZone_InsertSuccess = 161200,
        [Description("Thêm mới/ Cập nhật thông tin đánh giá cho khu vực  thành công")]
        Admin_Qcs_EvaluationSiteZone_UpdateSuccess = 161201,
        [Description("Lỗi khi tạo thông tin đánh giá")]
        Admin_Qcs_EvaluationSiteZone_ErrorInfo = 161203,
        [Description("Thông tin đánh giá cho khu vực không được rổng")]
        Admin_Qcs_EvaluationSiteZone_NoExists = 161204,

        [Description("Sao chép đánh giá cho khu vực thành công")]
        Admin_Qcs_EvaluationSiteZone_CloneZone_Success = 161205,
        [Description("Sao chép đánh giá cho khu vực thất bại")]
        Admin_Qcs_EvaluationSiteZone_CloneZone_False = 161206,
        [Description("Lổi khi sao chép đánh giá cho khu vực")]
        Admin_Qcs_EvaluationSiteZone_CloneZone_ErrorInfo = 161207,
        [Description("Thông tin sao chép đánh giá cho khu vực không được rổng")]
        Admin_Qcs_EvaluationSiteZone_CloneZone_NoExists = 161208,
        [Description("Thông tin sao chép tiêu chí đánh giá cho khu vực không tồn tại")]
        Admin_Qcs_EvaluationSiteZone_CloneZone_NoItemsExists = 161209,

        #endregion


        #region EvaluationCriteria 161300-161399

        Admin_Qcs_Invalid_Data = 161300,
        Admin_Qcs_EvaluationCriteria_Error = 161301,
        Admin_Qcs_EvaluationCriteria_AddNew = 161302,
        Admin_Qcs_EvaluationCriteria_Updated = 161303,
        Admin_Qcs_EvaluationCriteria_Delete = 161304,

        [Description("Tiêu chí đánh giá đã có trong hệ thống")]
        Admin_Qcs_EvaluationCriteria_Exists = 161305,

        [Description("Tiêu chí đánh giá đã không có trong hệ thống.")]
        Admin_Qcs_EvaluationCriteria_NotExists = 161306,

        [Description("Thông tin tiêu chí đánh giá cập nhật đã có trong hệ thống.")]
        Admin_Qcs_EvaluationCriteria_DuplicateInfo = 161307,

        [Description("Biên bản vi phạm thêm mới thành công")]
        Admin_Qcs_ViolationRecord_AddNew = 161308,

        [Description("Lỗi trong quá thêm mới biên bản vi phạm.")]
        Admin_Qcs_ViolationRecord_Error = 161309,

        [Description("Biên bản vi phạm đã có trong hệ thống.")]
        Admin_Qcs_ViolationRecord_DuplicateInfo = 161310,

        [Description("Biên bản vi phạm không có trong hệ thống.")]
        Admin_Qcs_ViolationRecord_NotExists = 161311,

        [Description("Biên bản vi phạm đã được cập nhật thành công")]
        Admin_Qcs_ViolationRecord_Updated = 161312,

        /* Evaluation criteria group */
        Admin_Qcs_EvaluationCriteriaGroup_AddNew = 161313,
        Admin_Qcs_EvaluationCriteriaGroup_Updated = 161314,
        Admin_Qcs_EvaluationCriteriaGroup_Deleted = 161315,
        Admin_Qcs_EvaluationCriteriaGroup_UpdateError = 161316,
        Admin_Qcs_EvaluationCriteriaGroup_DeleteError = 161317,        
        Admin_Qcs_EvaluationCriteriaGroup_Exists = 161318,
        Admin_Qcs_EvaluationCriteriaGroup_NoExists = 161319,

        /* Evaluation categories */
        Admin_Qcs_EvaluationCategory_AddNew = 161320,
        Admin_Qcs_EvaluationCategory_Updated = 161321,
        Admin_Qcs_EvaluationCategory_Deleted = 161322,
        Admin_Qcs_EvaluationCategory_UpdateError = 161323,
        Admin_Qcs_EvaluationCategory_DeleteError = 161324,
        Admin_Qcs_EvaluationCategory_Exists = 161325,
        Admin_Qcs_EvaluationCategory_NoExists = 161326,

        #endregion

        #region Violations 161400-161499

        Admin_Qcs_ZoneViolation_InsertSuccess = 161400,
        Admin_Qcs_ZoneViolation_UpdateSuccess = 161401,
        Admin_Qcs_ZoneViolation_DeleteSucess = 161402,
        Admin_Qcs_ZoneViolation_ErrorInfo = 161403,
        Admin_Qcs_ZoneViolation_NoExists = 161404,
        #endregion


        #region AdminDepartment 161600-161699
        Admin_Qcs_AdminDepartment_Error = 161601,
        [Description("Thêm mới bộ phận thành công.")]
        Admin_Qcs_AdminDepartment_Insert = 161602,
        [Description("Cập nhật bộ phận mới thành công.")]
        Admin_Qcs_AdminDepartment_Updated = 161603,
        [Description("Xóa bộ phận thành công")]
        Admin_Qcs_AdminDepartment_Delete = 161604,

        [Description("Thông tin bộ phận cập nhật đã có trong hệ thống")]
        Admin_Qcs_AdminInsertUpdate_DuplicateInfo = 161605,

        [Description("Thông tin bộ phận không có trong hệ thống.")]
        Admin_Qcs_AdminDepartment_NotExists = 161606,

        [Description("Bộ phận này có bộ phận con nên không thể xóa.")]
        Admin_Qcs_AdminDepartment_HaveChild = 161607,

        [Description("Tên bộ phận đã tồn tại trong hệ thống.")]
        Admin_Qcs_AdminDepartment_Name_Existsed = 161608,

        [Description("Mã bộ phận đã tồn tại trong hệ thống.")]
        Admin_Qcs_AdminDepartment_Code_Existsed = 161609,
        #endregion

        #region EvaluationCategoryGroup 161500-161599

        Admin_Qcs_EvaluationCategoryGroup_Error = 161501,
        [Description("Thêm mới loại hạng mục thành công.")]
        Admin_Qcs_EvaluationCategoryGroup_Insert = 161502,
        [Description("Cập nhật loại hạng mục mới thành công.")]
        Admin_Qcs_EvaluationCategoryGroup_Updated = 161503,
        [Description("Xóa loại hạng mục thành công")]
        Admin_Qcs_EvaluationCategoryGroup_Delete = 161504,

        [Description("Thông tin loại hạng mục cập nhật đã có trong hệ thống")]
        Admin_Qcs_CategoryGroupInsertUpdate_DuplicateInfo = 161505,

        [Description("Thông tin loại hạng mục không có trong hệ thống.")]
        Admin_Qcs_EvaluationCategoryGroup_NotExists = 161506,

        #endregion



        #endregion

        #region MediaFile 161700-161799

        MediaFile_SuccessCreate = 161701,
        MediaFile_SuccessUpdate = 161702,

        #endregion

        #region RouteMngt 162000-162999

        RouteMngt_SuccessCreate = 162000,
        RouteMngt_SuccessUpdate = 162001,
        RouteMngt_SuccessDelete = 162002,

        #endregion

        #region Device 163000-163999
        DeviceMngt_SuccessCreate=163001,
        DeviceMngt_SuccessUpdate = 163002,
        DeviceMngt_DuplicateImei = 163003,
        DeviceMngt_ChangeStatusSuccess = 163010,
        #endregion

        #region Location 164000-164999
        LocationMngt_SuccessCreate =164001,
        LocationMngt_SuccessUpdate = 164002,
        LocationMngt_DuplicateName = 164003,
        LocationMngt_DuplicateCode = 164004,
        LocationMngt_SuccessDelete = 164010,
        LocationMngt_IsUsing = 164011,
        #endregion

        #region QuestionGroup 165000-165999
        QuestionGroup_SuccessCreate = 165001,
        QuestionGroup_SuccessUpdate = 165002,
        QuestionGroup_DuplicateName = 165003,

        QuestionGroup_SuccessDelete = 165004,
        QuestionGroup_IsUsing = 165005,
        #endregion

        #region Question 166000-166999

        Question_SuccessCreate = 166001,
        Question_SuccessUpdate = 166002,
        Question_DuplicateName = 166003,
        Question_ExceedAmountOfQuestion  = 16604,

        Question_SuccessDelete = 166010,

        #endregion

        #region AnswerMapping 167000-167999
        AnswerMapping_SuccessUpdate = 167001,
        #endregion 

        #region Reason 168000-168999
        Reason_SuccessCreate= 168001,
        Reason_SuccessUpdate = 168002,
        Reason_DuplicateName = 166003,
        Reason_SuccessDelete = 168004,
        #endregion 

        #region Check list  200000-2000010
        CheckList_SuccessCreate = 200001,
        CheckList_SuccessUpdate = 200002,
        CheckList_SuccessDelete = 200003,
        CheckList_SuccessConfirm = 200004,
        CheckList_DuplicateExist=200005,
        CheckList_NoAssignItem=200006,
        CheckList_ActiveCheckList = 200007,
        CheckList_NotActiveWhenNoHavingOwner = 200008,
        #endregion

        #region Check list details  200010-2000020
        CheckListDetail_SuccessCreate = 200011,
        CheckListDetail_SuccessUpdate = 200012,
        CheckListDetail_SuccessDelete = 200013,
        CheckListDetail_SuccessConfirm = 200014,
        #endregion

        #region Check list map  200020-2000030
        CheckListMap_SuccessCreate = 200021,
        CheckListMap_SuccessUpdate = 200022,
        #endregion

        #region Check list operation  200030-2000040
        CheckListOperation_SuccessCreate = 200031,
        CheckListOperation_SuccessUpdate = 200032,
        #endregion

        #region Master data  200100-200900
        //PnLList
        PnLList_SuccessCreate = 200101,
        PnLList_SuccessUpdate = 200102,
        PnLList_SuccessDelete = 200103,
        PnLList_SuccessConfirm = 200104,
        PnLList_DuplicateName = 200105,
        PnLList_DuplicateCode = 200106,

        //PnLBUList
        PnLBuList_SuccessCreate = 200201,
        PnLBuList_SuccessUpdate = 200202,
        PnLBuList_SuccessDelete = 200203,
        PnLBuList_SuccessConfirm = 200204,
        PnLBuList_DuplicateName = 200205,
        PnLBuList_DuplicateCode = 200206,

        //DepartmentList
        DepartmentList_SuccessCreate = 200301,
        DepartmentList_SuccessUpdate = 200302,
        DepartmentList_SuccessDelete = 200303,
        DepartmentList_SuccessConfirm = 200304,

        //StaffList
        StaffList_SuccessCreate = 200401,
        StaffList_SuccessUpdate = 200402,
        StaffList_SuccessDelete = 200403,
        StaffList_SuccessConfirm = 200404,
        StaffList_DuplicateName = 200405,
        StaffList_DuplicateCode = 200406,

        //Đvhc
        Dvhc_SuccessCreate = 200501,
        Dvhc_SuccessUpdate = 200502,
        Dvhc_SuccessDelete = 200503,
        Dvhc_SuccessConfirm = 200504,
        Dvhc_DuplicateName = 200505,
        Dvhc_DuplicateCode = 200506,
        Dvhc_IsUsing = 200507,

        //PnLBuAttributeGroup
        PBAG_SuccessCreate = 200601,
        PBAG_SuccessUpdate = 200602,
        PBAG_SuccessDelete = 200603,
        PBAG_SuccessConfirm = 200604,
        PBAG_DuplicateCode = 200605,
        PBAG_DuplicateName = 200606,

        //PnLBuAttribute
        PnLBuAttribute_SuccessCreate = 200701,
        PnLBuAttribute_SuccessUpdate = 200702,
        PnLBuAttribute_SuccessDelete = 200703,
        PnLBuAttribute_SuccessConfirm = 200704,
        PnLBuAttribute_DuplicateName = 200705,
        PnLBuAttribute_DuplicateCode = 200706,

        //Region
        Region_SuccessCreate = 200801,
        Region_SuccessUpdate = 200802,
        Region_SuccessDelete = 200803,
        Region_SuccessConfirm = 200804,
        Region_DuplicateName = 200805,
        Region_DuplicateCode = 200806,

        //DepartmentTitle
        DepartmentTitle_SuccessCreate = 200901,
        DepartmentTitle_SuccessUpdate = 200902,
        DepartmentTitle_SuccessDelete = 200903,
        DepartmentTitle_SuccessConfirm = 200904,
        DepartmentTitle_DuplicateName = 200905,
        DepartmentTitle_DuplicateCode = 200906,

        //Level
        Level_SuccessCreate = 200101,
        Level_SuccessUpdate = 200102,
        Level_SuccessDelete = 200103,
        Level_SuccessConfirm = 200104,
        Level_DuplicateName = 200105,
        Level_DuplicateCode = 200106,

        //Basis Group
        BasisGroup_SuccessCreate = 200111,
        BasisGroup_SuccessUpdate = 200112,
        BasisGroup_SuccessDelete = 200113,
        BasisGroup_SuccessConfirm = 200114,
        BasisGroup_DuplicateName = 200115,
        BasisGroup_DuplicateCode = 200116,

        //Basis
        Basis_SuccessCreate = 200121,
        Basis_SuccessUpdate = 200122,
        Basis_SuccessDelete = 200123,
        Basis_SuccessConfirm = 200124,
        Basis_DuplicateName = 200125,
        Basis_DuplicateCode = 200126,

        // system checklist
        SystemMngt_NoExists = 200131,
        SystemMngt_SuccessDeleted = 200132,
        SystemMngt_Error = 200133,
        SystemMngt_SuccessUpdated=200134,
        SystemMngt_SuccessCreated=200135,
        SystemMngt_DuplicateItemExist=200136,
        SystemMngt_AssignOwnerSuccessed=200137,
        // item checklist
        ItemMngt_NoExists = 200141,
        ItemMngt_SuccessDeleted = 200142,
        ItemMngt_Error = 200143,
        ItemMngt_SuccessUpdated = 200144,
        ItemMngt_SuccessCreated = 200145,
        ItemMngt_DuplicateItemExist=200146,

        //operation checklist
        [Description("Cập nhật thông tin  thành công")]
        OperationCheckList_UpdateSuccess = 2001561,
        OperationCheckList_NoExists = 2001562,
        [Description("Chuyển trạng thái thành công")]
        OperationCheckList_ChangeStatus = 2001563,

        //log
        [Description("Ghi nhận log thành công")]
        LogObject_CreatedSuccess = 2001571,
        //log
        [Description("Có lỗi trong quá trình xử lý")]
        LogObject_Error = 20015712,
        
        #endregion


        #region OR   200160100-200160199
        OR_SuccessCreate = 200160100,
        OR_SuccessUpdate = 200160101,
        OR_SuccessDelete = 200160102,
        OR_SuccessConfirm = 200160103,
        OR_Room_SuccessShow = 200160104,
        OR_Room_SuccessHide = 200160105,
        OR_Room_SuccessDelete= 200160106
        #endregion
    }
}