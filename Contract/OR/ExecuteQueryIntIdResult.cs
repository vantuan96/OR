using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    [ProtoContract]
    public class ExecuteQueryIntIdResult
    {
        [ProtoMember(1)]
        public bool IsSuccess { get; set; }

        [ProtoMember(2)]
        public string Message { get; set; }
        [ProtoMember(3)]
        public int Identity { get; set; }
    }
}
