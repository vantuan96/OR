using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class SearchORRoom
    {
        [ProtoMember(1)]
        public List<ORRoomContract> Data { get; set; }
        [ProtoMember(2)]
        public int TotalRows { get; set; }
    }
}
