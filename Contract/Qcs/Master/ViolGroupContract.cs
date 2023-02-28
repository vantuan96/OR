using ProtoBuf;

namespace BMS.Contract.Qcs.Master
{
    [ProtoContract]
    public class ViolGroupContract
    {
        /// <summary>
        /// Mã nhóm lỗi
        /// </summary>
        [ProtoMember(1)]
        public int ViolGroupId { get; set; }
        /// <summary>
        /// Tên nhóm lỗi
        /// </summary>
        [ProtoMember(2)]
        public string ViolGroupName { get; set; }
        /// <summary>
        /// Mã code nhóm lỗi
        /// </summary>
        [ProtoMember(3)]
        public string ViolGroupCode { get; set; }
    }
}
