using VG.Common;
using Admin.Resource;
using Contract.Shared;

namespace Admin.Models.Shared
{
    public class ActionMessage
    {
        public ActionMessage(int id, string msg)
        {
            this.ID = id;
            this.Message = StringUtil.GetResourceString(typeof(MessageResource), msg);
        }

        public ActionMessage(CUDReturnMessage cudObj)
        {
            this.ID = cudObj.Id;
            this.Message = StringUtil.GetResourceString(typeof(MessageResource), cudObj.SystemMessage);
        }

        public int ID { get; set; }

        public string Message { get; set; }
    }
}