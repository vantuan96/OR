using System.Collections.Generic;

namespace Contract.Const
{
    public class ErrorMessage
    {
        private static Dictionary<int, string> dicMessage = new Dictionary<int, string>
        {
            { (int)ErrorCode.Unknow, "Lỗi không xác định" },
            { (int)ErrorCode.LoginFail, "Thông tin đăng nhập không đúng" },
            { (int)ErrorCode.AccessTokenInvalid, "Thiết bị chưa được đăng ký" },
            { (int)ErrorCode.Answer_InvalidLayoutType, "Thông tin không đúng với biểu mẫu đã đăng ký của thiết bị" },
            { (int)ErrorCode.Answer_SaveSuccess, "Thành công" },
        };

        public static string Get(int code)
        {
            if (dicMessage.ContainsKey(code))
                return dicMessage[code];

            return "";
        }
    }
}
