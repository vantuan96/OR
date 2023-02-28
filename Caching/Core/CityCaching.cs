using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Business.Core;
using BMS.Contract.City;

namespace BMS.Caching.Core
{
    public interface ICityCaching
    {
        List<CityContract> GetListCity();
        List<DistrictContract> GetListDistrict(int cityId);
    }

    public class CityCaching : BaseCaching, ICityCaching
    {
        private Lazy<CityBusiness> lazyCityBusiness;
        public CityCaching(/*string appid, int uid*/)  
        {
            lazyCityBusiness = new Lazy<CityBusiness>(() => new CityBusiness(appid, uid));
        }

        public List<CityContract> GetListCity()
        {
            return lazyCityBusiness.Value.GetListCity();
        }

        public List<DistrictContract> GetListDistrict(int cityId)
        {
            return lazyCityBusiness.Value.GetListDistrict(cityId);
        }
    }
}
