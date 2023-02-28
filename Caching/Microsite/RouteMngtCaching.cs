using System;
using System.Collections.Generic;
using BMS.Business.Microsite;
using BMS.Contract.Route;
using BMS.Contract.Shared;

namespace BMS.Caching.Microsite
{
    public interface IRouteMngtCaching
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

    public class RouteMngtCaching : BaseCaching, IRouteMngtCaching
    {
        private Lazy<IRouteMngtBusiness> lazyRouteMngtBusiness;

        public RouteMngtCaching(/*string appid, int uid*/)  
        {
            lazyRouteMngtBusiness = new Lazy<IRouteMngtBusiness>(() => new RouteMngtBusiness(appid, uid));
        }

        public CUDReturnMessage CreateRoute(RouteCreateContract route)
        {
            return lazyRouteMngtBusiness.Value.CreateRoute(route);
        }

        public CUDReturnMessage CreateUpdateRouteContent(RouteContentCreateUpdateContract content)
        {
            return lazyRouteMngtBusiness.Value.CreateUpdateRouteContent(content);
        }

        public CUDReturnMessage DeleteRoute(int routeId)
        {
            return lazyRouteMngtBusiness.Value.DeleteRoute(routeId);
        }

        public PagedList<RouteItemContract> GetListRoute(int msId, int page, int pageSize)
        {
            return lazyRouteMngtBusiness.Value.GetListRoute(msId, page, pageSize);
        }

        public RouteContentCreateUpdateContract GetRouteContent(int routeContentId)
        {
            return lazyRouteMngtBusiness.Value.GetRouteContent(routeContentId);
        }

        public RouteDetailContract GetRouteDetail(int routeId)
        {
            return lazyRouteMngtBusiness.Value.GetRouteDetail(routeId);
        }

        public CUDReturnMessage UpdateRoute(RouteUpdateContract route)
        {
            return lazyRouteMngtBusiness.Value.UpdateRoute(route);
        }

        #region FE

        /// <summary>
        /// lấy danh sách route theo microsite
        /// </summary>
        /// <returns></returns>
        public List<RouteForFEContract> GetListRoute(int msId, string lang)
        {
            return lazyRouteMngtBusiness.Value.GetListRoute(msId, lang);
        }
        #endregion FE
    }
}
