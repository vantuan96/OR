using ProtoBuf;
using System;
using VG.Common;
using Contract.Microsite;
using Contract.Shared;

namespace Contract.User
{
    [ProtoContract]
    public class LoginContract
    {
        [ProtoMember(1)]
        public int UserId { set; get; }

        [ProtoMember(2)]
        public string FullName { set; get; }

        [ProtoMember(3)]
        public string NickName { set; get; }

        [ProtoMember(4)]
        public string Email { set; get; }

        [ProtoMember(5)]
        public ResponseCode ResCode { get; set; }

        [ProtoMember(6)]
        public MicrositeItemContract CurrentMicrosite { get; set; }

        [ProtoMember(7)]
        public string CompanyName { set; get; }

        public LoginContract()
        {
            this.ResCode = ResponseCode.Successed;
        }

        public LoginContract(ResponseCode resCode)
        {
            this.ResCode = resCode;
        }

        public string ErrorMessage(Type typeofResouce)
        {
            string systemMessage = ResCode.GetDescription();
            return StringUtil.GetResourceString(typeofResouce, systemMessage);
        }
        public int CurrentLoginFail { get; set; } = 0;
        public string ClientIp { get; set; }
        public string TokenKey { get; set; }
        public bool IsLogon { get; set; }
        public bool IsViewPublic { get; set; }
    }
}
