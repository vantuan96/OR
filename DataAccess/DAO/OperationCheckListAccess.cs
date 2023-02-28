

using Contract.MasterData;
using DataAccess.Models;
using System.Linq;
using System;
using Contract.Shared;
using System.Collections.Generic;
using Contract.OperationCheckList;
using Contract.Log;
using VG.Common;

namespace DataAccess.DAO
{

    public interface IOperationCheckListAccess{
       
        #region operation checklist

        /// <summary>
        /// Tra cứu check list
        /// </summary>
        /// <param name="state"></param>
        /// <param name="systemId"></param>
        /// <param name="checkListTypeId"></param>
        /// <param name="kw"></param>
        /// <returns></returns>
        IQueryable<CheckListOperation> ListOperationCheckList(int state, int systemId, int checkListTypeId, long InstanceId, string kw,int userId,Boolean IsPermissionAprove,Boolean IsMonth);
        CUDReturnMessage SaveOperationCheckListDetail(List<UpdateItemContract> listClItems, int userId);
        CUDReturnMessage ChangeStateOperationCheckList(UpdateOperationCheckListContract info, int userId);


        #endregion

    }

    public class OperationCheckListAccess : BaseDataAccess, IOperationCheckListAccess
    {
        public OperationCheckListAccess(string appid, int uid, string connStr = "ConnString.WebPortal") : base(appid, uid, connStr)
        {

        }
        public IQueryable<CheckListOperation> ListOperationCheckList(int state, int systemId, int checkListTypeId, long InstanceId, string kw, int userId, Boolean IsPermissionAprove, Boolean IsMonth)
        {
            
            var query = DbContext.CheckListOperations.AsQueryable();
            if (InstanceId > 0)
                return query = query.Where(c => c.InstanceId == InstanceId);
            if (state > 0)
                query = query.Where(c => c.CheckListStatusId==state);
            if (!string.IsNullOrEmpty(kw))
                query = query.Where(c => c.CheckListName.Contains(kw));
            if (systemId > 0)
                query = query.Where(c => c.SystemId == systemId);
            if (checkListTypeId > 0)
                query = query.Where(c => c.CheckListTypeId == checkListTypeId);
            if (IsPermissionAprove && userId>0)
            {
                var userIds = DbContext.AdminUsers.Where(c => c.LineManagerId == userId && c.IsActive==true).Select(c => c.UId).ToList().Distinct();
                query = query.Where(c => c.SystemCheckList.AdminUser_System.Any(d => userIds.Contains(d.UId)));
            }
            else if (userId > 0)
                query = query.Where(c => c.SystemCheckList.AdminUser_System.Any(d => d.UId == userId));
            if (IsMonth)
            {
                DateTime dt = DateTime.Now;
                DateTime firstDayOfMonth = Extension.ToFirstDayOfMonth(dt);
                DateTime lastDayOfMonth = Extension.ToLastDayOfMonth(dt);
                query = query.Where(c => c.CreatedDate >= firstDayOfMonth && c.CreatedDate <= lastDayOfMonth);
            }
            
            return query;
        }
        /// <summary>
        /// Cập nhật trạng thái hoàn thành
        /// </summary>
        /// <param name="listClItems"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage SaveOperationCheckListDetail(List<UpdateItemContract> listClItems,int userId)
        {
            try
            {
                if (listClItems == null || !listClItems.Any()) return new CUDReturnMessage(ResponseCode.Error);
                var operationChecklist = listClItems.FirstOrDefault();
                long InstaceId = operationChecklist.InstanceId;
                var itemMaps = DbContext.CheckListOperationMappings.Where(r => r.InstanceId == InstaceId);
                foreach (var item in itemMaps)
                {
                    var updateItem = listClItems.FirstOrDefault(c => c.InstanceId == item.InstanceId && c.ClItemId == item.CLItemId);
                    if (updateItem != null)
                    {
                        item.Comment = updateItem.Comment;
                        item.State = updateItem.State;
                        item.UpdatedBy = userId;
                        item.UpdatedDate = DateTime.Now;
                    }
                }

                #region log
                DbContext.LogObjects.Add(new LogObject()
                {
                    ObjectId = InstaceId,
                    OldState = operationChecklist.State,
                    NewState = operationChecklist.State,
                    ObjectTypeId = (int)ObjectTypeEnum.CheckList,
                    ActionId = (int)ActionTypeEnum.FinishItems,
                    CreatedBy = userId,
                    NewInformation = string.Join(",",listClItems.Select(c => (c.ItemName +":"+c.State)).ToList())
                });
                #endregion

                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.OperationCheckList_UpdateSuccess);
            }
            catch(Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.Error,ex.Message);
            }
           
        }
        public CUDReturnMessage ChangeStateOperationCheckList(UpdateOperationCheckListContract info,int userId)
        {
            try
            {
                
                if (info==null) return new CUDReturnMessage(ResponseCode.Error);
                var checklist = DbContext.CheckListOperations.Find(info.InstanceId);
                if(checklist==null)
                    return new CUDReturnMessage(ResponseCode.OperationCheckList_NoExists);
                int oldState = checklist.CheckListStatusId;

                checklist.CheckListStatusId = info.State;
                checklist.UpdatedBy = userId;
                checklist.UpdatedDate = DateTime.Now;
                checklist.Description = info.Comment;

                #region log
                DbContext.LogObjects.Add(new LogObject()
                {
                    ObjectId = checklist.InstanceId,
                    OldState = oldState,
                    NewState = checklist.CheckListStatusId,
                    ObjectTypeId = (int)ObjectTypeEnum.CheckList,
                    ActionId = (int)ActionTypeEnum.ChangeStateCheckList,
                    CreatedBy = userId,
                    NewInformation = "Thay đổi trạng thái checklist"
                });
                #endregion

                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.OperationCheckList_ChangeStatus);
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.Error, ex.Message);
            }
        }
    }
}
