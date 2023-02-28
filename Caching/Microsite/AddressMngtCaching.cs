using System;
using System.Collections.Generic;
using BMS.Business.Microsite;
using BMS.Contract.Address;
using BMS.Contract.Shared;

namespace BMS.Caching.Microsite
{
    public interface IAddressMngtCaching
    {
        PagedList<AddressContract> GetListAddress(int MsId, int pageSize, int page);
        AddressDetailContract GetAddressDetail(int id);
        AddressContentContract GetAddressContentById(int id);
        CUDReturnMessage CreateUpdateAddress(AddressCreateContract address);
        CUDReturnMessage CreateUpdateAddressContent(AddressContentContract address);
        CUDReturnMessage DeleteAddress(int id);

        #region FE

        List<Contract.FE.AddressContract> GetListOnsiteAddress(int MsId, string lang);

        #endregion FE
    }

    public class AddressMngtCaching : BaseCaching, IAddressMngtCaching
    {
        private Lazy<IAddressMngtBusiness> lazyAddressMngtBusiness;

        public AddressMngtCaching(/*string appid, int uid*/)  
        {
            lazyAddressMngtBusiness = new Lazy<IAddressMngtBusiness>(() => new AddressMngtBusiness(appid, uid));
        }

        public CUDReturnMessage CreateUpdateAddress(AddressCreateContract address)
        {
            return lazyAddressMngtBusiness.Value.CreateUpdateAddress(address);
        }

        public CUDReturnMessage CreateUpdateAddressContent(AddressContentContract address)
        {
            return lazyAddressMngtBusiness.Value.CreateUpdateAddressContent(address);
        }

        public CUDReturnMessage DeleteAddress(int id)
        {
            return lazyAddressMngtBusiness.Value.DeleteAddress(id);
        }

        public AddressContentContract GetAddressContentById(int id)
        {
            return lazyAddressMngtBusiness.Value.GetAddressContentById(id);
        }

        public AddressDetailContract GetAddressDetail(int id)
        {
            return lazyAddressMngtBusiness.Value.GetAddressDetail(id);
        }

        public PagedList<AddressContract> GetListAddress(int MsId, int pageSize, int page)
        {
            return lazyAddressMngtBusiness.Value.GetListAddress(MsId, pageSize, page);
        }

        #region FE

        public List<Contract.FE.AddressContract> GetListOnsiteAddress(int MsId, string lang)
        {
            return lazyAddressMngtBusiness.Value.GetListOnsiteAddress(MsId, lang);
        }

        #endregion FE
    }
}
