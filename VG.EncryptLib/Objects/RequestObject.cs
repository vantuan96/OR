using ProtoBuf;
using System.Collections.Generic;

namespace VG.EncryptLib.Objects
{
    [ProtoContract]
    public class RequestObject<T> where T : class
    {
        public RequestObject()
        {
            Params = new Dictionary<string, string>();
        }
        /// <summary>
        /// List params
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<string, string> Params { get; set; }
        /// <summary>
        /// Request data object
        /// </summary>
        [ProtoMember(2)]
        public T Data { get; set; }
    }
}