using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OperationCheckList
{
    public class UpdateOperationCheckListContract
    {
        public long InstanceId { get; set; }
        public string Comment { get; set; }
        public int State { get; set; }
        public string Description { get; set; }
    }
}
