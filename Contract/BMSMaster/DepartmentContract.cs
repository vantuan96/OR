using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.BMSMaster
{
    public class DepartmentContract
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string HeadofDepartment { get; set; }
        public string Code { get; set; }
        public bool IsShowHotline { get; set; }
        public bool IsShowInventory { get; set; }
        public bool IsShowOther { get; set; }
    }
}
