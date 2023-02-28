using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract.AdminAction;
using Contract.Shared;
using DataAccess.Models;

namespace DataAccess.DAO
{
    public interface IAdminGroupActionDataAccess
    {
        IQueryable<Models.AdminGroupAction> GetListGroupAction(bool? isDeleted, int rId);
        IQueryable<Models.AdminGroupAction_Map> GetAllGroupActionMap();
        CUDReturnMessage ImportGroupActions(List<AdminGroupActionContract> data);
        CUDReturnMessage ImportGroupActionMaps(List<AdminGroupActionMapContract> data);
    }

    public class AdminGroupActionDataAccess : BaseDataAccess, IAdminGroupActionDataAccess
    {
        public AdminGroupActionDataAccess(string appid, int uid) : base(appid, uid)
        {
        }

        public IQueryable<AdminGroupAction> GetListGroupAction(bool? isDeleted, int rId)
        {
            var query = DbContext.AdminGroupActions.Where(r => true);

            if (isDeleted != null)
            {
                query = query.Where(r => r.IsDeleted == isDeleted);
            }

            if (rId > 0)
            {
                query = query.Where(r => r.AdminRole_GroupAction.Where(m => m.RId == rId && !m.IsDeleted).Any());
            }

            return query;
        }

        public IQueryable<AdminGroupAction_Map> GetAllGroupActionMap()
        {
            return DbContext.AdminGroupAction_Map.Where(r => 1 == 1);
        }

        public CUDReturnMessage ImportGroupActions(List<AdminGroupActionContract> data)
        {
            if (data != null && data.Count > 0)
            {
                foreach (var group in data)
                {
                    var dbGa = DbContext.AdminGroupActions.Where(r => r.GaId == group.GaId).SingleOrDefault();

                    if (dbGa == null)
                    {
                        dbGa = new AdminGroupAction
                        {
                            GaId = group.GaId,
                            Name = group.Name,
                            Description = group.Description,
                            CreatedBy = uid,
                            CreatedDate = DateTime.Now,
                            LastUpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,
                            IsDeleted = group.IsDeleted
                        };

                        DbContext.AdminGroupActions.Add(dbGa);
                    }
                    else if (IsDifferent(dbGa, group))
                    {
                        dbGa.Name = group.Name;
                        dbGa.Description = group.Description;
                        dbGa.LastUpdatedBy = uid;
                        dbGa.LastUpdatedDate = DateTime.Now;
                        dbGa.IsDeleted = group.IsDeleted;
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

        public CUDReturnMessage ImportGroupActionMaps(List<AdminGroupActionMapContract> data)
        {
            if (data != null && data.Count > 0)
            {
                foreach (var map in data)
                {
                    var dbmap = DbContext.AdminGroupAction_Map.Where(r => r.GaId == map.GaId && r.AId == map.AId).SingleOrDefault();

                    if (dbmap == null)
                    {
                        dbmap = new AdminGroupAction_Map
                        {
                            GaId = map.GaId,
                            AId = map.AId,                            
                            CreatedBy = uid,
                            CreatedDate = DateTime.Now,
                            LastUpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,
                            IsDeleted = map.IsDeleted
                        };

                        DbContext.AdminGroupAction_Map.Add(dbmap);
                    }
                    else if (dbmap.IsDeleted != map.IsDeleted)
                    {
                        dbmap.LastUpdatedBy = uid;
                        dbmap.LastUpdatedDate = DateTime.Now;
                        dbmap.IsDeleted = map.IsDeleted;
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

        private bool IsDifferent(Models.AdminGroupAction dbgroup, AdminGroupActionContract group)
        {
            return dbgroup.Name != group.Name 
                || dbgroup.Description != group.Description 
                || dbgroup.IsDeleted != group.IsDeleted;
        }
    }
}
