using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.MasterData
{
    public class UpdateItemContract
    {
        public int ClItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
        public string Comment { get; set; }
        public int Sort { get; set; }
        public long InstanceId { get; set; }
        public int SystemId { get; set; }
    }
}
