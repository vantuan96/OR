using Admin.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Models.Master
{
    public class InsertUpdatetSystemModel
    {
       
        public int SystemId { get; set; }
        [Display(Name = "Master_SystemCheckList_SystemName", ResourceType = typeof(LayoutResource))]
        [Required]
        public string SystemName { get; set; }
        [Display(Name = "Master_SystemCheckList_Description", ResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        [Display(Name = "Master_SystemCheckList_State", ResourceType = typeof(LayoutResource))]
        [Required]
        public int State { get; set; }
        public int Priority { get; set; }
        //add info
        public List<SelectListItem> listStates { get; set; }
        public List<SelectListItem> listCates { get; set; }
        public List<SelectListItem> listSubCates { get; set; }
        public List<SelectListItem> listOrginTypes { get; set; }
        public List<SelectListItem> listAppTypes { get; set; }
        public List<SelectListItem> listStatus { get; set; }
        public List<SelectListItem> listAuthenticationMethods { get; set; }
        public List<SelectListItem> listRanks { get; set; }
        public List<SelectListItem> listRTOTypes { get; set; }
        public List<SelectListItem> listRPOTypes { get; set; }
        public List<SelectListItem> listDRs { get; set; }
        public List<SelectListItem> listServiceContinutys { get; set; }
        public List<SelectListItem> listStabilitys { get; set; }
        public List<SelectListItem> listSystemTypes { get; set; }
        public List<SelectListItem> listSecuritys { get; set; }
        public List<SelectListItem> listPerformances { get; set; }
        public List<SelectListItem> listPLs { get; set; }
        [Display(Name = "Master_SystemCheckList_Code", ResourceType = typeof(LayoutResource))]
        [Required]
        public string Code { get; set; }
        [Display(Name = "Master_SystemCheckList_PL", ResourceType = typeof(LayoutResource))]
        [Required]
        public int PlId { get; set; }
        [Display(Name = "Master_SystemCheckList_SystemNameEn", ResourceType = typeof(LayoutResource))]
        [Required]
        public string SystemNameEn { get; set; }
        [Display(Name = "Master_SystemCheckList_Cate", ResourceType = typeof(LayoutResource))]
        [Required]
        public int CateId { get; set; }
        [Display(Name = "Master_SystemCheckList_SubCate", ResourceType = typeof(LayoutResource))]
        [Required]
        public int SubCateId { get; set; }
        [Display(Name = "Master_SystemCheckList_Overview", ResourceType = typeof(LayoutResource))]
        public string FunctionOverview { get; set; }
        [Display(Name = "Master_SystemCheckList_ProviderName", ResourceType = typeof(LayoutResource))]
        public string ProviderName { get; set; }
        [Display(Name = "Master_SystemCheckList_OriginTypeId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int OriginTypeId { get; set; }
        [Display(Name = "Master_SystemCheckList_Platform", ResourceType = typeof(LayoutResource))]
        public string Platform { get; set; }
        [Display(Name = "Master_SystemCheckList_AppTypeId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int AppTypeId { get; set; }
        [Display(Name = "Master_SystemCheckList_IsSAP", ResourceType = typeof(LayoutResource))]
        [Required]
        public bool IsSAP { get; set; }
        [Display(Name = "Master_SystemCheckList_Status", ResourceType = typeof(LayoutResource))]
        [Required]
        public int Status { get; set; }
        [Display(Name = "Master_SystemCheckList_UrlSystem", ResourceType = typeof(LayoutResource))]
        public string UrlSystem { get; set; }
        [Display(Name = "Master_SystemCheckList_AuthenticationMethodId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int AuthenticationMethodId { get; set; }
        [Display(Name = "Master_SystemCheckList_RankId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int RankId { get; set; }
        [Display(Name = "Master_SystemCheckList_RTOTypeId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int RTOTypeId { get; set; }
        [Display(Name = "Master_SystemCheckList_RPOTypeId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int RPOTypeId { get; set; }
        [Display(Name = "Master_SystemCheckList_RLO", ResourceType = typeof(LayoutResource))]
        [Required]
        public int RLO { get; set; }
        [Display(Name = "Master_SystemCheckList_DRTypeId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int DRTypeId { get; set; }
        
        public DateTime LastDateDRTest { get; set; }
        [Display(Name = "Master_SystemCheckList_LastDateDRTest", ResourceType = typeof(LayoutResource))]
        [Required]
        public string showLastDateDRTest
        {
            get
            {
                return LastDateDRTest.ToVEShortDate() + " " + LastDateDRTest.ToVEShortTime();
            }
            set { }
        }

        [Display(Name = "Master_SystemCheckList_ScStateId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int ScStateId { get; set; }
        [Display(Name = "Master_SystemCheckList_SME", ResourceType = typeof(LayoutResource))]
        public string SME { get; set; }
        [Display(Name = "Master_SystemCheckList_OwingBusinessUnit", ResourceType = typeof(LayoutResource))]
        public string OwingBusinessUnit { get; set; }
        [Display(Name = "Master_SystemCheckList_ITContact", ResourceType = typeof(LayoutResource))]
        public string ITContact { get; set; }
        [Display(Name = "Master_SystemCheckList_YearImplement", ResourceType = typeof(LayoutResource))]
        public int YearImplement { get; set; }
        [Display(Name = "Master_SystemCheckList_StabilityId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int StabilityId { get; set; }
        [Display(Name = "Master_SystemCheckList_QuantityUserActive", ResourceType = typeof(LayoutResource))]
        public int QuantityUserActive { get; set; }
        [Display(Name = "Master_SystemCheckList_ConCurrentUser", ResourceType = typeof(LayoutResource))]
        public int ConCurrentUser { get; set; }
        [Display(Name = "Master_SystemCheckList_BusinessHour", ResourceType = typeof(LayoutResource))]
        public string BusinessHour { get; set; }
        [Display(Name = "Master_SystemCheckList_BusinessIssue", ResourceType = typeof(LayoutResource))]
        public string BusinessIssue { get; set; }
        [Display(Name = "Master_SystemCheckList_TechIssue", ResourceType = typeof(LayoutResource))]
        public string TechIssue { get; set; }
        [Display(Name = "Master_SystemCheckList_SystemMaintainTime", ResourceType = typeof(LayoutResource))]
        public string SystemMaintainTime { get; set; }
        [Display(Name = "Master_SystemCheckList_IsDevTest", ResourceType = typeof(LayoutResource))]
        public bool IsDevTest { get; set; }
        [Display(Name = "Master_SystemCheckList_HostingLocation", ResourceType = typeof(LayoutResource))]
        public string HostingLocation { get; set; }
        [Display(Name = "Master_SystemCheckList_IsReplace", ResourceType = typeof(LayoutResource))]
        public bool IsReplace { get; set; }
        [Display(Name = "Master_SystemCheckList_ReplaceBy", ResourceType = typeof(LayoutResource))]
        public int ReplaceBy { get; set; }
        [Display(Name = "Master_SystemCheckList_DetailReplaceBy", ResourceType = typeof(LayoutResource))]
        public string DetailReplaceBy { get; set; }
        [Display(Name = "Master_SystemCheckList_IsRequirementSecurity", ResourceType = typeof(LayoutResource))]
        [Required]
        public bool IsRequirementSecurity { get; set; }
        [Display(Name = "Master_SystemCheckList_IsRequirementSecurityDesign", ResourceType = typeof(LayoutResource))]
        [Required]
        public bool IsRequirementSecurityDesign { get; set; }
        [Display(Name = "Master_SystemCheckList_IsCheckCertification", ResourceType = typeof(LayoutResource))]
        [Required]
        public bool IsCheckCertification { get; set; }
        [Display(Name = "Master_SystemCheckList_IsCheckSecurityByGolive", ResourceType = typeof(LayoutResource))]
        [Required]
        public bool IsCheckSecurityByGolive { get; set; }
        [Display(Name = "Master_SystemCheckList_IsCheckRisk", ResourceType = typeof(LayoutResource))]
        [Required]
        public bool IsCheckRisk { get; set; }
        [Display(Name = "Master_SystemCheckList_SecurityStateId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int SecurityStateId { get; set; }
        [Display(Name = "Master_SystemCheckList_PerformanceId", ResourceType = typeof(LayoutResource))]
        [Required]
        public int PerformanceId { get; set; }
        [Display(Name = "Master_SystemCheckList_PerformanceNote", ResourceType = typeof(LayoutResource))]
        public string PerformanceNote { get; set; }





    }
}
