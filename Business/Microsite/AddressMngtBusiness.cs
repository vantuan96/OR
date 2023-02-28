using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Contract.Address;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;

namespace BMS.Business.Microsite
{
    public interface IAddressMngtBusiness
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
    public class AddressMngtBusiness : BaseBusiness, IAddressMngtBusiness
    {
        private Lazy<IAddressDataAccess> lazyAddressDataAccess;

        public AddressMngtBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAddressDataAccess = new Lazy<IAddressDataAccess>(() => new AddressDataAccess(appid, uid));
        }

        public CUDReturnMessage CreateUpdateAddress(AddressCreateContract address)
        {
            if (address.Id == 0)
            {
                return lazyAddressDataAccess.Value.CreateAddress(address, defaultLanguageCode);
            }
            else
                return lazyAddressDataAccess.Value.UpdateAddress(address);
        }

        public CUDReturnMessage CreateUpdateAddressContent(AddressContentContract address)
        {
            if (address.ContentId == 0)
                return lazyAddressDataAccess.Value.CreateAddressContent(address);
            else
                return lazyAddressDataAccess.Value.UpdateAddressContent(address);
        }

        public CUDReturnMessage DeleteAddress(int id)
        {
            return lazyAddressDataAccess.Value.DeleteAddress(id);
        }

        public AddressDetailContract GetAddressDetail(int id)
        {
            var query = lazyAddressDataAccess.Value.GetAddressById(id);
            
            var detail = query.Select(item => new AddressDetailContract
            {
                Id = item.Id,
                MsId = item.MsId,
                CityId = item.CityId,
                DistrictId = item.DistrictId,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                ApprovalStatus = item.ApprovalStatus,
                IsOnsite = item.IsOnsite,
                Sort = item.Sort,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                LastUpdatedBy = item.LastUpdatedBy,
                LastUpdatedDate = item.LastUpdatedDate,
                Email = item.Email,
                Fax = item.Fax,
                PhoneNumber1 = item.PhoneNumber1,
                PhoneNumber2 = item.PhoneNumber2,
            }).SingleOrDefault();

            if (detail == null) return null;

            detail.ListContent = new List<AddressContentContract>();

            foreach (var lang in listApprovedLanguage)
            {
                var content = query.Select(item => item.AddressContents.FirstOrDefault(r => r.IsDeleted == false && r.LangShortName.Equals(lang))).First();
                if (content == null)
                {
                    detail.ListContent.Add(new AddressContentContract
                    {
                        LangShortName = lang,
                        FullAddress = "",
                        Name = "",
                        AddressId = detail.Id,
                        ContentId = 0,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = 0, 
                        LastUpdatedDate = DateTime.Now
                    });
                }
                else
                {
                    detail.ListContent.Add(new AddressContentContract
                    {
                        LangShortName = lang,
                        FullAddress = content.FullAddress,
                        Name = content.Name,
                        ContentId = content.ContentId,
                        AddressId = content.AddressId,
                        CreatedBy = content.CreatedBy,
                        CreatedDate = content.CreatedDate,
                        LastUpdatedBy = content.LastUpdatedBy,
                        LastUpdatedDate = content.LastUpdatedDate
                    });
                }
            }

            return detail;
        }

        public PagedList<AddressContract> GetListAddress(int MsId, int pageSize, int page)
        {
            var query = lazyAddressDataAccess.Value.GetListAddress(MsId, null);
            var result = new PagedList<AddressContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderBy(r => r.Id).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new AddressContract
                {
                    Id = r.Id,
                    MsId = r.MsId,
                    CityId = r.CityId,
                    DistrictId = r.DistrictId,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude,
                    ApprovalStatus = r.ApprovalStatus,
                    IsOnsite = r.IsOnsite,
                    Sort = r.Sort,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    LastUpdatedBy = r.LastUpdatedBy,
                    LastUpdatedDate = r.LastUpdatedDate,
                    Email = r.Email,
                    Fax = r.Fax,
                    PhoneNumber1 = r.PhoneNumber1,
                    PhoneNumber2 = r.PhoneNumber2,
                    FullAddress = r.AddressContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.FullAddress).FirstOrDefault(),
                    Name = r.AddressContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault()
                }).ToList();
            }

            return result;
        }

        public AddressContentContract GetAddressContentById(int id)
        {
            var query = lazyAddressDataAccess.Value.GetAddressContentById(id);

            return query.Select(content => new AddressContentContract
            {
                AddressId = content.AddressId,
                ContentId = content.ContentId,
                LangShortName = content.LangShortName,
                FullAddress = content.FullAddress,
                Name = content.Name,
                CreatedBy = content.CreatedBy,
                CreatedDate = content.CreatedDate,
                LastUpdatedBy = content.LastUpdatedBy,
                LastUpdatedDate = content.LastUpdatedDate
            }).SingleOrDefault();
        }

        #region FE

        public List<Contract.FE.AddressContract> GetListOnsiteAddress(int MsId, string lang)
        {
            var query = lazyAddressDataAccess.Value.GetListAddress(MsId, true);

            return query.OrderBy(r => r.Sort).Select(r => new Contract.FE.AddressContract
            {
                Id = r.Id,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Email = r.Email,
                Fax = r.Fax,
                PhoneNumber1 = r.PhoneNumber1,
                PhoneNumber2 = r.PhoneNumber2,
                CityName = r.City.CityName,
                DistrictName = r.District.DistrictName,
                CityNameEN = r.City.CityNameEN,
                DistrictNameEN = r.District.DistrictNameEN,
                CityId = r.CityId,
                DistrictId = r.DistrictId,
                FullAddress = r.AddressContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.FullAddress).FirstOrDefault(),
                Name = r.AddressContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Name).FirstOrDefault()
            }).ToList();
        }

        #endregion FE
    }
}
