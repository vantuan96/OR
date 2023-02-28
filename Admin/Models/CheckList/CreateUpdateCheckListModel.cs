using System;
using System.ComponentModel.DataAnnotations;
using Admin.Resource;
using System.Collections.Generic;
using System.Web.Mvc;
using Admin.Helper;
using Contract.CheckList;
using VG.Common;

namespace Admin.Models.CheckList
{
    public class CreateUpdateCheckListModel
    {
        public int CheckListId { get; set; }
        [Required]
        [Display(Name = "Master_CheckList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(512)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string CheckListName { get; set; }
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int Priority { get; set; }
        public string CreateName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        public List<SelectListItem> listStates { get; set; }
        public List<SelectListItem> listCheckListTypes { get; set; }
        public List<SelectListItem> listSystems { get; set; }
        public List<SelectListItem> listClItems { get; set; }

        [Required]
        [Display(Name = "CheckList_Lable_State", ResourceType = typeof(LayoutResource))]
        public int State { get; set; }
        [Required]
        [Display(Name = "CheckList_Lable_CheckListType", ResourceType = typeof(LayoutResource))]
        public int CheckListTypeId { get; set; }
        [Required]
        [Display(Name = "CheckList_Lable_System", ResourceType = typeof(LayoutResource))]
        public int SystemId { get; set; }
        [Required]
        [Display(Name = "CheckList_Lable_Item", ResourceType = typeof(LayoutResource))]
        public List <int> CLItemIds { get; set; }

        public DateTime SetupDateFrom { get; set; }
        [Display(Name = "CheckList_Label_SetupDateFrom", ResourceType = typeof(LayoutResource))]
        public string showSetupDateFrom
        {
            get
            {
                return SetupDateFrom.ToVEShortDate() + " " + SetupDateFrom.ToVEShortTime();
            }
            set { }
        }
        
    }

    public class ListCheckListModel
    {
        public int CheckListId { get; set; }
        public string CheckListName { get; set; }
        public string Description { get; set; }
    }

    public class CreateUpdateCheckListDetailModel
    {
        public int Id { get; set; }
        public int CheckListDetailId { get; set; }
        [Required]
        [Display(Name = "Master_CheckListDetail_Label_Title", ResourceType = typeof(LayoutResource))]
        [StringLength(250)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string CheckListDetailName { get; set; }

        public string Description { get; set; }
        public int CheckListId { get; set; }
        public bool Visible { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int Priority { get; set; }
        public List<CheckListContract> ListCheckList { get; set; }
    }
};