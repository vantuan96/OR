using Contract.Enum;
using Contract.MasterData;
using Contract.OperationCheckList;
using Contract.Shared;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VG.Common;

namespace Business.Core
{
    public interface IOperationCheckListBusiness
    {
        SearchOperationCheckList ListCheckList(int state, int systemId, int checkListTypeId, long InstanceId, string kw, int p, int ps,int userId, Boolean IsPermissionAprove);
        CUDReturnMessage SaveOperationCheckListDetail(List<UpdateItemContract> listClItems, int userId);
        CUDReturnMessage ChangeStateOperationCheckList(UpdateOperationCheckListContract info, int userId);
    }
    public class OperationCheckListBusiness : BaseBusiness, IOperationCheckListBusiness
    {
        private Lazy<IOperationCheckListAccess> lazyOperationCheckListAccess;

       
        public OperationCheckListBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyOperationCheckListAccess = new Lazy<IOperationCheckListAccess>(() => new OperationCheckListAccess(appid, uid));

        }

        public CUDReturnMessage ChangeStateOperationCheckList(UpdateOperationCheckListContract info, int userId)
        {
            return lazyOperationCheckListAccess.Value.ChangeStateOperationCheckList(info, userId);
        }

        public SearchOperationCheckList ListCheckList(int state, int systemId, int checkListTypeId, long InstanceId, string kw, int p, int ps,int userId, Boolean IsPermissionAprove)
        {
            SearchOperationCheckList result = new SearchOperationCheckList() { Data = new List<OperationCheckListContract>(), TotalRows = 0 };
            var query = lazyOperationCheckListAccess.Value.ListOperationCheckList(state, systemId, checkListTypeId, InstanceId, kw, userId,IsPermissionAprove,false);

            if (query != null && query.Any())
            {
                result.TotalRows = query.Count();
                var data = query.OrderByDescending(c => c.CreatedBy)
                            .ThenByDescending(c => c.CheckListName)
                            .Skip((p - 1) * ps)
                            .Take(ps).ToList();
                result.Data = data.Select(c => new OperationCheckListContract()
                {
                    InstanceId=c.InstanceId,
                    CheckListId = c.CheckListId,
                    CheckListName = c.CheckListName,
                    Description = c.Description,
                    CheckListStatusId = c.CheckListStatusId,
                    CreatedBy = c.CreatedBy ?? 0,
                    CreatedDate = c.CreatedDate ?? DateTime.Now,
                    UpdatedBy = c.UpdatedBy ?? 0,
                    UpdatedDate = c.UpdatedDate ?? DateTime.Now,
                    CheckListTypeId = c.CheckListTypeId,
                    StateName = EnumExtension.GetDescription((CheckListStateEnum)c.CheckListStatusId),
                    SystemId = c.SystemId,
                    SystemName = c.SystemCheckList.SystemName,
                    OwnerEmail = string.Join(",",c.SystemCheckList.AdminUser_System.Select( d =>d.AdminUser.Email).ToList()),
                    CheckListTypeName = EnumExtension.GetDescription((CheckListTypeEnum)c.CheckListTypeId),
                    lstItemIds = c.CheckListOperationMappings.Select(d => d.CLItemId).ToList(),
                    Items = c.CheckListOperationMappings.Select(d => new OperationItemContract() { ItemName = d.CheckListItem.ItemName, CLItemId = d.CheckListItem.CLItemId, State = d.State, Description = d.Description,Comment=d.Comment }).ToList(),
                    SetupDateFrom = c.SetupDateFrom,
                    Deadline=c.Deadline,
                    Users = c.SystemCheckList.AdminUser_System.Select(d => new Contract.CheckList.UserContract() { Email = d.AdminUser.Email, FullName = d.AdminUser.Name, UserName = d.AdminUser.Username }).ToList(),

                }).ToList();
            }
            return result;
        }

        public CUDReturnMessage SaveOperationCheckListDetail(List<UpdateItemContract> listClItems, int userId)
        {
            return lazyOperationCheckListAccess.Value.SaveOperationCheckListDetail(listClItems, userId);
        }
    }
}
