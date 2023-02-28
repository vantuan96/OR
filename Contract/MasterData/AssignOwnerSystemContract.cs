using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.MasterData
{
    public class AssignOwnerSystemContract
    {
        public int SystemId { get; set; }
        public List<int> UIds { get; set; }
        public int TypeRule { get; set; }
    }
}
