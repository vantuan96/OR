using Contract.Const;

namespace Contract
{
    public class ResponseContract<T>
    {
        public bool IsSuccess { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string SystemMessage { get; set; }

        public T Data { get; set; }

        public ResponseContract()
        {
        }

        public ResponseContract(T data)
        {
            if (data == null)
            {
                this.IsSuccess = false;
                this.ErrorCode = 0;
                this.ErrorMessage = "";
                this.SystemMessage = "";
                this.Data = default(T);
            }
            else
            {
                this.IsSuccess = true;
                this.ErrorCode = 0;
                this.ErrorMessage = "";
                this.SystemMessage = "";
                this.Data = data;
            }
        }

        public ResponseContract(ErrorCode errorCode = Const.ErrorCode.Unknow)
        {
            this.IsSuccess = false;
            this.ErrorCode = (int)errorCode;
            this.ErrorMessage = Const.ErrorMessage.Get(this.ErrorCode);
            this.SystemMessage = "";
            this.Data = default(T);
        }
    }
}
