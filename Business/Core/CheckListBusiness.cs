using System;
using System.Linq;
using System.Collections.Generic;
using Contract.Shared;
using VG.Common;
using DataAccess.DAO;
using Contract.CheckList;
using Contract.MasterData;
using Contract.Enum;
using Contract.OperationCheckList;

namespace Business.Core
{
    public interface ICheckListBusiness
    {
        #region Check list
        List<CheckListContract> GetListCheckList(int Sort);
        CUDReturnMessage InsertUpdateCheckList(CheckListContract data,int userId);
        CUDReturnMessage ActiveCheckList(int CheckListId, int userId);
        #endregion

        #region Check list Item
        List<CheckListItemContract> GetCheckListIitem();
        CUDReturnMessage InsertUpdateCheckListItem(CheckListItemContract data, int userId);
        CUDReturnMessage DeleteCheckListItem(int CheckListDetailsId, int userId);
        #endregion

        #region Item checklist
       /// <summary>
       /// Lay danh muc item
       /// </summary>
       /// <param name="state"></param>
       /// <param name="kw"></param>
       /// <param name="p"></param>
       /// <param name="ps"></param>
       /// <param name="clItemId"></param>
       /// <returns></returns>
        SearchItem GetItem(int state, string kw, int p, int ps, int clItemId);
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
        SearchCheckList ListCheckList(int state, int systemId, int checkListTypeId, int checkListId, string kw, int p, int ps);
        #endregion

        #region worker gen check list
        List<OperationCheckListContract> GenCheckListByType(int quantityCheckList);
        void ExecuteBlock(Action blockFunction);
        #endregion



    }

    public class CheckListBusiness : BaseBusiness, ICheckListBusiness
    {
        private Lazy<ICheckListDataAccess> lazyCheckListDataAccess;

        #region Check list
        public CheckListBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyCheckListDataAccess = new Lazy<ICheckListDataAccess>(() => new CheckListDataAccess(appid, uid));

        }

        public List<CheckListContract> GetListCheckList(int Sort)
        {
            var infoUser = lazyCheckListDataAccess.Value.GetListUser();
            var checkList = lazyCheckListDataAccess.Value.GetListCheckList(sort: Sort);

            return checkList.Select(r => new CheckListContract
            {
                CheckListId = r.CheckListId,
                CheckListName = r.CheckListName,
                Description = r.Description,
                Visible = (bool)r.Visible,
                Priority = (int)r.Priority,
                StateName = (bool) r.Visible ? "Đang sử dụng":"Không sử dụng",
                CreatedDate = (DateTime)r.CreatedDate,
                CreateName = infoUser.FirstOrDefault(x=>x.UId == r.CreatedBy).Name
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateCheckList(CheckListContract data, int userId)
        {
            return lazyCheckListDataAccess.Value.InsertUpdateCheckList(data,userId);
        }

        public CUDReturnMessage ActiveCheckList(int CheckListId, int userId)
        {
            return lazyCheckListDataAccess.Value.ActiveCheckList(CheckListId,userId);
        }
        #endregion

        public List<CheckListItemContract> GetCheckListIitem()
        {
            var checkList = lazyCheckListDataAccess.Value.GetListCheckList(0);

            var ListItem = lazyCheckListDataAccess.Value.GetCheckListIitem();
            return ListItem.Select(r => new CheckListItemContract()
            {
                CheckListDetailId = r.CheckListDetailId,
                CheckListDetailName = r.CheckListDetailName,
                CheckListId = r.CheckListId,
                Description = r.Description,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreatedDate,
                CreatedBy = (int)r.CreatedBy,
                UpdatedDate = (DateTime)r.UpdatedDate,
                UpdatedBy = (int)r.UpdatedBy,
                Priority = (int)r.Priority,
                CheckListName = checkList.FirstOrDefault(x=>x.CheckListId == r.CheckListId).CheckListName
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateCheckListItem(CheckListItemContract data, int userId)
        {
            return lazyCheckListDataAccess.Value.InsertUpdateCheckListDetails(data,userId);
        }


        public CUDReturnMessage DeleteCheckListItem(int CheckListDetailsId, int userId)
        {
            return lazyCheckListDataAccess.Value.DeleteCheckListItem(CheckListDetailsId,userId);
        }

        #region Item check list
        public SearchItem GetItem(int state , string kw , int p, int ps, int clItemId )
        {
            SearchItem result = new SearchItem() { Data = new List<ItemContract>(), TotalRows = 0 };
            var query = lazyCheckListDataAccess.Value.GetItem(state, kw, clItemId);

            if (query != null && query.Any())
            {
                result.TotalRows = query.Count();
                var data = query.OrderByDescending(c => c.CreatedDate)
                            .ThenBy(c => c.Sort)
                            .ThenByDescending(c => c.ItemName)
                            .Skip((p - 1) * ps)
                            .Take(ps).ToList();
                result.Data = data.Select(c => new ItemContract()
                {
                    CLItemId = c.CLItemId,
                    ItemName = c.ItemName,
                    Description = c.Description,
                    Visible = c.Visible ?? false,
                    CreatedBy = c.CreatedBy ?? 0,
                    CreatedDate = c.CreatedDate,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedDate = c.UpdatedDate,
                    StateName = EnumExtension.GetDescription((SystemStateEnum)((c.Visible ?? true) ? SystemStateEnum.Active : SystemStateEnum.NoActive))
                }).ToList();
            }
            return result;
        }

        public CUDReturnMessage DeleteItem(int CLItemId, int userId)
        {
            return lazyCheckListDataAccess.Value.DeleteItem(CLItemId,userId);
        }

        public CUDReturnMessage InsertUpdateItem(ItemContract info, int userId)
        {
            return lazyCheckListDataAccess.Value.InsertUpdateItem(info,userId);
        }
        #endregion

        #region new checklist
        public SearchCheckList ListCheckList(int state , int systemId , int checkListTypeId ,int checkListId, string kw , int p, int ps )
        {
            SearchCheckList result = new SearchCheckList() { Data = new List<CheckListContract>(), TotalRows = 0 };
            var query = lazyCheckListDataAccess.Value.ListCheckList(state, systemId,checkListTypeId, checkListId, kw);

            if (query != null && query.Any())
            {
                result.TotalRows = query.Count();
                var data = query.OrderByDescending(c => c.CreatedBy)
                            .ThenBy(c => c.Priority)
                            .ThenByDescending(c => c.CheckListName)
                            .Skip((p - 1) * ps)
                            .Take(ps).ToList();
                result.Data = data.Select(c => new CheckListContract()
                {
                    CheckListId = c.CheckListId,
                    CheckListName = c.CheckListName,
                    Description = c.Description,
                    Visible = c.Visible ?? false,
                    CreatedBy = c.CreatedBy ?? 0,
                    CreatedDate = c.CreatedDate ?? DateTime.Now,
                    UpdatedBy = c.UpdatedBy ?? 0,
                    UpdatedDate = c.UpdatedDate ?? DateTime.Now,
                    Priority = c.Priority ?? 1,
                    CheckListTypeId = c.CheckListType.CheckListTypeId,
                    CheckListTypeName = c.CheckListType.CheckListTypeName,
                    StateName = EnumExtension.GetDescription((SystemStateEnum)((c.Visible ?? true) ? SystemStateEnum.Active : SystemStateEnum.NoActive)),
                    IsSync = c.IsSync,

                    DateSync = c.DateSync ?? DateTime.Now,
                    SystemId = c.SystemId,
                    SystemName = c.SystemCheckList.SystemName,
                    lstItemIds = c.CheckListItemMaps.Where(i => i.Visible == true).Select(d => d.CLItemId).ToList(),
                    Items = c.CheckListItemMaps.Where(i => i.Visible == true).Select(d => new ItemContract() { ItemName = d.CheckListItem.ItemName,CLItemId=d.CheckListItem.CLItemId}).ToList(),
                    SetupDateFrom =c.SetupDateFrom

                }).ToList();
            }
            return result;
        }

        public List<OperationCheckListContract> GenCheckListByType(int quantityCheckList)
        {
           return  lazyCheckListDataAccess.Value.GenCheckListByType(quantityCheckList);
        }

        public void ExecuteBlock(Action blockFunction)
        {
            lazyCheckListDataAccess.Value.ExecuteBlock(blockFunction);
        }
        #endregion
    }
}
