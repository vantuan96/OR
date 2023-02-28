using Contract.Const;
using ProtoBuf;
using VG.Common;

namespace Contract.Shared
{
    [ProtoContract]
    public class CUDReturnMessage
    {
        public CUDReturnMessage()
        {
            Id = 0;
            Message = "";
        }

        public CUDReturnMessage(ErrorCode errCode)
        {
            this.Id = (int)errCode;
            this.Message = ErrorMessage.Get(this.Id);
            this.SystemMessage = this.Message;
        }

        public CUDReturnMessage(ResponseCode resCode)
        {
            this.Id = (int)resCode;
            this.SystemMessage = resCode.GetDescription();
        }
        public CUDReturnMessage(ResponseCode resCode, string sysMsg)
        {
            this.Id = (int)resCode;
            this.Message = resCode.GetDescription();
            this.SystemMessage = sysMsg;
        }

        public CUDReturnMessage(int id, string msg, string sysMsg)
        {
            this.Id = id;
            this.Message = msg;
            this.SystemMessage = sysMsg;
        }
        
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(4)]
        private string _message;

        [ProtoMember(2)]
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value.Trim();
            }
        }

        [ProtoMember(3)]
        public string SystemMessage { get; set; }

        /// <summary>
        /// Chứa Id được cập nhật hoặc thêm mới
        /// </summary>
        [ProtoMember(5)]
        public string ReturnData { get; set; }
    }
}
