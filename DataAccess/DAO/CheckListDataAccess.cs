using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Shared;
using Contract.CheckList;
using DataAccess.Models;
using Contract.MasterData;
using Contract.Enum;
using VG.Common;
using Contract.Log;
using Contract.OperationCheckList;

namespace DataAccess.DAO
{
    public interface ICheckListDataAccess
    {
        #region Check list
        List<CheckList> GetListCheckList(int sort);

        CUDReturnMessage InsertUpdateCheckList(CheckListContract data, int userId);

        CUDReturnMessage ActiveCheckList(int CheckListId, int userId);
        List<AdminUser> GetListUser();
        #endregion

        #region Check list detail
        List<CheckListDetail> GetCheckListIitem();

        CUDReturnMessage InsertUpdateCheckListDetails(CheckListItemContract data, int userId);

        CUDReturnMessage DeleteCheckListItem(int CheckListDetailId, int userId);
        #endregion

        #region Check list map
        IQueryable<Models.CheckListMap> GetListCheckListMap();
        #endregion

        #region Item checklist
        /// <summary>
        /// Lấy ds Item check list
        /// </summary>
        /// <param name="state"></param>
        /// <param name="kw"></param>
        /// <returns></returns>
        IQueryable<CheckListItem> GetItem(int state = 0, string kw = "", int systemId = 0);
        /// <summary>
        /// Xoa item chi dinh
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteItem(int CLItemId, int userId);
        /// <summary>
        /// Tạo mới/ Cập nhật item
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateItem(ItemContract info, int userId);
        #endregion

        #region new checklist
        /// <summary>
        /// Tra cứu check list
        /// </summary>
        /// <param name="state"></param>
        /// <param name="systemId"></param>
        /// <param name="checkListTypeId"></param>
        /// <param name="kw"></param>
        /// <returns></returns>
        IQueryable<CheckList> ListCheckList(int state, int systemId, int checkListTypeId, int CheckListId, string kw);
        #endregion

        #region worker gen checklist operation

        List<OperationCheckListContract> GenCheckListByType(int quantityCheckList);
        /// <summary>
        /// Thuc thi block 
        /// </summary>
        /// <param name="blockFunction"></param>
        void ExecuteBlock(Action blockFunction);

        #endregion



    }

    public class CheckListDataAccess : BaseDataAccess, ICheckListDataAccess
    {
        private readonly Lazy<ILogObjectDataAccess> logAccess;
        #region Check list
        public CheckListDataAccess(string appid, int uid, string connStr = "ConnString.WebPortal") : base(appid, uid, connStr)
        {
            logAccess = new Lazy<ILogObjectDataAccess>(() => { return new LogObjectDataAccess(appid, uid); });
        }

        public List<CheckList> GetListCheckList(int sort)
        {
            var query = DbContext.CheckLists.Where(x => (bool)x.Visible).ToList();
            return query;
        }



        public CUDReturnMessage ActiveCheckList(int CheckListId, int userId)
        {
            var checklist = DbContext.CheckLists.FirstOrDefault(r => r.CheckListId == CheckListId);
            if (checklist == null || (checklist.Visible == true))
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            var systemCheckList = DbContext.SystemCheckLists.FirstOrDefault(c => c.SystemId == checklist.SystemId && c.State == (int)SystemStatusEnum.Active);
            if (systemCheckList == null)
                return new CUDReturnMessage(ResponseCode.SystemMngt_NoExists);
            if (systemCheckList.AdminUser_System == null || !systemCheckList.AdminUser_System.Any(c => c.IsDeleted == false))
                return new CUDReturnMessage(ResponseCode.CheckList_NotActiveWhenNoHavingOwner);

            checklist.Visible = true;
            checklist.UpdatedBy = userId;
            checklist.UpdatedDate = DateTime.Now;
            DbContext.SaveChanges();
           #region log
            logAccess.Value.AddLog(new Contract.Log.LogObjectContract()
            {
                ObjectId = checklist.CheckListId,
                OldState = 0,
                NewState = 0,
                ObjectTypeId =(int) ObjectTypeEnum.CheckList,
                ActionId=(int)ActionTypeEnum.Orther,
                CreatedBy=0,
                NewInformation="Active checklist để tạo kích hoạt gen"
            });
            #endregion

            return new CUDReturnMessage(ResponseCode.CheckList_ActiveCheckList);
        }

        public List<AdminUser> GetListUser()
        {
            var query = DbContext.AdminUsers.Where(x => x.IsActive).ToList();
            return query;
        }

        #endregion

        #region Check list details
        public List<CheckListDetail> GetCheckListIitem()
        {
            var query = DbContext.CheckListDetails.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdateCheckListDetails(CheckListItemContract data, int userId)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            if (data.CheckListDetailId == 0) //Innsert
            {
                var newChecklistD = new CheckListDetail()
                {
                    CheckListDetailName = data.CheckListDetailName,
                    Description = data.Description,
                    CheckListId = data.CheckListId,
                    Visible = data.Visible,
                    Priority = data.Priority,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = 0,
                    UpdatedDate = DateTime.Now
                };
                DbContext.CheckListDetails.Add(newChecklistD);
                DbContext.SaveChanges();


                #region log
                logAccess.Value.AddLog(new Contract.Log.LogObjectContract()
                {
                    ObjectId = newChecklistD.CheckListId,
                    OldState = 0,
                    NewState = 0,
                    ObjectTypeId = (int)ObjectTypeEnum.CheckList,
                    ActionId = (int)ActionTypeEnum.Orther,
                    CreatedBy = userId,
                    NewInformation = "thêm/ chỉnh sửa hạng mục cho checklist"
                });
                #endregion



                return new CUDReturnMessage(ResponseCode.CheckListDetail_SuccessCreate);
            }
            //Update
            var checkListD = DbContext.CheckListDetails.SingleOrDefault(r => r.CheckListDetailId == data.CheckListDetailId);
            if (checkListD == null)
                return new CUDReturnMessage(ResponseCode.Error);

            checkListD.CheckListDetailName = data.CheckListDetailName;
            checkListD.CheckListId = data.CheckListId;
            checkListD.Description = data.Description;
            checkListD.Visible = data.Visible;
            checkListD.Priority = data.Priority;
            checkListD.UpdatedBy = userId;
            checkListD.UpdatedDate = DateTime.Now;
            DbContext.SaveChanges();


            #region log
            logAccess.Value.AddLog(new Contract.Log.LogObjectContract()
            {
                ObjectId = checkListD.CheckListId,
                OldState = 0,
                NewState = 0,
                ObjectTypeId = (int)ObjectTypeEnum.CheckList,
                ActionId = (int)ActionTypeEnum.Orther,
                CreatedBy = userId,
                NewInformation = "thêm/ chỉnh sửa hạng mục cho checklist"
            });
            #endregion

            return new CUDReturnMessage(ResponseCode.CheckListDetail_SuccessUpdate);
        }

        public CUDReturnMessage DeleteCheckListItem(int CheckListDetailId, int userId)
        {
            var checklistDetail = DbContext.CheckListDetails.FirstOrDefault(r => r.CheckListDetailId == CheckListDetailId);
            if (checklistDetail == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            checklistDetail.Visible = false;
            checklistDetail.UpdatedBy = userId;
            checklistDetail.UpdatedDate = DateTime.Now;
            DbContext.SaveChanges();
                     


            return new CUDReturnMessage(ResponseCode.CheckListDetail_SuccessDelete);
        }
        #endregion

        #region Check list map
        public IQueryable<Models.CheckListMap> GetListCheckListMap()
        {
            var query = DbContext.CheckListMaps;
            return query;
        }
        #endregion

        #region Check list type
        public IQueryable<Models.CheckListType> GetListCheckListType()
        {
            var query = DbContext.CheckListTypes.Where(x => x.Visible == true);
            return query;
        }


        #endregion

        #region Item 
        /// <summary>
        /// Tim kiếm /tra cứu hạng mục <item>
        /// </summary>
        /// <param name="state"></param>
        /// <param name="kw"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public IQueryable<CheckListItem> GetItem(int state = 0, string kw = "", int clItemId = 0)
        {
            var query = DbContext.CheckListItems.AsQueryable();
            if (clItemId > 0)
                query = query.Where(c => c.CLItemId == clItemId);
            if (state > 0)
                query = query.Where(c => c.Visible == (state == (int)SystemStateEnum.Active));
            if (!string.IsNullOrEmpty(kw))
                query = query.Where(c => c.ItemName.Contains(kw));
            return query;
        }
        /// <summary>
        /// Xóa item danh mục
        /// </summary>
        /// <param name="CLItemId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteItem(int CLItemId, int userId)
        {
            var result = default(CUDReturnMessage);
            try
            {
                var system = DbContext.CheckListItems.Find(CLItemId);
                if (system == null) return new CUDReturnMessage(ResponseCode.ItemMngt_NoExists);
                system.Visible = false;
                system.UpdatedBy =userId;
                system.UpdatedDate = DateTime.Now;
                DbContext.SaveChanges();

                #region log
                logAccess.Value.AddLog(new Contract.Log.LogObjectContract()
                {
                    ObjectId = CLItemId,
                    OldState = 0,
                    NewState = 0,
                    ObjectTypeId = (int)ObjectTypeEnum.Item,
                    ActionId = (int)ActionTypeEnum.Orther,
                    CreatedBy = userId,
                    NewInformation = "Xóa hang mục item "
                });
                #endregion


                return new CUDReturnMessage(ResponseCode.ItemMngt_SuccessDeleted);
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.ItemMngt_Error, ex.Message);
            }
            return result;
        }

        public CUDReturnMessage InsertUpdateItem(ItemContract info, int userId)
        {
            try
            {
                var result = default(CUDReturnMessage);
                if (info == null) new CUDReturnMessage(ResponseCode.ItemMngt_NoExists);

                if (DbContext.CheckListItems.Where(i => i.ItemName.ToLower().Trim().Equals(info.ItemName.ToLower().Trim()) && i.CLItemId != info.CLItemId).Any())
                    return result = new CUDReturnMessage(ResponseCode.ItemMngt_DuplicateItemExist);

                var item = DbContext.CheckListItems.FirstOrDefault(c => (c.CLItemId == info.CLItemId));
                if (item != null)
                {
                    #region update temitemplate
                    item.UpdatedBy = userId;
                    item.UpdatedDate = DateTime.Now;
                    item.Visible = info.Visible;
                    item.Description = info.Description;
                    //system.SystemName = info.SystemName;
                    item.Sort = info.Sort;
                    #endregion
                    result = new CUDReturnMessage(ResponseCode.ItemMngt_SuccessUpdated);
                }
                else
                {
                    item = new CheckListItem()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = this.uid,
                        UpdatedDate = DateTime.Now,
                        UpdatedBy = userId,
                        ItemName = info.ItemName,
                        Description = info.Description,
                        Visible = info.Visible,
                        Sort = info.Sort
                    };
                    DbContext.CheckListItems.Add(item);
                    result = new CUDReturnMessage(ResponseCode.ItemMngt_SuccessCreated);
                }
                DbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.ItemMngt_Error, ex.Message);
            }
        }

        public IQueryable<CheckList> ListCheckList(int state, int systemId, int checkListTypeId, int checkListId, string kw)
        {
            var query = DbContext.CheckLists.AsQueryable();
            if (checkListId > 0)
                return query = query.Where(c => c.CheckListId == checkListId);
            if (state > 0)
                query = query.Where(c => c.Visible == (state == (int)SystemStateEnum.Active));
            if (!string.IsNullOrEmpty(kw))
                query = query.Where(c => c.CheckListName.Contains(kw));
            if (systemId > 0)
                query = query.Where(c => c.SystemId == systemId);
            if (checkListTypeId > 0)
                query = query.Where(c => c.CheckListTypeId == checkListTypeId);
            return query;
        }
        public CUDReturnMessage InsertUpdateCheckList(CheckListContract data, int userId)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            if (DbContext.CheckLists.Where(i => i.CheckListName.ToLower().Trim().Equals(data.CheckListName.ToLower().Trim()) && i.CheckListId != data.CheckListId).Any())
                return new CUDReturnMessage(ResponseCode.CheckList_DuplicateExist);
            if (data.Visible && !data.lstItemIds.Any())
                return new CUDReturnMessage(ResponseCode.CheckList_NoAssignItem);

            if (data.Visible == true)//kiem tra set owner co set owner hệ thống này chưa? luc đó mới cho active
            {
                var systemCheckList = DbContext.SystemCheckLists.FirstOrDefault(c => c.SystemId == data.SystemId && c.State == (int)SystemStatusEnum.Active);
                if (systemCheckList == null)
                    return new CUDReturnMessage(ResponseCode.SystemMngt_NoExists);
                if (systemCheckList.AdminUser_System == null || !systemCheckList.AdminUser_System.Any(c => c.IsDeleted == false))
                    return new CUDReturnMessage(ResponseCode.CheckList_NotActiveWhenNoHavingOwner);
            }
            try
            {
                if (data.CheckListId == 0) //Insert
                {
                    Models.CheckList newChecklist = new Models.CheckList()
                    {
                        CheckListName = data.CheckListName,
                        Description = data.Description,
                        Visible = data.Visible,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = 0,
                        UpdatedDate = DateTime.Now,
                        Priority = data.Priority,
                        SystemId = data.SystemId,
                        CheckListTypeId = data.CheckListTypeId,
                        IsSync = false,
                        SetupDateFrom = data.SetupDateFrom
                    };
                    foreach (var clItemId in data.lstItemIds)
                    {
                        newChecklist.CheckListItemMaps.Add(new CheckListItemMap()
                        {
                            CreatedDate = DateTime.Now,
                            CreatedBy = userId,
                            CLItemId = clItemId,
                            Visible = true
                        });
                    }
                    DbContext.CheckLists.Add(newChecklist);
                    
                    DbContext.SaveChanges();


                    #region log
                    logAccess.Value.AddLog(new Contract.Log.LogObjectContract()
                    {
                        ObjectId = newChecklist.CheckListId,
                        OldState = 0,
                        NewState = 0,
                        ObjectTypeId = (int)ObjectTypeEnum.CheckList,
                        ActionId = (int)ActionTypeEnum.Orther,
                        CreatedBy = userId,
                        NewInformation = "khai báo checklist"
                    });
                    #endregion


                    return new CUDReturnMessage(ResponseCode.CheckList_SuccessCreate);
                }
                else   //Update
                {
                    var checklist = DbContext.CheckLists.SingleOrDefault(r => r.CheckListId == data.CheckListId);
                    if (checklist == null)
                        return new CUDReturnMessage(ResponseCode.Error);

                    checklist.CheckListName = data.CheckListName;
                    checklist.Description = data.Description;
                    checklist.Visible = data.Visible;
                    checklist.UpdatedBy = userId;
                    checklist.Priority = data.Priority;
                    checklist.UpdatedDate = DateTime.Now;
                    checklist.SetupDateFrom = data.SetupDateFrom;
                    checklist.CheckListItemMaps.ToList().ForEach(c => { c.Visible = false; c.UpdatedBy = uid; c.UpdatedDate = DateTime.Now; });
                    foreach (var clItemId in data.lstItemIds)
                    {
                        var itemMap = checklist.CheckListItemMaps.FirstOrDefault(c => c.CLItemId.Equals(clItemId));
                        if (itemMap != null) itemMap.Visible = true;
                        else
                        {
                            checklist.CheckListItemMaps.Add(new CheckListItemMap()
                            {
                                CLItemId = clItemId,
                                CreatedDate = DateTime.Now,
                                CreatedBy =userId,
                                Visible = true,

                            });
                        }
                    }
                    DbContext.SaveChanges();

                    #region log
                    logAccess.Value.AddLog(new Contract.Log.LogObjectContract()
                    {
                        ObjectId = checklist.CheckListId,
                        OldState = 0,
                        NewState = 0,
                        ObjectTypeId = (int)ObjectTypeEnum.CheckList,
                        ActionId = (int)ActionTypeEnum.Orther,
                        CreatedBy = userId,
                        NewInformation = "chỉnh sửa khai báo checklist"
                    });
                    #endregion

                    return new CUDReturnMessage(ResponseCode.CheckList_SuccessUpdate);
                }
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.Error, ex.ToString());
            }
        }

        public List<OperationCheckListContract> GenCheckListByType(int quantityCheckList)
        {

            DateTime dtNow = DateTime.Now;
            int delta = DayOfWeek.Monday - dtNow.Date.DayOfWeek;
            DateTime mondayWeek = Extension.AddTimeToTheStartOfDay(dtNow.Date.AddDays(delta));
            DateTime dayEndWeek = Extension.AddTimeToTheEndOfDay(mondayWeek.AddDays(5));
            Boolean checkTypeWeekly = mondayWeek <= dtNow && dtNow <= dayEndWeek && dtNow.Date.DayOfWeek != DayOfWeek.Sunday;
            var query = DbContext.CheckLists.AsQueryable();
            query = query.Where(c => c.Visible == true
                                        && c.IsProcess == false
                                        && c.SetupDateFrom <= dtNow
                                        && ( c.IsSync == false
                                            ||(
                                                    c.DateSync != null && c.CheckListTypeId != (int)CheckListTypeEnum.OnlyOne  
                                                    &&
                                                    (
                                                        (((int)CheckListTypeEnum.Monthly==(int)c.CheckListTypeId) && c.DateSync.Value.Month!= dtNow.Month && dtNow.Date.DayOfWeek != DayOfWeek.Sunday)
                                                        ||(((int)CheckListTypeEnum.Weekly == (int)c.CheckListTypeId) && (checkTypeWeekly) && !( (c.DateSync >= mondayWeek) && c.DateSync <= dayEndWeek))
                                                        || (((int)CheckListTypeEnum.Daily == (int)c.CheckListTypeId) && (!(c.DateSync.Value.Day==dtNow.Day && c.DateSync.Value.Month == dtNow.Month && c.DateSync.Value.Year == dtNow.Year) && dtNow.Date.DayOfWeek != DayOfWeek.Sunday))
                                                    )                                             
                                                )
                                          )
                                );            
            var data = quantityCheckList == -1 ? query.ToList() : query.Take(quantityCheckList).ToList();
            data.ForEach(c => { c.IsProcess = true; c.DateSync = dtNow; });
            DbContext.SaveChanges();
            List<OperationCheckListContract> listSendMails = new List<OperationCheckListContract>();
            foreach (var chklist in data)
            {
                CheckListOperation operationObject = CloneCheckList(chklist);
                DbContext.CheckListOperations.Add(operationObject);
                var listEmail = chklist.SystemCheckList.AdminUser_System.Select(d => d.AdminUser.Email).ToList();
                string email = string.Join(",", listEmail);
                listSendMails.Add(new OperationCheckListContract() {CheckListId=chklist.CheckListId ,CheckListName=chklist.CheckListName,OwnerEmail= email } );                
            }

            data.ForEach(c => { c.IsProcess = false; c.IsSync = true; c.DateSync = DateTime.Now;  });
            
            DbContext.SaveChanges();

            List<OperationCheckListContract> listNotifys = new List<OperationCheckListContract>();


            return listSendMails;

            #endregion

        }
        private Boolean ValidateCheckListType(DateTime dtLastSync,int checkListTypeId)
        {
            Boolean result =false;
            if (dtLastSync == null) return true;
            DateTime dt = DateTime.Now;
            if (checkListTypeId == (int)CheckListTypeEnum.Weekly)
            {
                 return  !Extension.AreFallingInSameWeek(dt,dtLastSync);
            }
             else if(checkListTypeId == (int)CheckListTypeEnum.Monthly)
            {
                return dt.Month != dtLastSync.Month;
            }
            return result;
        }
       
        public CheckListOperation CloneCheckList(CheckList chkList)
        {
            var operationChkList = new CheckListOperation()
            {
                SystemId = chkList.SystemId,
                CheckListId = chkList.CheckListId,
                CheckListTypeId = chkList.CheckListTypeId,
                CheckListStatusId = (int)CheckListStateEnum.PrepareExecute,
                CreatedBy = chkList.CreatedBy,
                CreatedDate = DateTime.Now,
                Description = chkList.Description,
                Deadline = DateTime.Now,             //to update bo sung field
                CheckListName=chkList.CheckListName+" "+DateTime.Now.ToString("dd-MM-yyyy"),
                SetupDateFrom=chkList.SetupDateFrom
            };
            switch (chkList.CheckListTypeId)
            {
                case (int)CheckListTypeEnum.Daily:
                case (int)CheckListTypeEnum.OnlyOne:
                                                        operationChkList.Deadline = Extension.AddTimeToTheEndOfDay(DateTime.Now);
                                                        break;
                case (int)CheckListTypeEnum.Weekly:    operationChkList.Deadline = Extension.AddTimeToTheEndOfDay(Extension.LastDayOfWeek(DateTime.Now));  break;

                case (int)CheckListTypeEnum.Monthly:
                                                        operationChkList.Deadline = Extension.AddTimeToTheEndOfDay(Extension.ToLastDayOfMonth(DateTime.Now));  break;
            }
            foreach (var chkMap in chkList.CheckListItemMaps)
            {
                if (chkMap.Visible == true)
                {
                    operationChkList.CheckListOperationMappings.Add(new CheckListOperationMapping()
                    {
                        CheckListId = chkList.CheckListId,
                        CLItemId = chkMap.CLItemId,
                        State = 0,
                        CreatedDate = DateTime.Now
                    });
                }

            }
            return operationChkList;
        }

       



        #region worker gen checklist
        public void ExecuteBlock(Action blockFunction)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    blockFunction();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #endregion
    }
}
