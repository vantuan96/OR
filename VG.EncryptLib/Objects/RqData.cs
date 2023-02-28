using VG.EncryptLib.Shared;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VG.EncryptLib.Objects
{
    [ProtoContract]
    public class RqData
    {
        public string SaltKey { get; set; }
        public string AppSecret { get; set; }
        public string AppId { get; set; }
        public int UserId { get; set; }
        public string StationId { get; set; }
        public bool UserSlaveDB { get; set; }
        public Dictionary<string, string> RequestQueryParam { get; set; }

        /// <summary>
        /// Giải mã và lấy đối tượng request từ client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetParameters<T>()
        {
            return SerializerObject.ProtoBufDeserialize<T>(Parameters, SaltKey);//SaltKey
        }
        [ProtoMember(2)]
        public byte[] Parameters { get; set; }
    }
}
