using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Contract.Route;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;

namespace BMS.Business.Microsite
{
    public interface IRouteMngtBusiness
    {
        /// <summary>
        /// lấy danh sách route theo microsite
        /// </summary>
        /// <returns></returns>
        PagedList<RouteItemContract> GetListRoute(int msId, int page, int pageSize);

        /// <summary>
        /// thêm route
        /// </summary>
        /// <returns></returns>
        CUDReturnMessage CreateRoute(RouteCreateContract route);

        /// <summary>
        /// lấy thông tin chi tiết 1 route
        /// </summary>
        /// <returns></returns>
        RouteDetailContract GetRouteDetail(int routeId);

        /// <summary>
        /// cập nhật route
        /// </summary>
        /// <returns></returns>
        CUDReturnMessage UpdateRoute(RouteUpdateContract route);

        /// <summary>
        /// lấy thông tin 1 route content để update
        /// </summary>
        /// <returns></returns>
        RouteContentCreateUpdateContract GetRouteContent(int routeContentId);

        /// <summary>
        /// thêm, sửa route content
        /// </summary>
        /// <returns></returns>
        CUDReturnMessage CreateUpdateRouteContent(RouteContentCreateUpdateContract content);

        /// <summary>
        /// xóa route
        /// </summary>
        /// <returns></returns>
        CUDReturnMessage DeleteRoute(int routeId);


        #region FE

        /// <summary>
        /// lấy danh sách route theo microsite
        /// </summary>
        /// <returns></returns>
        List<RouteForFEContract> GetListRoute(int msId, string lang);

        #endregion FE
    }

    public class RouteMngtBusiness : BaseBusiness, IRouteMngtBusiness
    {
        private Lazy<IRouteDataAccess> lazyRouteDataAccess;

        public RouteMngtBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyRouteDataAccess = new Lazy<IRouteDataAccess>(() => new RouteDataAccess(appid, uid));
        }

        public CUDReturnMessage CreateRoute(RouteCreateContract route)
        {
            route.LangShortName = defaultLanguageCode;
            return lazyRouteDataAccess.Value.CreateRoute(route);
        }

        public CUDReturnMessage CreateUpdateRouteContent(RouteContentCreateUpdateContract content)
        {
            if (content.RouteContentId > 0)
            {
                return lazyRouteDataAccess.Value.UpdateRouteContent(content);
            }
            else
            {
                return lazyRouteDataAccess.Value.CreateRouteContent(content);
            }
        }

        public PagedList<RouteItemContract> GetListRoute(int msId, int page, int pageSize)
        {
            var query = lazyRouteDataAccess.Value.GetRoute(msId, 0, 0);
            var result = new PagedList<RouteItemContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderBy(r => r.MsId).ThenBy(r => r.Sort).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new RouteItemContract
                {
                    RouteId = r.RouteId,
                    MsId = r.MsId,
                    Status = r.Status,
                    RouteName = r.RouteContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.RouteName).FirstOrDefault()
                }).ToList();
            }

            return result;
        }

        public RouteDetailContract GetRouteDetail(int routeId)
        {
            var query = lazyRouteDataAccess.Value.GetRoute(0, routeId, 0);

            var detail = query.Select(r => new RouteDetailContract
            {
                RouteId = r.RouteId,
                MsId = r.MsId,
                CenterPosLatitude = r.CenterPosLatitude,
                CenterPosLongitude = r.CenterPosLongitude,
                FromLatitude = r.FromLatitude,
                FromLongitude = r.FromLongitude,
                ToLatitude = r.ToLatitude,
                ToLongitude = r.ToLongitude,
                ZoomLevel = r.ZoomLevel,
                Sort = r.Sort,
                Status = r.Status
            }).SingleOrDefault();

            if (detail == null) return null;

            detail.Contents = new List<RouteContentItemContract>();

            foreach (var lang in listApprovedLanguage)
            {
                var content = query.Select(item => item.RouteContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang))).First();
                if (content == null)
                {
                    detail.Contents.Add(new RouteContentItemContract
                    {
                        LangShortName = lang,
                        ApprovalStatus = 0,
                        RouteContentId = 0,
                        RouteName = ""
                    });
                }
                else
                {
                    detail.Contents.Add(new RouteContentItemContract
                    {
                        LangShortName = lang,
                        ApprovalStatus = content.ApprovalStatus,
                        RouteContentId = content.RouteContentId,
                        RouteName = content.RouteName
                    });
                }
            }

            return detail;
        }

        public CUDReturnMessage UpdateRoute(RouteUpdateContract route)
        {
            return lazyRouteDataAccess.Value.UpdateRoute(route);
        }

        public CUDReturnMessage DeleteRoute(int routeId)
        {
            return lazyRouteDataAccess.Value.DeleteRoute(routeId);
        }

        public RouteContentCreateUpdateContract GetRouteContent(int routeContentId)
        {
            var query = lazyRouteDataAccess.Value.GetRouteContent(routeContentId);

            return query.Select(r => new RouteContentCreateUpdateContract
            {
                RouteContentId = r.RouteContentId,
                RouteId = r.RouteId,
                RouteName = r.RouteName,
                TravelTime = r.TravelTime,
                Distance = r.Distance,
                ContactName = r.ContactName,
                ContactPhone = r.ContactPhone,
                LangShortName = r.LangShortName,
                ApprovalStatus = r.ApprovalStatus
            }).SingleOrDefault();
        }

        #region FE

        /// <summary>
        /// lấy danh sách route theo microsite
        /// </summary>
        /// <returns></returns>
        public List<RouteForFEContract> GetListRoute(int msId, string lang) {
            var query = lazyRouteDataAccess.Value.GetRoute(msId, 0, (int)ObjectStatus.Onsite );
            var result = new List<RouteForFEContract>();

            if (query.Any())
            {
                result = query.OrderBy(r => r.MsId).Select(r => new RouteForFEContract
                {
                    RouteId = r.RouteId,
                    MsId = r.MsId,
                    CenterPosLatitude = r.CenterPosLatitude,
                    CenterPosLongitude = r.CenterPosLongitude,
                    FromLatitude = r.FromLatitude,
                    FromLongitude = r.FromLongitude,
                    ToLatitude = r.ToLatitude,
                    ToLongitude = r.ToLongitude,
                    ZoomLevel = r.ZoomLevel,
                    Sort = r.Sort,
                    RouteDetail = r.RouteContents.Where(c => c.IsDeleted == false && c.LangShortName == lang && c.ApprovalStatus == (int)ApprovalStatus.Approved)
                                    .Select(c => new RouteDetailForFEContract() {
                                        RouteName = c.RouteName,
                                        TravelTime = c.TravelTime,
                                        Distance = c.Distance,
                                        ContactName = c.ContactName,
                                        ContactPhone = c.ContactPhone,
                                        LangShortName = c.LangShortName
                                    }).FirstOrDefault()
                }).ToList();
            }

            return result;
        }

        #endregion FE
    }
}
