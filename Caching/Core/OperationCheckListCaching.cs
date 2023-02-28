using Business.Core;
using Contract.MasterData;
using Contract.OperationCheckList;
using Contract.Report;
using Contract.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching.Core
{
    public interface IOperationCheckListCaching
    {
        SearchOperationCheckList ListCheckList(int state, int systemId, int checkListTypeId, long InstanceId, string kw, int p, int ps, int userId, Boolean IsPermissionAprove);
        CUDReturnMessage SaveOperationCheckListDetail(List<UpdateItemContract> listClItems, int userId);
        CUDReturnMessage ChangeStateOperationCheckList(UpdateOperationCheckListContract info, int userId);
        

    }
    public class OperationCheckListCaching : BaseCaching, IOperationCheckListCaching
    {

        private Lazy<IOperationCheckListBusiness> lazy;

        public OperationCheckListCaching()
        {
            lazy = new Lazy<IOperationCheckListBusiness>(() => new OperationCheckListBusiness(appid, uid));
        }

        public CUDReturnMessage ChangeStateOperationCheckList(UpdateOperationCheckListContract info, int userId)
        {
            return lazy.Value.ChangeStateOperationCheckList(info, userId);
        }
        public SearchOperationCheckList ListCheckList(int state, int systemId, int checkListTypeId, long InstanceId, string kw, int p, int ps, int userId, Boolean IsPermissionAprove)
        {
            return lazy.Value.ListCheckList(state, systemId, checkListTypeId, InstanceId, kw, p, ps, userId,IsPermissionAprove);
        }
        public CUDReturnMessage SaveOperationCheckListDetail(List<UpdateItemContract> listClItems, int userId)
        {
            return lazy.Value.SaveOperationCheckListDetail(listClItems, userId);
        }
    }
}
