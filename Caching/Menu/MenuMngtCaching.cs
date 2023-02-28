using System;
using System.Collections.Generic;
using BMS.Business.Menu;
using BMS.Contract.Menu;
using BMS.Contract.Shared;

namespace BMS.Caching.Menu
{
    public interface IMenuMngtCaching
    {
        List<MenuItemContract> GetListMenuItem(int MsId, bool isTemplate);
        MenuItemDetailContract GetMenuItemDetail(int menuItemId);
        MenuItemContentContract GetMenuItemContentById(int menuItemContentId);
        CUDReturnMessage CreateUpdateMenuItem(MenuItemContentContract menu);
        CUDReturnMessage UpdateMenuItemApprovalStatus(int MenuItemId, int Status);
        CUDReturnMessage DeleteMenuItem(int MenuItemId);
        CUDReturnMessage UpdateMenuItemSort(List<UpdateMenuItemSortContract> menuItems);

        /// <summary>
        /// Tạo menu tự động cho microsite
        /// </summary>
        /// <param name="msId"></param>
        /// <returns></returns>
        CUDReturnMessage CreateMenuFromTemplate(int msId);

        #region FE

        List<Contract.FE.MenuItemContract> GetListOnsiteMenuItem(int MsId, string lang);

        #endregion FE
    }

    public class MenuMngtCaching : BaseCaching, IMenuMngtCaching
    {
        private Lazy<IMenuMngtBusiness> lazyMenuMngtBusiness;

        public MenuMngtCaching(/*string appid, int uid*/)  
        {
            lazyMenuMngtBusiness = new Lazy<IMenuMngtBusiness>(() => new MenuMngtBusiness(appid, uid));
        }

        public List<MenuItemContract> GetListMenuItem(int MsId, bool isTemplate)
        {
            return lazyMenuMngtBusiness.Value.GetListMenuItem(MsId, isTemplate);
        }

        public MenuItemDetailContract GetMenuItemDetail(int menuItemId)
        {
            return lazyMenuMngtBusiness.Value.GetMenuItemDetail(menuItemId);
        }

        public MenuItemContentContract GetMenuItemContentById(int menuItemContentId)
        {
            return lazyMenuMngtBusiness.Value.GetMenuItemContentById(menuItemContentId);
        }

        public CUDReturnMessage CreateUpdateMenuItem(MenuItemContentContract menu)
        {
            return lazyMenuMngtBusiness.Value.CreateUpdateMenuItem(menu);
        }

        public CUDReturnMessage UpdateMenuItemApprovalStatus(int MenuItemId, int Status)
        {
            return lazyMenuMngtBusiness.Value.UpdateMenuItemApprovalStatus(MenuItemId, Status);
        }

        public CUDReturnMessage DeleteMenuItem(int MenuItemId)
        {
            return lazyMenuMngtBusiness.Value.DeleteMenuItem(MenuItemId);
        }

        public CUDReturnMessage UpdateMenuItemSort(List<UpdateMenuItemSortContract> menuItems)
        {
            return lazyMenuMngtBusiness.Value.UpdateMenuItemSort(menuItems);
        }

        /// <summary>
        /// Tạo menu tự động cho microsite
        /// </summary>
        /// <param name="msId"></param>
        /// <returns></returns>
        public CUDReturnMessage CreateMenuFromTemplate(int msId) {
            return lazyMenuMngtBusiness.Value.CreateMenuFromTemplate(msId);
        }

        #region FE

        public List<Contract.FE.MenuItemContract> GetListOnsiteMenuItem(int MsId, string lang)
        {
            return lazyMenuMngtBusiness.Value.GetListOnsiteMenuItem(MsId, lang);
        }

        #endregion FE
    }
}
