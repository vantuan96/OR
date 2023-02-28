using ProtoBuf;

namespace VG.EncryptLib.Objects
{
    [ProtoContract]
    public enum ResponseStatus
    {
        Successed = 200,
        InvalidUser = 401,
        InvalidSession = 403,
        Error = 500        
    }

    [ProtoContract]
    public class ResponseObject
    {
        public ResponseObject()
        {
            Data = null;
        }

        /// <summary>
        /// Response status code: Base on http return code
        /// </summary>
        [ProtoMember(1)]
        public int Status { get; set; }

        /// <summary>
        /// Response data object
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Response data in byte[]
        /// </summary>
        [ProtoMember(2)]
        public byte[] ResponseData { get; set; }
        
        /// <summary>
        /// Response message
        /// </summary>
        [ProtoMember(3)]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// SaltKey
        /// </summary>
        [ProtoMember(4)]
        public string SaltKey
        {
            get;
            set;
        }
    }
}