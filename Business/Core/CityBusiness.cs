using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Contract.City;
using BMS.DataAccess.DAO;

namespace BMS.Business.Core
{
    public interface ICityBusiness
    {
        List<CityContract> GetListCity();
        List<DistrictContract> GetListDistrict(int cityId);
    }

    public class CityBusiness : BaseBusiness, ICityBusiness
    {
        private Lazy<CityDataAccess> lazyCityDataAccess;
        public CityBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyCityDataAccess = new Lazy<CityDataAccess>(() => new CityDataAccess(appid, uid));
        }

        public List<CityContract> GetListCity()
        {
            var query = lazyCityDataAccess.Value.GetListCity();

            return query.OrderBy(r => r.CityName).Select(r => new CityContract
            {
                CityId = r.CityId,
                Prefix = r.Prefix,
                CityName = r.CityName,
                CityNameEN = r.CityNameEN,
                ShortName = r.ShortName,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Priority = r.Priority.HasValue ? r.Priority.Value : int.MaxValue
            }).ToList();
        }

        public List<DistrictContract> GetListDistrict(int cityId)
        {
            var query = lazyCityDataAccess.Value.GetListDistrict(cityId);

            return query.OrderBy(r => r.DistrictName).Select(r => new DistrictContract
            {
                DistrictId = r.DistrictId,
                CityId = r.CityId,
                Prefix = r.Prefix,
                DistrictName = r.DistrictName,
                DistrictNameEN = r.DistrictNameEN,
                ShortName = r.ShortName,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Priority = r.Priority.HasValue ? r.Priority.Value : int.MaxValue
            }).ToList();
        }
    }
}
