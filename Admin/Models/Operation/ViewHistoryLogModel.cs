using Admin.Helper;
using Admin.Resource;
using Contract.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models.Operation
{
    public class ViewHistoryLogModel
    {
        public long InstanceId { get; set; }
        public int CheckListId { get; set; }
        [Required]
        [Display(Name = "Master_CheckList_Label_Title_Code", ResourceType = typeof(LayoutResource))]
        [StringLength(512)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        public string CheckListName { get; set; }
        [StringLength(500)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg, ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar", ErrorMessageResourceType = typeof(LayoutResource))]
        [Display(Name = "OperationCheckList_Lable_Description", ResourceType = typeof(LayoutResource))]
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public List<UpdateItemModel> listClItems { get; set; }
        [Required]
        [Display(Name = "CheckList_Lable_State", ResourceType = typeof(LayoutResource))]
        public int State { get; set; }
        [Required]
        [Display(Name = "CheckList_Lable_CheckListType", ResourceType = typeof(LayoutResource))]
        public int CheckListTypeId { get; set; }
        public string CheckListTypeName { get; set; }
        [Required]
        [Display(Name = "CheckList_Lable_System", ResourceType = typeof(LayoutResource))]
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public DateTime DeadLine { get; set; }
        public List<LogObjectContract> LogChange { get; set; }
    }
}
