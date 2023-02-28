using System;
using System.Collections.Generic;
using System.Linq;
using BMS.Contract.Menu;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;

namespace BMS.Business.Menu
{
    public interface IMenuMngtBusiness
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

    public class MenuMngtBusiness : BaseBusiness, IMenuMngtBusiness
    {
        private Lazy<IMenuItemDataAccess> lazyMenuItemDataAccess;

        public MenuMngtBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyMenuItemDataAccess = new Lazy<IMenuItemDataAccess>(() => new MenuItemDataAccess(appid, uid));
        }

        public List<MenuItemContract> GetListMenuItem(int MsId, bool isTemplate)
        {            
            var query = lazyMenuItemDataAccess.Value.GetListMenuItem(MsId, null, isTemplate);

            return query.Select(r => new MenuItemContract
            {
                MenuItemId = r.MenuItemId,
                IsOnsite = r.IsOnsite,
                ApprovalStatus = r.ApprovalStatus,
                Sort = r.Sort,
                Name = r.MenuItemContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault(),
                ParentId = r.ParentId,
                IsPredefined = r.IsPredefined,
                TemplateId = r.TemplateId
            }).ToList();
        }

        public MenuItemDetailContract GetMenuItemDetail(int menuItemId)
        {
            var query = lazyMenuItemDataAccess.Value.GetMenuItemById(menuItemId);

            var detail = query.Select(menu => new MenuItemDetailContract
            {
                MenuItemId = menu.MenuItemId,
                MsId = menu.MsId,
                IsOnsite = menu.IsOnsite,
                Sort = menu.Sort,
                Status = menu.ApprovalStatus,
                IsPredefined = menu.IsPredefined,
                IsTemplate = menu.IsTemplate
            }).SingleOrDefault();

            detail.ListMenuItemContent = new List<MenuItemContentContract>();
            if (detail == null) return null;

            foreach(var lang in listApprovedLanguage)
            {
                var content = query.Select(menu => menu.MenuItemContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang))).First();
                if (content == null)
                {
                    detail.ListMenuItemContent.Add(new MenuItemContentContract
                    {
                        LangShortName = lang,
                        MenuItemContentId = 0,
                        MenuItemId = detail.MenuItemId,
                        MetaDescription = "",
                        MetaKeyword = "",
                        MetaTitle = "",
                        MsId = detail.MsId,
                        Name = "",
                        RedirectUrl = ""
                    });
                }
                else
                {
                    detail.ListMenuItemContent.Add(new MenuItemContentContract
                    {
                        LangShortName = lang,
                        MenuItemContentId = content.MenuItemContentId,
                        MenuItemId = detail.MenuItemId,
                        MetaDescription = content.MetaDescription,
                        MetaKeyword = content.MetaKeyword,
                        MetaTitle = content.MetaTitle,
                        MsId = detail.MsId,
                        Name = content.Name,
                        RedirectUrl = content.RedirectUrl
                    });
                }
            }

            return detail;
        }

        public MenuItemContentContract GetMenuItemContentById(int menuItemContentId)
        {
            var query = lazyMenuItemDataAccess.Value.GetMenuItemContentById(menuItemContentId);

            var content = query.Select(c => new MenuItemContentContract
            {
                LangShortName = c.LangShortName,
                MenuItemContentId = c.MenuItemContentId,
                MenuItemId = c.MenuItemId,
                MetaDescription = c.MetaDescription,
                MetaKeyword = c.MetaKeyword,
                MetaTitle = c.MetaTitle,
                MsId = c.MenuItem.MsId,
                Name = c.Name,
                RedirectUrl = c.RedirectUrl
            }).SingleOrDefault();

            if (content != null)
            {
                var query2 = lazyMenuItemDataAccess.Value.GetMenuItemById(content.MenuItemId);
                content.IsPredefined = query2.Select(r => r.IsPredefined).FirstOrDefault();
            }

            return content;
        }

        public CUDReturnMessage CreateUpdateMenuItem(MenuItemContentContract menu)
        {
            if (menu.MenuItemContentId > 0)
                return lazyMenuItemDataAccess.Value.UpdateMenuItemContent(menu);

            if (menu.MenuItemId > 0)
                return lazyMenuItemDataAccess.Value.CreateMenuItemContent(menu);

            if (string.IsNullOrEmpty(menu.LangShortName))
            {
                menu.LangShortName = defaultLanguageCode;
            }

            return lazyMenuItemDataAccess.Value.CreateMenuItem(menu);
        }

        public CUDReturnMessage UpdateMenuItemApprovalStatus(int MenuItemId, int Status)
        {
            return lazyMenuItemDataAccess.Value.UpdateMenuItemApprovalStatus(MenuItemId, Status);
        }

        public CUDReturnMessage DeleteMenuItem(int MenuItemId)
        {
            return lazyMenuItemDataAccess.Value.DeleteMenuItem(MenuItemId);
        }
        
        public CUDReturnMessage UpdateMenuItemSort(List<UpdateMenuItemSortContract> menuItems)
        {
            return lazyMenuItemDataAccess.Value.UpdateMenuItemSort(menuItems);
        }

        /// <summary>
        /// Tạo menu tự động cho microsite
        /// </summary>
        /// <param name="msId"></param>
        /// <returns></returns>
        public CUDReturnMessage CreateMenuFromTemplate(int msId) {
            return lazyMenuItemDataAccess.Value.CreateMenuFromTemplate(msId);
        }

        #region FE

        public List<Contract.FE.MenuItemContract> GetListOnsiteMenuItem(int MsId, string lang)
        {
            var query = lazyMenuItemDataAccess.Value.GetListMenuItem(MsId, true, false);

            return query.Select(r => new Contract.FE.MenuItemContract
            {
                MenuItemId = r.MenuItemId,
                Sort = r.Sort,
                IsPredefined = r.IsPredefined,
                Content = r.MenuItemContents.Where(c => c.IsDeleted == false && c.LangShortName == lang).Select(c => new Contract.FE.MenuItemContentContract
                {
                    Name = c.Name,
                    Url = c.RedirectUrl,
                    MetaTitle = c.MetaTitle,
                    MetaKeyword = c.MetaKeyword,
                    MetaDescription = c.MetaDescription
                }).FirstOrDefault(),
                ContentDefaultLanguage = r.MenuItemContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => new Contract.FE.MenuItemContentContract
                {
                    Name = c.Name,
                    Url = c.RedirectUrl,
                    MetaTitle = c.MetaTitle,
                    MetaKeyword = c.MetaKeyword,
                    MetaDescription = c.MetaDescription
                }).FirstOrDefault(),
                ParentId = r.ParentId
            }).ToList();
        }

        #endregion FE
    }
}
