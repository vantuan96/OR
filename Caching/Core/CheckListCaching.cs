using System;
using System.Collections.Generic;
using Business.Core;
using Contract.Shared;
using Contract.User;
using Contract.CheckList;
using Contract.MasterData;
using Contract.OperationCheckList;

namespace Caching.Core
{
    public interface ICheckListCaching
    {
        #region Check List
        List<CheckListContract> GetListCheckList(int Sort);
        CUDReturnMessage InsertUpdateCheckList(CheckListContract data,int userId);
        CUDReturnMessage ActiveCheckList(int CheckListId,int userId);
        #endregion

        #region Check List Item
        List<CheckListItemContract> GetCheckListIitem();
        CUDReturnMessage InsertUpdateCheckListItem(CheckListItemContract data,int userId);
        CUDReturnMessage DeleteCheckListItem(int CheckListDetailsId,int userId);
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
        CUDReturnMessage DeleteItem(int CLItemId,int userId);
        /// <summary>
        /// Tạo mới/ Cập nhật item
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateItem(ItemContract info,int userId);
        #endregion
        #region new checklist
        SearchCheckList ListCheckList(int state, int systemId, int checkListTypeId, int checkListId, string kw, int p, int ps);
        #endregion

        #region worker
        void ExecuteBlock(Action blockFunction);
        List<OperationCheckListContract> GenCheckListByType(int quantityCheckList);
        #endregion
    }

    public class CheckListCaching : BaseCaching, ICheckListCaching
    {
        private Lazy<ICheckListBusiness> lazyCheckListBusiness;

        public CheckListCaching(/*string appid, int uid*/)  
        {
            lazyCheckListBusiness = new Lazy<ICheckListBusiness>(() => new CheckListBusiness(appid, uid));
        }
      


        #region Check List
        public List<CheckListContract> GetListCheckList(int Sort)
        {
            return lazyCheckListBusiness.Value.GetListCheckList(Sort);
        }

        public CUDReturnMessage InsertUpdateCheckList(CheckListContract data,int userId)
        {
            return lazyCheckListBusiness.Value.InsertUpdateCheckList(data,userId);
        }

        public CUDReturnMessage ActiveCheckList(int CheckListId,int userId)
        {
            return lazyCheckListBusiness.Value.ActiveCheckList(CheckListId,userId);
        }

        #endregion

        #region Check List Item
        public List<CheckListItemContract> GetCheckListIitem()
        {
            return lazyCheckListBusiness.Value.GetCheckListIitem();
        }

        public CUDReturnMessage InsertUpdateCheckListItem(CheckListItemContract data,int userId)
        {
            return lazyCheckListBusiness.Value.InsertUpdateCheckListItem(data,userId);
        }

        public CUDReturnMessage DeleteCheckListItem(int CheckListDetailsId,int userId)
        {
            return lazyCheckListBusiness.Value.DeleteCheckListItem(CheckListDetailsId,userId);
        }


        #endregion

        #region Item
        public SearchItem GetItem(int state, string kw, int p, int ps, int clItemId)
        {
            return lazyCheckListBusiness.Value.GetItem(state, kw, p, ps, clItemId);
        }

        public CUDReturnMessage DeleteItem(int CLItemId,int userId)
        {
            return lazyCheckListBusiness.Value.DeleteItem(CLItemId,userId);
        }

        public CUDReturnMessage InsertUpdateItem(ItemContract info,int userId)
        {
            return lazyCheckListBusiness.Value.InsertUpdateItem(info,userId);
        }

        #endregion

        #region new checklist
        public SearchCheckList ListCheckList(int state, int systemId, int checkListTypeId, int checkListId, string kw, int p, int ps)
        {
            return lazyCheckListBusiness.Value.ListCheckList(state, systemId, checkListTypeId, checkListId, kw, p, ps);
        }

        public void ExecuteBlock(Action blockFunction)
        {
            lazyCheckListBusiness.Value.ExecuteBlock(blockFunction);
        }

        public List<OperationCheckListContract> GenCheckListByType(int quantityCheckList)
        { 
           return  lazyCheckListBusiness.Value.GenCheckListByType(quantityCheckList);
        }
        #endregion
    }
}
