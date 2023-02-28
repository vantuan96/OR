using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class SearchORProgress
    {
        [ProtoMember(1)]
        public List<ORAnesthProgressContract> Data { get; set; }
        [ProtoMember(121)]
        public int TotalRows { get; set; }
    }
}
