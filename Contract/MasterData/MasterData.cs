using System;
using ProtoBuf;
using System.Collections.Generic;
using Contract.CheckList;

namespace Contract.MasterData
{
    [ProtoContract]
    public class PnLListContract
    {
        [ProtoMember(1)]
        public int PnLListId { get; set; }
        [ProtoMember(2)]
        public string PnLListCode { get; set; }
        [ProtoMember(3)]
        public string PnLListName { get; set; }
        [ProtoMember(4)]
        public string Description { get; set; }
        [ProtoMember(5)]
        public string FullAddress { get; set; }
        [ProtoMember(6)]
        public int StatusId { get; set; }
        [ProtoMember(7)]
        public bool Visible { get; set; }
        [ProtoMember(8)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(9)]
        public int CreatedBy { get; set; }
        [ProtoMember(10)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(11)]
        public int UpdatedBy { get; set; }
        [ProtoMember(12)]
        public string StatusName { get; set; }
    }
    public class PnLListStatusContract
    {
        [ProtoMember(1)]
        public int StatusId { get; set; }
        [ProtoMember(2)]
        public string StatusName { get; set; }
    }
    public class PnLBuListContract
    {
        [ProtoMember(1)]
        public int PnLBuListId { get; set; }
        [ProtoMember(2)]
        public string PnLBuListCode { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public int PnLListId { get; set; }
        [ProtoMember(5)]
        public int? Sort { get; set; }
        [ProtoMember(6)]
        public bool Visible { get; set; }
        [ProtoMember(7)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(8)]
        public int CreatedBy { get; set; }
        [ProtoMember(9)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(10)]
        public int UpdatedBy { get; set; }
        [ProtoMember(11)]
        public string PnLListCode { get; set; }
    }
    public class DepartmentListContract
    {
        [ProtoMember(1)]
        public int DepartmentListId { get; set; }
        [ProtoMember(2)]
        public string DepartmentListCode { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public int Type { get; set; }
        [ProtoMember(5)]
        public string ParentCode { get; set; }
        [ProtoMember(6)]
        public int Level { get; set; }
        [ProtoMember(7)]
        public int Sort { get; set; }
        [ProtoMember(8)]
        public bool Visible { get; set; }
        [ProtoMember(9)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(10)]
        public int CreatedBy { get; set; }
        [ProtoMember(11)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(12)]
        public int UpdatedBy { get; set; }
        [ProtoMember(13)]
        public string DepartmentTypeName { get; set; }
    }
    public class DepartmentListTypeContract
    {
        [ProtoMember(1)]
        public int DepartmentTypeId { get; set; }
        [ProtoMember(2)]
        public string DepartmentTypeName { get; set; }
    }

    public class DepartmentStatusContract
    {
        [ProtoMember(1)]
        public int DepartmentStatusId { get; set; }
        [ProtoMember(2)]
        public string DepartmentStatusName { get; set; }
    }
    public class DepartmentGeneralContract
    {
        [ProtoMember(1)]
        public int DId { get; set; }
        [ProtoMember(2)]
        public string GeneralVN { get; set; }
        [ProtoMember(3)]
        public string GeneralEN { get; set; }
    }
    public class StaffListContract
    {
        [ProtoMember(1)]
        public int StaffListId { get; set; }
        [ProtoMember(2)]
        public string StaffListCode { get; set; }
        [ProtoMember(3)]
        public string FullName { get; set; }
        [ProtoMember(4)]
        public int General { get; set; }
        [ProtoMember(5)]
        public string Email { get; set; }
        [ProtoMember(6)]
        public string PhoneNo { get; set; }
        [ProtoMember(7)]
        public int UnitCodeId { get; set; }
        [ProtoMember(8)]
        public int CentreCodeId { get; set; }
        [ProtoMember(9)]
        public int DepartmentCodeId { get; set; }
        [ProtoMember(10)]
        public int GroupCodeId { get; set; }
        [ProtoMember(11)]
        public string OfficeLocation { get; set; }
        [ProtoMember(12)]
        public string CityCode { get; set; }
        [ProtoMember(13)]
        public int TitleCodeId { get; set; }
        [ProtoMember(14)]
        public string LevelCode { get; set; }
        [ProtoMember(15)]
        public int StatusId { get; set; }
        [ProtoMember(16)]
        public string ManagerCode { get; set; }
        [ProtoMember(17)]
        public DateTime BirthDate { get; set; }
        [ProtoMember(18)]
        public DateTime JoinCompanyDate { get; set; }
        [ProtoMember(19)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(20)]
        public int CreatedBy { get; set; }
        [ProtoMember(21)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(22)]
        public int UpdatedBy { get; set; }
        [ProtoMember(23)]
        public string GeneralName { get; set; }
        [ProtoMember(24)]
        public string UnitCode { get; set; }
        [ProtoMember(25)]
        public string CentreCode { get; set; }
        [ProtoMember(26)]
        public string DepartmentCode { get; set; }
        [ProtoMember(27)]
        public string GroupCode { get; set; }
        [ProtoMember(28)]
        public string TitleCode { get; set; }
        [ProtoMember(29)]
        public string StatusName { get; set; }
        [ProtoMember(30)]
        public bool Visible { get; set; }
    }

    public class DvhcContract
    {
        public int AdministrativeUnitsId { get; set; }
        public string AdministrativeUnitsVN { get; set; }
        public string AdministrativeUnitsEN { get; set; }
        public Nullable<int> ParentId { get; set; }
        public int LevelNo { get; set; }
        public string LevelPath { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string Prefix { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

    }

    public class PrefixContract
    {
        [ProtoMember(1)]
        public int PrefixId { get; set; }
        [ProtoMember(2)]
        public string PrefixName { get; set; }
        [ProtoMember(3)]
        public int PrefixGroup { get; set; }
    }

    public class PnLBuAttributeGroupContract
    {
        [ProtoMember(1)]
        public int PnLBuAttributeGroupId { get; set; }
        [ProtoMember(2)]
        public string PnLBuAttributeGroupCode { get; set; }
        [ProtoMember(3)]
        public string PnLBuAttributeGroupName { get; set; }
        [ProtoMember(4)]
        public string Description { get; set; }
        [ProtoMember(5)]
        public bool Visible { get; set; }
        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(7)]
        public int CreatedBy { get; set; }
        [ProtoMember(8)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(9)]
        public int UpdatedBy { get; set; }
    }

    public class PnLBuAttributeContract
    {
        [ProtoMember(1)]
        public int PnLBuAttributeId { get; set; }
        [ProtoMember(2)]
        public string PnLBuAttributeCode { get; set; }
        [ProtoMember(3)]
        public string PnLBuAttributeName { get; set; }
        [ProtoMember(4)]
        public string Description { get; set; }
        [ProtoMember(5)]
        public bool Visible { get; set; }
        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(7)]
        public int CreatedBy { get; set; }
        [ProtoMember(8)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(9)]
        public int UpdatedBy { get; set; }
        [ProtoMember(10)]
        public int PnLAttributeGroupId { get; set; }
        [ProtoMember(11)]
        public int PnLListId { get; set; }
        [ProtoMember(12)]
        public int PnLBUListId { get; set; }
        [ProtoMember(13)]
        public string PnLAttributeGroupName { get; set; }
        [ProtoMember(14)]
        public string PnLListName { get; set; }
        [ProtoMember(15)]
        public string PnLBUListName { get; set; }
    }

    public class RegionContract
    {
        [ProtoMember(1)]
        public int RegionId { get; set; }
        [ProtoMember(2)]
        public string RegionCode { get; set; }
        [ProtoMember(3)]
        public string RegionName { get; set; }
        [ProtoMember(4)]
        public string Description { get; set; }
        [ProtoMember(5)]
        public bool Visible { get; set; }
        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(7)]
        public int CreatedBy { get; set; }
        [ProtoMember(8)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(9)]
        public int UpdatedBy { get; set; }
    }
    public class DepartmentTitleContract
    {
        [ProtoMember(1)]
        public int DepartmentTitleId { get; set; }
        [ProtoMember(2)]
        public string DepartmentTitleCode { get; set; }
        [ProtoMember(3)]
        public string DepartmentTitleName { get; set; }
        [ProtoMember(4)]
        public string Description { get; set; }
        [ProtoMember(5)]
        public bool Visible { get; set; }
        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(7)]
        public int CreatedBy { get; set; }
        [ProtoMember(8)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(9)]
        public int UpdatedBy { get; set; }
    }
    public class LevelContract
    {
        [ProtoMember(1)]
        public int LevelId { get; set; }
        [ProtoMember(2)]
        public string LevelCode { get; set; }
        [ProtoMember(3)]
        public string LevelName { get; set; }
        [ProtoMember(4)]
        public string Description { get; set; }
        [ProtoMember(5)]
        public bool Visible { get; set; }
        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(7)]
        public int CreatedBy { get; set; }
        [ProtoMember(8)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(9)]
        public int UpdatedBy { get; set; }
    }
    public class BasisGroupContract
    {
        [ProtoMember(1)]
        public int BasisGroupId { get; set; }
        [ProtoMember(2)]
        public string BasisGroupCode { get; set; }
        [ProtoMember(3)]
        public string BasisGroupName { get; set; }
        [ProtoMember(4)]
        public string Address { get; set; }
        [ProtoMember(5)]
        public float Longitude { get; set; }
        [ProtoMember(6)]
        public float Latitude { get; set; }
        [ProtoMember(7)]
        public bool Visible { get; set; }
        [ProtoMember(8)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(9)]
        public int CreatedBy { get; set; }
        [ProtoMember(10)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(11)]
        public int UpdatedBy { get; set; }
    }

    public class BasisContract
    {
        [ProtoMember(1)]
        public int BasisId { get; set; }
        [ProtoMember(2)]
        public string BasisCode { get; set; }
        [ProtoMember(3)]
        public string BasisName { get; set; }
        [ProtoMember(4)]
        public int BasisGroupId { get; set; }
        [ProtoMember(5)]
        public int PnLListId { get; set; }
        [ProtoMember(6)]
        public int PnLBUListId { get; set; }
        [ProtoMember(7)]
        public string Description { get; set; }
        [ProtoMember(8)]
        public int CityId { get; set; }
        [ProtoMember(9)]
        public int DistrictId { get; set; }
        [ProtoMember(10)]
        public int WardId { get; set; }
        [ProtoMember(11)]
        public string RefCode { get; set; }
        [ProtoMember(12)]
        public int StatusId { get; set; }
        [ProtoMember(13)]
        public string FullName { get; set; }
        [ProtoMember(14)]
        public string StatusDescription { get; set; }
        [ProtoMember(15)]
        public DateTime OpeningDate { get; set; }
        [ProtoMember(16)]
        public float Longitude { get; set; }
        [ProtoMember(17)]
        public float Latitude { get; set; }
        [ProtoMember(18)]
        public string Address { get; set; }
        [ProtoMember(19)]
        public string Manager { get; set; }
        [ProtoMember(20)]
        public string ManagerPhone { get; set; }
        [ProtoMember(21)]
        public string SitePhone { get; set; }
        [ProtoMember(22)]
        public string SiteEmail { get; set; }
        [ProtoMember(23)]
        public string AreaManager { get; set; }
        [ProtoMember(24)]
        public string AreaManagerPhone { get; set; }
        [ProtoMember(25)]
        public string AreaManagerEmail { get; set; }
        [ProtoMember(26)]
        public int DepartmentId { get; set; }
        [ProtoMember(27)]
        public int StaffId { get; set; }
        [ProtoMember(28)]
        public bool Visible { get; set; }
        [ProtoMember(29)]
        public DateTime CreateDate { get; set; }
        [ProtoMember(30)]
        public int CreateBy { get; set; }
        [ProtoMember(31)]
        public DateTime UpdateDate { get; set; }
        [ProtoMember(32)]
        public int UpdateBy { get; set; }
        [ProtoMember(33)]
        public string PnLListName { get; set; }
        [ProtoMember(34)]
        public string PnLBuListName { get; set; }
        [ProtoMember(35)]
        public string StatusName { get; set; }
        [ProtoMember(36)]
        public string BasisGroupName { get; set; }
    }

    public class BasisStatusContract
    {
        [ProtoMember(1)]
        public int BasisStatusId { get; set; }
        [ProtoMember(2)]
        public string BasisStatusName { get; set; }
    }

    #region system checklist
    public class SearchSystemCheckList
    {
        [ProtoMember(1)]
        public List<SystemCheckListContract> Data { get; set; }
        [ProtoMember(21)]
        public int TotalRows { get; set; }
    }
    public class SystemCheckListContract
    {
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public Boolean Visible { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int Priority { get; set; }
        public string StateName { get; set; }
        public List<UserContract> Users{get;set;}
        //add more info
        public string Code { get; set; }
        public int PlId { get; set; }
        public string SystemNameEn { get; set; }
        public int CateId { get; set; }
        public int SubCateId { get; set; }
        public string FunctionOverview { get; set; }
        public string ProviderName { get; set; }
        public int OriginTypeId { get; set; }
        public string Platform { get; set; }
        public int AppTypeId { get; set; }
        public bool IsSAP { get; set; }
        public int State { get; set; }
        public string UrlSystem { get; set; }
        public int AuthenticationMethodId { get; set; }
        public int RankId { get; set; }
        public int RTOTypeId { get; set; }
        public int RPOTypeId { get; set; }
        public Nullable<int> RLO { get; set; }
        public int DRTypeId { get; set; }
        public Nullable<System.DateTime> LastDateDRTest { get; set; }
        public int ScStateId { get; set; }
        public string SME { get; set; }
        public string OwingBusinessUnit { get; set; }
        public string ITContact { get; set; }
        public int YearImplement { get; set; }
        public int StabilityId { get; set; }
        public int QuantityUserActive { get; set; }
        public int ConCurrentUser { get; set; }
        public string BusinessHour { get; set; }
        public string BusinessIssue { get; set; }
        public string TechIssue { get; set; }
        public string SystemMaintainTime { get; set; }
        public bool IsDevTest { get; set; }
        public string HostingLocation { get; set; }
        public bool IsReplace { get; set; }
        public int ReplaceBy { get; set; }
        public string DetailReplaceBy { get; set; }
        public bool IsRequirementSecurity { get; set; }
        public bool IsRequirementSecurityDesign { get; set; }
        public bool IsCheckCertification { get; set; }
        public bool IsCheckSecurityByGolive { get; set; }
        public bool IsCheckRisk { get; set; }
        public int SecurityStateId { get; set; }
        public int PerformanceId { get; set; }
        public string PerformanceNote { get; set; }
        public string PLName { get; set; }
        public string CateName { get; set; }
        public string SubCateName { get; set; }        
    }

    #endregion
}
