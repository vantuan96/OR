using System;
using System.Collections.Generic;
using System.Linq;
using VG.Common;
using Contract.AdminAction;
using Contract.Shared;
using Contract.User;
using DataAccess.Models;

namespace DataAccess
{
    public interface IAdminActionDataAccess
    {
        List<AdminActionContract> GetListActionByListRoleId(List<int> listRoleIds);
        IQueryable<Models.AdminController> GetAllController();
        CUDReturnMessage ImportActions(List<AdminControllerContract> data);
    }

    public class AdminActionDataAccess : BaseDataAccess, IAdminActionDataAccess
    {
        public AdminActionDataAccess(string appid, int uid) : base(appid, uid)
        {

        }
        public IQueryable<AdminController> GetAllController()
        {
            return DbContext.AdminControllers.Where(r => true);
        }

        public List<AdminActionContract> GetListActionByListRoleId(List<int> listRoleIds)
        {
            var listPublicActions = (from action in DbContext.AdminActions
                                     join controller in DbContext.AdminControllers on action.CId equals controller.CId
                                     where controller.IsDeleted == false
                                        && controller.Visible == true
                                        && action.IsDeleted == false
                                        && action.Visible == true
                                        && action.PublicStatus == true
                                     select new AdminActionContract
                                     {
                                         ControllerName = controller.ControllerName,
                                         ControllerDisplayName = controller.ControllerDisplayName,
                                         ControllerCssIcon = controller.CssIcon,
                                         ControllerSort = controller.Sort,
                                         ActionName = action.ActionName,
                                         ActionDisplayName = action.ActionDisplayName,
                                         ActionCssIcon = action.CssIcon,
                                         IsShowMenu = action.ShowMenuStatus,
                                         IsDefault = action.IsDefault,
                                         ActionSort = action.Sort
                                     });

            var listGrantedAction = (from rolegroup in DbContext.AdminRole_GroupAction
                                     join gr in DbContext.AdminGroupActions on rolegroup.GaId equals gr.GaId
                                     join map in DbContext.AdminGroupAction_Map on gr.GaId equals map.GaId
                                     join action in DbContext.AdminActions on map.AId equals action.AId
                                     join controller in DbContext.AdminControllers on action.CId equals controller.CId
                                     where rolegroup.IsDeleted == false
                                         && gr.IsDeleted == false
                                         && map.IsDeleted == false
                                         && action.IsDeleted == false
                                         && action.Visible == true
                                         && controller.IsDeleted == false
                                         && controller.Visible == true
                                         && listRoleIds.Contains(rolegroup.RId)
                                     select new AdminActionContract
                                     {
                                         ControllerName = controller.ControllerName,
                                         ControllerDisplayName = controller.ControllerDisplayName,
                                         ControllerCssIcon = controller.CssIcon,
                                         ControllerSort = controller.Sort,
                                         ActionName = action.ActionName,
                                         ActionDisplayName = action.ActionDisplayName,
                                         ActionCssIcon = action.CssIcon,
                                         IsShowMenu = action.ShowMenuStatus,
                                         IsDefault = action.IsDefault,
                                         ActionSort = action.Sort
                                     });

            return listPublicActions.Union(listGrantedAction).Distinct().OrderBy(r => r.ControllerSort).ThenBy(r => r.ActionSort).ToList();
        }

        public CUDReturnMessage ImportActions(List<AdminControllerContract> data)
        {
            if (data != null && data.Count > 0)
            {
                foreach (var controller in data)
                {
                    var dbController = DbContext.AdminControllers.Where(r => r.CId == controller.CId).SingleOrDefault();

                    if (dbController == null)
                    {
                        var newController = new AdminController
                        {
                            CId = controller.CId,
                            ControllerDisplayName = controller.ControllerDisplayName,
                            ControllerName = controller.ControllerName,
                            CreatedBy = uid,
                            CreatedDate = DateTime.Now,
                            LastUpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,
                            CssIcon = controller.CssIcon,
                            IsDeleted = controller.IsDeleted,
                            Sort = controller.Sort,
                            Visible = controller.Visible
                        };

                        DbContext.AdminControllers.Add(newController);
                    }
                    else if (IsDifferent(dbController, controller))
                    {
                        dbController.ControllerDisplayName = controller.ControllerDisplayName;
                        dbController.ControllerName = controller.ControllerName;
                        dbController.CssIcon = controller.CssIcon;
                        dbController.IsDeleted = controller.IsDeleted;
                        dbController.Sort = controller.Sort;
                        dbController.Visible = controller.Visible;
                        dbController.LastUpdatedBy = uid;
                        dbController.LastUpdatedDate = DateTime.Now;
                    }

                    if (controller.ListActions != null && controller.ListActions.Count > 0)
                        foreach (var action in controller.ListActions)
                        {
                            var dbAction = DbContext.AdminActions.Where(r => r.AId == action.AId).SingleOrDefault();

                            if (dbAction == null)
                            {
                                var newAction = new AdminAction
                                {
                                    ActionDisplayName = action.ActionDisplayName,
                                    ActionName = action.ActionName,
                                    CId = controller.CId,
                                    AId = action.AId,
                                    CreatedBy = uid,
                                    CreatedDate = DateTime.Now,
                                    LastUpdatedBy = uid,
                                    LastUpdatedDate = DateTime.Now,
                                    CssIcon = action.CssIcon,
                                    IsDefault = action.IsDefault,
                                    IsDeleted = action.IsDeleted,
                                    PublicStatus = action.PublicStatus,
                                    ShowMenuStatus = action.ShowMenuStatus,
                                    Sort = action.Sort,
                                    Visible = action.Visible
                                };

                                DbContext.AdminActions.Add(newAction);
                            }
                            else if (IsDifferent(dbAction, action) || dbAction.CId != controller.CId)
                            {
                                dbAction.ActionDisplayName = action.ActionDisplayName;
                                dbAction.ActionName = action.ActionName;
                                dbAction.CId = controller.CId;
                                dbAction.CssIcon = action.CssIcon;
                                dbAction.IsDefault = action.IsDefault;
                                dbAction.IsDeleted = action.IsDeleted;
                                dbAction.PublicStatus = action.PublicStatus;
                                dbAction.ShowMenuStatus = action.ShowMenuStatus;
                                dbAction.Sort = action.Sort;
                                dbAction.Visible = action.Visible;
                                dbAction.LastUpdatedBy = uid;
                                dbAction.LastUpdatedDate = DateTime.Now;
                            }
                        }
                }

                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.Successed);
            }
            else
            {
                return new CUDReturnMessage(ResponseCode.NoChanged);
            }
        }

        private bool IsDifferent(Models.AdminController dbController, AdminControllerContract controller)
        {
            return dbController.ControllerName != controller.ControllerName
                        || dbController.ControllerDisplayName != controller.ControllerDisplayName
                        || dbController.CssIcon != controller.CssIcon
                        || dbController.IsDeleted != controller.IsDeleted
                        || dbController.Sort != controller.Sort
                        || dbController.Visible != controller.Visible;
        }
        private bool IsDifferent(Models.AdminAction dbAction, AdminActionFullContract action)
        {
            return dbAction.ActionDisplayName != action.ActionDisplayName
                || dbAction.ActionName != action.ActionName
                || dbAction.CssIcon != action.CssIcon
                || dbAction.IsDefault != action.IsDefault
                || dbAction.IsDeleted != action.IsDeleted
                || dbAction.PublicStatus != action.PublicStatus
                || dbAction.ShowMenuStatus != action.ShowMenuStatus
                || dbAction.Sort != action.Sort
                || dbAction.Visible != action.Visible;            
        }
    }
}
