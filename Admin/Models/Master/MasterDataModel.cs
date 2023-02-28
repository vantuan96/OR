using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Admin.Helper;
using Admin.Resource;
using Contract.MasterData;
using Contract.User;

namespace Admin.Models.Master
{
    public class CreateUpdatePnLListModel
    {
        public int Id { get; set; }
        public int PnLListId { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string PnLListCode { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string PnLListName { get; set; }

        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Master_PnLList_Label_Title_FullAddress", ResourceType = typeof(LayoutResource))]
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string FullAddress { get; set; }
        public int StatusId { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public List<PnLListStatusContract> ListPnLsStatus { get; set; }
    }

    public class CreateUpdatePnLBuListModel
    {
        public int Id { get; set; }

        public int PnLBUListId { get; set; }

        [Required]
        [Display(Name = "Master_PnLBuList_Label_Title", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string PnLBUListCode { get; set; }

        [Display(Name = "Master_PnLBuList_Label_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }

        public int PnLListId { get; set; }

        public int Sort { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public List<PnLListContract> ListPnLList { get; set; }

    }

    public class CreateUpdateDepartmentListModel
    {
        public int Id { get; set; }
        public int DepartmentListId { get; set; }

        [Required]
        [Display(Name = "Master_DepartmentList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(512)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string DepartmentListCode { get; set; }     
        public string Description { get; set; }
        public int Type { get; set; }
        public string ParentCode { get; set; }
        public int Level { get; set; }
        public int Sort { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public List<DepartmentListTypeContract> ListDepartmentType { get; set; }
    }

    public class CreateUpdateStaffListModel
    {
        public int Id { get; set; }
        public int StaffListId { get; set; }

        [Required]
        [Display(Name = "Master_StaffList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string StaffListCode { get; set; }

        [Required]
        [Display(Name = "Master_StaffList_Label_Title_FullName", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string FullName { get; set; }
        public int General { get; set; }

        [Required]
        [Display(Name = "Master_StaffList_Label_Title_Email", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Master_StaffList_Label_Title_PhoneNo", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string PhoneNo { get; set; }
        [Required]
        [Display(Name = "Master_StaffList_Label_Title_UnitCode", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public int UnitCodeId { get; set; }
        [Required]
        [Display(Name = "Master_StaffList_Label_Title_CentreCode", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public int CentreCodeId { get; set; }
        [Required]
        [Display(Name = "Master_StaffList_Label_Title_DepartmentCode", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public int DepartmentCodeId { get; set; }
        [Required]
        [Display(Name = "Master_StaffList_Label_Title_GroupName", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public int GroupCodeId { get; set; }

        [Required]
        [Display(Name = "Master_StaffList_Label_Title_OfficeLocation", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string OfficeLocation { get; set; }

        [Required]
        [Display(Name = "Master_StaffList_Label_Title_CityCode", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string CityCode { get; set; }
        public int TitleCodeId { get; set; }

        [Required]
        [Display(Name = "Master_StaffList_Label_Title_LevelCode", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string LevelCode { get; set; }
        public int StatusId { get; set; }

        [Required]
        [Display(Name = "Master_StaffList_Label_Title_ManageCode", ResourceType = typeof(LayoutResource))]
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string ManagerCode { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime JoinCompanyDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateBy { get; set; }
        public bool Visible { get; set; }
        public List<DepartmentTitleContract> ListDepartmentTitle { get; set; }
        public List<DepartmentStatusContract> ListDepartmentStatus { get; set; }
        public List<DepartmentGeneralContract> ListDepartmentGeneral { get; set; }
        public List<LocationContract> ListDepartmentUnit { get; set; }
        public List<LocationContract> ListDepartmentCentre { get; set; }
        public List<LocationContract> ListDepartmentDepartment { get; set; }
        public List<LocationContract> ListDepartmentGroup { get; set; }
    }

    public class CreateUpdateDvhcModel
    {
        public int AdministrativeUnitsId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Master_Dvhc_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string AdministrativeUnitsVN { get; set; }
        public string AdministrativeUnitsEN { get; set; }
        public int LevelNo { get; set; }

        public int ParentId { get; set; }

        [Display(Name = "Dvhc_Parent_Name", ResourceType = typeof(LayoutResource))]
        public string ParentName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Dvhc_Parent_Prefix", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Prefix { get; set; }
        public List<PrefixContract> ListPrefix { get; set; }
    }

    public class CreateUpdatePnLBuAttributeGroupModel
    {
        public int Id { get; set; }
        public int PnLBuAttributeGroupId { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string PnLBuAttributeGroupCode { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string PnLBuAttributeGroupName { get; set; }

        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(250)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class CreateUpdatePnLBuAttributeModel
    {
        public int Id { get; set; }
        public int PnLBuAttributeId { get; set; }
        [Required]
        [Display(Name = "Master_PnLAttribute_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string PnLBuAttributeCode { get; set; }
        [Required]
        [Display(Name = "Master_PnLAttribute_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string PnLBuAttributeName { get; set; }

        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(250)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        public bool Visible { get; set; }

        [Required]
        [Display(Name = "Master_PnLAttribute_Label_Title_PnLAttributeGroupId", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public int PnLAttributeGroupId { get; set; }
        [Required]
        [Display(Name = "Master_PnLAttribute_Label_Title_PnLListId", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public int PnLListId { get; set; }

        [Required]
        [Display(Name = "Master_PnLAttribute_Label_Title_PnLBUListId", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public int PnLBUListId { get; set; }
        
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public List<PnLBuAttributeGroupContract> ListPnLAttributeGroup { get; set; }
        public List<PnLListContract> ListPnLList { get; set; }
        public List<PnLBuListContract> ListPnLBuList { get; set; }
    }

    public class CreateUpdateRegionModel
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string RegionCode { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string RegionName { get; set; }

        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(250)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class CreateUpdateDepartmentTitleModel
    {
        public int Id { get; set; }
        public int DepartmentTitleId { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string DepartmentTitleCode { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string DepartmentTitleName { get; set; }

        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(250)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class CreateUpdateLevelModel
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string LevelCode { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string LevelName { get; set; }
        [Required]
        [Display(Name = "Master_PnLList_Label_Title_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(250)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class CreateUpdateBasisGroupModel
    {
        public int Id { get; set; }
        public int BasisGroupId { get; set; }
        [Required]
        [Display(Name = "BasisGroup_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string BasisGroupCode { get; set; }
        [Required]
        [Display(Name = "BasisGroup_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string BasisGroupName { get; set; }

        [Required]
        [Display(Name = "BasisGroup_Label_Title_Address", ResourceType = typeof(LayoutResource))]
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Address { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class CreateUpdateBasisModel
    {
        public int Id { get; set; }
        public int BasisId { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string BasisCode { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string BasisName { get; set; }
        public int BasisGroupId { get; set; }
        public int PnLListId { get; set; }
        public int PnLBUListId { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(250)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_RefCode", ResourceType = typeof(LayoutResource))]
        [StringLength(15)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string RefCode { get; set; }
        public int StatusId { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_FullName", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_StatusDescription", ResourceType = typeof(LayoutResource))]
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string StatusDescription { get; set; }
        public DateTime OpeningDate { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_Address", ResourceType = typeof(LayoutResource))]
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_Manager", ResourceType = typeof(LayoutResource))]
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Manager { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_ManagerPhone", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string ManagerPhone { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_SitePhone", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string SitePhone { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_SiteEmail", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string SiteEmail { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_AreaManager", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string AreaManager { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_AreaManagerPhone", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string AreaManagerPhone { get; set; }

        [Required]
        [Display(Name = "Basis_Label_Title_AreaManagerEmail", ResourceType = typeof(LayoutResource))]
        [StringLength(150)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string AreaManagerEmail { get; set; }
        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public bool Visible { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public string PnLListName { get; set; }
        public string PnLBuListName { get; set; }
        public string StatusName { get; set; }
        public string BasisGroupName { get; set; }

        public List<BasisGroupContract> ListBasisGroup { get; set; }
        public List<PnLListContract> ListPnLList { get; set; }
        public List<PnLBuListContract> ListPnLBuList { get; set; }
        public List<DvhcContract> ListCity{ get; set; }
        public List<DvhcContract> ListDistrict { get; set; }
        public List<DvhcContract> ListWard { get; set; }
        public List<BasisStatusContract> ListBasisStatus { get; set; }
        public List<LocationContract> ListDepartment { get; set; }
        public List<LocationContract> ListDepartmentView { get; set; }
        public List<StaffListContract> ListStaff { get; set; }
    }
}