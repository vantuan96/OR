using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.BMSMaster
{
    public class PlanSiteContract
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string PathName { get; set; }

        public Nullable<int> PlanPnLID { get; set; }
        public string PlanPnLName { get; set; }
        public Nullable<bool> Inactive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
