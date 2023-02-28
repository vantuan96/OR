using Contract.CheckList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OperationCheckList
{

    public partial class OperationCheckListContract
    {


        public long InstanceId { get; set; }
        public int SystemId { get; set; }
        public int CheckListId { get; set; }
        public int CheckListTypeId { get; set; }
        public string SystemName { get; set; }
        public string CheckListName { get; set; }
        public string CheckListTypeName { get; set; }
        public int CheckListStatusId { get; set; }
        public string CheckListStatusName { get; set; }
        public DateTime SetupDateFrom { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string StateName { get; set; }
        public List<int> lstItemIds { get;set;} 
        public List<OperationItemContract> Items { get; set; }
        public List<UserContract> Users { get; set; }
        public string OwnerEmail { get; set; }

    }
}
