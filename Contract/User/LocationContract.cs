using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.User
{
    public class LocationContract
    {
        public int LocationId { get; set; }
        public string NameVN { get; set; }
        public string NameEN { get; set; }
        public string SloganVN { get; set; }
        public string SloganEN { get; set; }
        public string LogoName { get; set; }
        public string BackgroundName { get; set; }
        public string ColorCode { get; set; }
        public Nullable<int> ParentId { get; set; }
        public int LevelNo { get; set; }
        public string LevelPath { get; set; }
        public Nullable<int> RootId { get; set; }
        public Nullable<int> LayoutTypeId { get; set; }
        public string LayoutTypeName { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> QuestionGroupId { get; set; }
        public string QuestionGroupName { get; set; }

        /// <summary>
        /// Version bộ câu hỏi
        /// </summary>
        public int QuestionGroupVersion { get; set; }
        [DefaultValue("2")]
        public int SourceClientId { get; set; }


        public string ORUnitName { get; set; }
        public string ORUnitEmail { get; set; }
    }

    public class DepartmentTypeV01
    {
        public int DepartmentTypeId { get; set; }
        public string DepartmentTypeName { get; set; }
    }
}
