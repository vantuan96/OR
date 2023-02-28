using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.Master
{
    [ProtoContract]
    public class ViolResolveTypeContract
    {
        /// <summary>
        /// Mã hình thức xử lý
        /// </summary>
        [ProtoMember(1)]
        public int ResolveTypeId { get; set; }
        /// <summary>
        /// Tên hình thức xử lý
        /// </summary>
        [ProtoMember(2)]
        public string ResolveTypeName { get; set; }
        /// <summary>
        /// Mã code hình thức xử lý
        /// </summary>
        [ProtoMember(3)]
        public string ResolveTypeCode { get; set; }
    }
}
