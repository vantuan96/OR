using System;
using System.Collections.Generic;
using System.Linq;
using Contract.MasterData;
using Contract.Shared;
using DataAccess.DAO;
using VG.Common;
using Contract.Enum;
using Contract.CheckList;
using DataAccess.Models;
using Contract.OR;

namespace Business.Core
{
    public interface IMasterBusiness
    {
        #region Blocktime
        List<Blocktime_view> GetListBlocktime();
        CUDReturnMessage InsertUpdateBlocktime(Blocktime_view data);
        CUDReturnMessage DeleteBlocktime(int Id);
        #endregion
        #region PnL List
        List<PnLListContract> GetListPnLList();
        CUDReturnMessage InsertUpdatePnLList(PnLListContract data);
        CUDReturnMessage DeletePnLList(int PnLListId, int userId);
        List<PnLListStatusContract> GetListPnLListStatus();
        #endregion

        #region PnL BU List
        List<PnLBuListContract> GetListPnLBuList();
        CUDReturnMessage InsertUpdatePnLBuList(PnLBuListContract data);
        CUDReturnMessage DeletePnLBuList(int PnLBuListId, int userId);
        #endregion

        #region Danh mục phòng ban bộ phận
        List<DepartmentListContract> GetDepartmentList();

        CUDReturnMessage InsertUpdateDepartmentList(DepartmentListContract data);

        CUDReturnMessage DeleteDepartmentList(int DepartmentListId, int userId);

        List<DepartmentListTypeContract> GetDepartmentListType();
        #endregion

        #region Danh mục nhân viên
        List<StaffListContract> GetStaffList();
        CUDReturnMessage InsertUpdateStaffList(StaffListContract data);
        CUDReturnMessage DeleteStaffList(int StaffListId, int userId);
        List<DepartmentTitleContract> GetDepartmentTitle();
        List<DepartmentStatusContract> GetDepartmentStatus();
        List<DepartmentGeneralContract> GetDepartmentGeneral();
        #endregion

        #region Danh mục nhóm thuộc tính BU
        List<PnLBuAttributeGroupContract> GetPnLBuAttributeGroup();
        CUDReturnMessage InsertUpdatePnLBuAttributeGroup(PnLBuAttributeGroupContract data);
        CUDReturnMessage DeletePnLBuAttributeGroup(int PnLBuAttributeGroupId, int userId);
        #endregion

        #region Thuộc tính BU
        List<PnLBuAttributeContract> GetPnLBuAttribute();
        CUDReturnMessage InsertUpdatePnLBuAttribute(PnLBuAttributeContract data);
        CUDReturnMessage DeletePnLBuAttribute(int pnlBuAttributeId, int userId);
        #endregion

        #region Vùng miền
        List<RegionContract> GetRegion();
        CUDReturnMessage InsertUpdateRegion(RegionContract data);
        CUDReturnMessage DeleteRegion(int RegionId, int userId);
        #endregion

        #region Đơn vị hành chính
        List<DvhcContract> GetDvhc();
        DvhcContract Find(int id);
        CUDReturnMessage Delete(int id, int userId);
        CUDReturnMessage InsertUpdateDvhc(DvhcContract data);
        List<PrefixContract> GetPrefix();
        #endregion

        #region Danh mục nhóm cơ sở
        List<BasisGroupContract> GetBasisGroup();
        CUDReturnMessage InsertUpdateBasisGroup(BasisGroupContract data);
        CUDReturnMessage DeleteBasisGroup(int LevelId, int userId);
        #endregion

        #region Danh mục cơ sở
        List<BasisContract> GetBasis();
        CUDReturnMessage InsertUpdateBasis(BasisContract data);
        CUDReturnMessage DeleteBasis(int BasisId, int userId);
        List<BasisStatusContract> GetBasisStatus();
        #endregion

        #region Title
        CUDReturnMessage InsertUpdateDepartmentTitle(DepartmentTitleContract data);
        CUDReturnMessage DeleteDepartmentTitle(int departmentTitleId, int userId);
        #endregion

        #region Level
        List<LevelContract> GetLevel();
        CUDReturnMessage InsertUpdateLevel(LevelContract data);
        CUDReturnMessage DeleteLevel(int LevelId, int userId);
        #endregion

        #region system checklist
        SearchSystemCheckList GetSystemCheckList(int userId, int state, string kw, int p, int ps, int systemId = 0, Boolean IsCheckRule = false);
        CUDReturnMessage DeleteSystemCheckList(int systemId, int userId);
        CUDReturnMessage InsertUpdateSystemCheckList(SystemCheckListContract info, int userId);
        CUDReturnMessage AssignUserToSystemCheckList(AssignOwnerSystemContract info, int userId);
        #endregion

        #region cate,subcate
        SearchSubCateSystem GetSubCate(int state, string kw, int subCateId, int p, int ps);
        SearchCateSystem GetCate(int state, string kw, int CateId, int p, int ps);
        #endregion


    }
    public class MasterBusiness : BaseBusiness, IMasterBusiness
    {
        private readonly Lazy<IMasterDataAccess> lazyMasterDataAccess;
        private Lazy<ILocationDataAccess> lazyLocation;
        public MasterBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyMasterDataAccess = new Lazy<IMasterDataAccess>(() => new MasterDataAccess(appid, uid));
            lazyLocation = new Lazy<ILocationDataAccess>(() => new LocationDataAccess(appid, uid));
        }
        #region Blocktime
        public List<Blocktime_view> GetListBlocktime()
        {
            var typeDoctor = lazyMasterDataAccess.Value.GetListBlocktime();
            return typeDoctor.Select(r => new Blocktime_view
            {
                id = r.id,
                MaDV = r.MaDV,
                TenDv = r.TenDv,
                CleaningTime = Convert.ToInt32(r.CleaningTime),
                AnesthesiaTime = Convert.ToInt32(r.AnesthesiaTime),
                PreparationTime = Convert.ToInt32(r.PreparationTime),
                OtherTime = Convert.ToInt32(r.OtherTime),
                Ehos_Iddv = Convert.ToInt32(r.Ehos_Iddv)
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateBlocktime(Blocktime_view data)
        {
            return lazyMasterDataAccess.Value.InsertUpdateBlocktime(data);
        }

        public CUDReturnMessage DeleteBlocktime(int Id)
        {
            return lazyMasterDataAccess.Value.DeleteBlockTime(Id);
        }

        #endregion
        #region PnL List
        public List<PnLListContract> GetListPnLList()
        {
            var pnlList = lazyMasterDataAccess.Value.GetListPnLList();
            var pnlListStatus = lazyMasterDataAccess.Value.GetListPnLListStatus();
            return pnlList.Select(r => new PnLListContract
            {
                PnLListId = r.PnLListId,
                PnLListCode = r.PnLListCode,
                PnLListName = r.PnLListName,
                Description = r.Description,
                FullAddress = r.FullAddress,
                StatusId = (int)r.Status,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
                StatusName = pnlListStatus.FirstOrDefault(x => x.PnLListStatusId == r.Status).PnLListStatusName
            }).ToList();
        }

        public CUDReturnMessage InsertUpdatePnLList(PnLListContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdatePnLList(data);
        }

        public CUDReturnMessage DeletePnLList(int PnLListId, int userId)
        {
            return lazyMasterDataAccess.Value.DeletePnLList(PnLListId, userId);
        }

        public List<PnLListStatusContract> GetListPnLListStatus()
        {
            var pnlListStatus = lazyMasterDataAccess.Value.GetListPnLListStatus();
            return pnlListStatus.Select(r => new PnLListStatusContract
            {
                StatusId = r.PnLListStatusId,
                StatusName = r.PnLListStatusName
            }).ToList();
        }
        #endregion

        #region PnL BU List
        public List<PnLBuListContract> GetListPnLBuList()
        {
            var pnlList = lazyMasterDataAccess.Value.GetListPnLList();
            var pnlBuList = lazyMasterDataAccess.Value.GetListPnLBuList();
            return pnlBuList.Select(r => new PnLBuListContract
            {
                PnLListId = (int)r.PnLListId,
                PnLBuListId = r.PnLBUListId,
                PnLBuListCode = r.PnLBUListCode,
                Description = r.Description,
                Sort = r.Sort,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
                PnLListCode = pnlList.FirstOrDefault(x => x.PnLListId == r.PnLListId).PnLListCode
            }).ToList();
        }

        public CUDReturnMessage InsertUpdatePnLBuList(PnLBuListContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdatePnLBuList(data);
        }

        public CUDReturnMessage DeletePnLBuList(int PnLBuListId, int userId)
        {
            return lazyMasterDataAccess.Value.DeletePnLBuList(PnLBuListId, userId);
        }

        #endregion

        #region Danh mục phòng ban bộ phận
        public List<DepartmentListContract> GetDepartmentList()
        {
            var departmentList = lazyMasterDataAccess.Value.GetDepartmentList();
            var departmentType = lazyMasterDataAccess.Value.GetDepartmentListType();
            return departmentList.Select(r => new DepartmentListContract
            {
                DepartmentListId = r.DepartmentListId,
                DepartmentListCode = r.DepartmentListCode,
                Description = r.Description,
                Type = (int)r.Type,
                ParentCode = r.ParentCode,
                Level = (int)r.Level,
                Sort = (int)r.Sort,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
                DepartmentTypeName = departmentType.FirstOrDefault(x => x.DepartmentTypeId == r.Type).DepartmentTypeName
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateDepartmentList(DepartmentListContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdateDepartmentList(data);
        }

        public CUDReturnMessage DeleteDepartmentList(int DepartmentListId, int userId)
        {
            return lazyMasterDataAccess.Value.DeleteDepartmentList(DepartmentListId, userId);
        }

        public List<DepartmentListTypeContract> GetDepartmentListType()
        {
            var departmentType = lazyMasterDataAccess.Value.GetDepartmentListType();
            return departmentType.Select(r => new DepartmentListTypeContract
            {
                DepartmentTypeId = r.DepartmentTypeId,
                DepartmentTypeName = r.DepartmentTypeName
            }).ToList();
        }

        #endregion

        #region Danh mục nhân viên
        public List<StaffListContract> GetStaffList()
        {
            var staffList = lazyMasterDataAccess.Value.GetStaffList();
            var departmentGeneral = lazyMasterDataAccess.Value.GetDepartmentGeneral();
            var departmentList = lazyLocation.Value.Get();
            var departmentTitle = lazyMasterDataAccess.Value.GetDepartmentTitle();
            var departmentStatus = lazyMasterDataAccess.Value.GetDepartmentStatus();

            return staffList.Select(r => new StaffListContract
            {
                StaffListId = r.StaffListId,
                StaffListCode = r.StaffListCode,
                FullName = r.FullName,
                General = Convert.ToInt32(r.General),
                Email = r.Email,
                PhoneNo = r.PhoneNo,
                UnitCodeId = Convert.ToInt32(r.UnitCodeId),
                CentreCodeId = Convert.ToInt32(r.CentreCodeId),
                DepartmentCodeId = Convert.ToInt32(r.DepartmentCodeId),
                GroupCodeId = Convert.ToInt32(r.GroupCodeId),
                OfficeLocation = r.OfficeLocation,
                CityCode = r.CityCode,
                TitleCodeId = Convert.ToInt32(r.TitleCodeId),
                LevelCode = r.LevelCode,
                StatusId = Convert.ToInt32(r.StatusId),
                ManagerCode = r.ManagerCode,
                BirthDate = Convert.ToDateTime(r.BirthDate),
                JoinCompanyDate = Convert.ToDateTime(r.JoinCompanyDate),
                Visible = Convert.ToBoolean(r.Visible),
                CreatedDate = Convert.ToDateTime(r.CreateDate),
                CreatedBy = Convert.ToInt32(r.CreateBy),
                UpdatedDate = Convert.ToDateTime(r.UpdateDate),
                UpdatedBy = Convert.ToInt32(r.UpdateBy),
                GeneralName = departmentGeneral.FirstOrDefault(x => x.DId == r.General).GeneralVN,
                UnitCode = departmentList.FirstOrDefault(x => x.LocationId == r.UnitCodeId).NameEN,
                CentreCode = departmentList.FirstOrDefault(x => x.LocationId == r.CentreCodeId).NameEN,
                DepartmentCode = departmentList.FirstOrDefault(x => x.LocationId == r.DepartmentCodeId).NameEN,
                GroupCode = departmentList.FirstOrDefault(x => x.LocationId == r.GroupCodeId).NameVN,
                TitleCode = departmentTitle.FirstOrDefault(x => x.DepartmentTitleId == r.TitleCodeId).DepartmentTitleName,
                StatusName = departmentStatus.FirstOrDefault(x => x.DepartmentStatusId == r.StaffListId).DepartmentStatusName,
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateStaffList(StaffListContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdateStaffList(data);
        }

        public CUDReturnMessage DeleteStaffList(int StaffListId, int userId)
        {
            return lazyMasterDataAccess.Value.DeleteStaffList(StaffListId, userId);
        }

        public List<DepartmentTitleContract> GetDepartmentTitle()
        {
            var departmentTitle = lazyMasterDataAccess.Value.GetDepartmentTitle();
            return departmentTitle.Select(r => new DepartmentTitleContract()
            {
                DepartmentTitleId = r.DepartmentTitleId,
                DepartmentTitleCode = r.DepartmentTitleCode,
                DepartmentTitleName = r.DepartmentTitleName,
                Description = r.Description,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
            }).ToList();
        }

        public List<DepartmentStatusContract> GetDepartmentStatus()
        {
            var departmentStatus = lazyMasterDataAccess.Value.GetDepartmentStatus();
            return departmentStatus.Select(r => new DepartmentStatusContract()
            {
                DepartmentStatusId = r.DepartmentStatusId,
                DepartmentStatusName = r.DepartmentStatusName
            }).ToList();
        }

        public List<DepartmentGeneralContract> GetDepartmentGeneral()
        {
            var departmentGeneral = lazyMasterDataAccess.Value.GetDepartmentGeneral();
            return departmentGeneral.Select(r => new DepartmentGeneralContract()
            {
                DId = r.DId,
                GeneralVN = r.GeneralVN,
                GeneralEN = r.GeneralEN
            }).ToList();
        }

        #endregion

        #region Danh mục nhóm thuộc tính BU
        public List<PnLBuAttributeGroupContract> GetPnLBuAttributeGroup()
        {
            var pnlList = lazyMasterDataAccess.Value.GetPnLBuAttributeGroup();
            return pnlList.Select(r => new PnLBuAttributeGroupContract
            {
                PnLBuAttributeGroupId = r.PnLBuAttributeGroupId,
                PnLBuAttributeGroupCode = r.PnLBuAttributeGroupCode,
                PnLBuAttributeGroupName = r.PnLBuAttributeGroupName,
                Description = r.Description,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
            }).ToList();
        }

        public CUDReturnMessage InsertUpdatePnLBuAttributeGroup(PnLBuAttributeGroupContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdatePnLBuAttributeGroup(data);
        }

        public CUDReturnMessage DeletePnLBuAttributeGroup(int PnLBuAttributeGroupId, int userId)
        {
            return lazyMasterDataAccess.Value.DeletePnLBuAttributeGroup(PnLBuAttributeGroupId, userId);
        }

        #endregion

        #region Thuộc tính BU
        public List<PnLBuAttributeContract> GetPnLBuAttribute()
        {
            var pnlList = lazyMasterDataAccess.Value.GetListPnLList();
            var pnlBuList = lazyMasterDataAccess.Value.GetListPnLBuList();
            var pnlAttributeGroup = lazyMasterDataAccess.Value.GetPnLBuAttributeGroup();

            var pnlBuAttribute = lazyMasterDataAccess.Value.GetPnLBuAttribute();
            return pnlBuAttribute.Select(r => new PnLBuAttributeContract
            {
                PnLBuAttributeId = r.PnLBuAttributeId,
                PnLBuAttributeCode = r.PnLBuAttributeCode,
                PnLBuAttributeName = r.PnLBuAttributeName,
                Description = r.Description,
                PnLListId = (int)r.PnLListId,
                PnLBUListId = (int)r.PnLBUListId,
                PnLAttributeGroupId = (int)r.PnLAttributeGroupId,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
                PnLListName = pnlList.FirstOrDefault(x => x.PnLListId == r.PnLListId).PnLListName,
                PnLBUListName = pnlBuList.FirstOrDefault(x => x.PnLBUListId == r.PnLBUListId).Description,
                PnLAttributeGroupName = pnlAttributeGroup.FirstOrDefault(x => x.PnLBuAttributeGroupId == r.PnLAttributeGroupId).PnLBuAttributeGroupName

            }).ToList();
        }

        public CUDReturnMessage InsertUpdatePnLBuAttribute(PnLBuAttributeContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdatePnLBuAttribute(data);
        }

        public CUDReturnMessage DeletePnLBuAttribute(int pnlBuAttributeId, int userId)
        {
            return lazyMasterDataAccess.Value.DeletePnLBuAttribute(pnlBuAttributeId, userId);
        }

        #endregion

        #region Vùng miền
        public List<RegionContract> GetRegion()
        {
            var region = lazyMasterDataAccess.Value.GetRegion();

            return region.Select(r => new RegionContract()
            {
                RegionId = r.RegionId,
                RegionCode = r.RegionCode,
                RegionName = r.RegionName,
                Description = r.Description,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateRegion(RegionContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdateRegion(data);
        }

        public CUDReturnMessage DeleteRegion(int RegionId, int userId)
        {
            return lazyMasterDataAccess.Value.DeleteRegion(RegionId, userId);
        }
        #endregion

        #region Đơn vị hành chính
        public List<DvhcContract> GetDvhc()
        {
            var data = lazyMasterDataAccess.Value.GetDvhc();
            return data.Select(x => new DvhcContract()
            {
                AdministrativeUnitsId = x.AdministrativeUnitsId,
                AdministrativeUnitsVN = x.AdministrativeUnitsVN,
                AdministrativeUnitsEN = x.AdministrativeUnitsEN,
                ParentId = (int)x.ParentId,
                LevelNo = (int)x.LevelNo,
                LevelPath = x.LevelPath,
                CreatedBy = (int)x.CreatedBy,
                CreatedDate = (DateTime)x.CreatedDate,
                UpdatedBy = (int)x.UpdatedBy,
                UpdatedDate = (DateTime)x.UpdatedDate,
                Visible = (bool)x.Visible,
                Prefix = x.Prefix,
                Latitude = (float)x.Latitude,
                Longitude = (float)x.Longitude
            }).ToList();
        }

        public DvhcContract Find(int id)
        {
            var x = lazyMasterDataAccess.Value.Find(id);
            return new DvhcContract()
            {
                AdministrativeUnitsId = x.AdministrativeUnitsId,
                AdministrativeUnitsVN = x.AdministrativeUnitsVN,
                AdministrativeUnitsEN = x.AdministrativeUnitsEN,
                ParentId = Convert.ToInt32(x.ParentId),
                LevelNo = Convert.ToInt32(x.LevelNo),
                LevelPath = x.LevelPath,
                CreatedBy = Convert.ToInt32(x.CreatedBy),
                CreatedDate = Convert.ToDateTime(x.CreatedDate),
                UpdatedBy = Convert.ToInt32(x.UpdatedBy),
                UpdatedDate = Convert.ToDateTime(x.UpdatedDate),
                Visible = Convert.ToBoolean(x.Visible),
                Prefix = x.Prefix,
                Latitude = (float)x.Latitude,
                Longitude = (float)x.Longitude
            };
        }

        public CUDReturnMessage Delete(int id, int userId)
        {
            return lazyMasterDataAccess.Value.Delete(id, userId);
        }

        public CUDReturnMessage InsertUpdateDvhc(DvhcContract data)
        {
            if (data.ParentId == 0)
            {
                data.LevelNo = 1;
                data.ParentId = 0;
            }
            else
            {
                var parent = this.Find(data.ParentId ?? 0);
                if (parent != null)
                {
                    data.LevelNo = parent.LevelNo + 1;
                    data.LevelPath = parent.LevelPath;
                }
                else
                {
                    return new CUDReturnMessage(ResponseCode.Error);
                }
            }
            return lazyMasterDataAccess.Value.InsertUpdateDvhc(data);
        }

        public List<PrefixContract> GetPrefix()
        {
            var prefix = lazyMasterDataAccess.Value.GetPrefix();
            return prefix.Select(r => new PrefixContract()
            {
                PrefixId = r.AUId,
                PrefixName = r.AUName,
                PrefixGroup = (int)r.AUGroup
            }).ToList();
        }

        #endregion

        #region Danh mục nhóm cơ sở
        public List<BasisGroupContract> GetBasisGroup()
        {
            var basisGroup = lazyMasterDataAccess.Value.GetBasisGroup();
            return basisGroup.Select(r => new BasisGroupContract()
            {
                BasisGroupId = r.BasisGroupId,
                BasisGroupCode = r.BasisGroupCode,
                BasisGroupName = r.BasisGroupName,
                Address = r.Address,
                Longitude = (float)r.Longitude,
                Latitude = (float)r.Latitude,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateBasisGroup(BasisGroupContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdateBasisGroup(data);
        }

        public CUDReturnMessage DeleteBasisGroup(int LevelId, int userId)
        {
            return lazyMasterDataAccess.Value.DeleteBasisGroup(LevelId, userId);
        }

        #endregion

        #region Danh mục cơ sở
        public List<BasisContract> GetBasis()
        {
            var pnlList = lazyMasterDataAccess.Value.GetListPnLList();
            var pnlBuList = lazyMasterDataAccess.Value.GetListPnLBuList();
            var basisStatus = lazyMasterDataAccess.Value.GetBasisStatus();
            var basisGroup = lazyMasterDataAccess.Value.GetBasisGroup();

            var basis = lazyMasterDataAccess.Value.GetBasis();
            return basis.Select(r => new BasisContract
            {
                BasisCode = r.BasisCode,
                BasisName = r.BasisName,
                BasisGroupId = (int)r.BasisGroupId,
                PnLListId = (int)r.PnLListId,
                PnLBUListId = (int)r.PnLBUListId,
                Description = r.Description,
                CityId = (int)r.CityId,
                DistrictId = (int)r.DistrictId,
                WardId = (int)r.WardId,
                RefCode = r.RefCode,
                StatusId = (int)r.StatusId,
                FullName = r.FullName,
                StatusDescription = r.StatusDescription,
                OpeningDate = (DateTime)r.OpeningDate,
                Latitude = (float)r.Latitude,
                Longitude = (float)r.Longitude,
                Address = r.Address,
                Manager = r.Manager,
                ManagerPhone = r.ManagerPhone,
                SitePhone = r.SitePhone,
                SiteEmail = r.SiteEmail,
                AreaManager = r.AreaManager,
                AreaManagerPhone = r.AreaManagerPhone,
                AreaManagerEmail = r.AreaManagerEmail,
                DepartmentId = (int)r.DepartmentId,
                StaffId = (int)r.StaffId,
                Visible = (bool)r.Visible,
                CreateBy = (int)r.CreateBy,
                CreateDate = (DateTime)r.CreateDate,
                UpdateBy = (int)r.UpdateBy,
                UpdateDate = (DateTime)r.UpdateDate,
                PnLListName = pnlList.FirstOrDefault(x => x.PnLListId == r.PnLListId).PnLListName,
                PnLBuListName = pnlBuList.FirstOrDefault(x => x.PnLBUListId == r.PnLBUListId).Description,
                StatusName = basisStatus.FirstOrDefault(x => x.BasisStatusId == r.StatusId).BasisStatusName,
                BasisGroupName = basisGroup.FirstOrDefault(x => x.BasisGroupId == r.BasisGroupId).BasisGroupName,
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateBasis(BasisContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdateBasis(data);
        }

        public CUDReturnMessage DeleteBasis(int BasisId, int userId)
        {
            return lazyMasterDataAccess.Value.DeleteBasis(BasisId, userId);
        }

        public List<BasisStatusContract> GetBasisStatus()
        {
            var basisStatus = lazyMasterDataAccess.Value.GetBasisStatus();
            return basisStatus.Select(r => new BasisStatusContract()
            {
                BasisStatusId = r.BasisStatusId,
                BasisStatusName = r.BasisStatusName
            }).ToList();
        }

        #endregion

        #region Title
        public CUDReturnMessage InsertUpdateDepartmentTitle(DepartmentTitleContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdateDepartmentTitle(data);
        }

        public CUDReturnMessage DeleteDepartmentTitle(int departmentTitleId, int userId)
        {
            return lazyMasterDataAccess.Value.DeleteDepartmentTitle(departmentTitleId, userId);
        }

        #endregion

        #region Level
        public List<LevelContract> GetLevel()
        {
            var level = lazyMasterDataAccess.Value.GetLevel();
            return level.Select(r => new LevelContract()
            {
                LevelId = r.LevelId,
                LevelCode = r.LevelCode,
                LevelName = r.LevelName,
                Description = r.Description,
                Visible = (bool)r.Visible,
                CreatedDate = (DateTime)r.CreateDate,
                CreatedBy = (int)r.CreateBy,
                UpdatedDate = (DateTime)r.UpdateDate,
                UpdatedBy = (int)r.UpdateBy,
            }).ToList();
        }

        public CUDReturnMessage InsertUpdateLevel(LevelContract data)
        {
            return lazyMasterDataAccess.Value.InsertUpdateLevel(data);
        }

        public CUDReturnMessage DeleteLevel(int LevelId, int userId)
        {
            return lazyMasterDataAccess.Value.DeleteLevel(LevelId, userId);
        }


        #endregion

        #region system checklist
        public SearchSystemCheckList GetSystemCheckList(int userId, int state, string kw, int p, int ps, int systemId = 0, Boolean IsCheckRule = false)
        {
            SearchSystemCheckList result = new SearchSystemCheckList() { Data = new List<SystemCheckListContract>(), TotalRows = 0 };
            var query = lazyMasterDataAccess.Value.GetSystemCheckList(userId,state, kw, systemId, IsCheckRule);

            if (query != null && query.Any())
            {
                result.TotalRows = query.Count();
                var data = query.OrderByDescending(c => c.CreatedDate)
                            .ThenBy(c => c.Priority)
                            .ThenByDescending(c => c.SystemName)
                            .Skip((p - 1) * ps)
                            .Take(ps).ToList();
                result.Data = data.Select(c => new SystemCheckListContract()
                {
                    SystemId = c.SystemId,
                    SystemName = c.SystemName,
                    Description = c.Description,
                    Visible = c.Visible ?? false,
                    CreatedBy = c.CreatedBy ?? 0,
                    CreatedDate = c.CreatedDate ?? DateTime.Now,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedDate = c.UpdatedDate,
                    StateName = EnumExtension.GetDescription((SystemStatusEnum)c.State),
                    Users = c.AdminUser_System.Where(i => i.IsDeleted == false).Select(d => new UserContract() { UserName = d.AdminUser.Username, UserId = d.UId, PhoneNumber = d.AdminUser.PhoneNumber, IsOwner = d.IsOwner, FullName = d.AdminUser.Name, Email = d.AdminUser.Email }).ToList(),
                    // add more info
                    Code = c.Code,
                    PlId = c.PlId??0,
                    SystemNameEn = c.SystemNameEn,
                    CateId = c.CateId,
                    SubCateId = c.SubCateId,
                    FunctionOverview = c.FunctionOverview
                    ,
                    ProviderName = c.ProviderName
                    ,
                    OriginTypeId = c.OriginTypeId
                    ,
                    Platform = c.Platform
                    ,
                    AppTypeId = c.AppTypeId
                    ,
                    IsSAP = c.IsSAP
                    ,
                    State = c.State
                    ,
                    UrlSystem = c.UrlSystem
                    ,
                    AuthenticationMethodId = c.AuthenticationMethodId
                    ,
                    RankId = c.RankId
                    ,
                    RTOTypeId = c.RTOTypeId
                    ,
                    RPOTypeId = c.RPOTypeId
                    ,
                    RLO = c.RLO ?? 0
                    ,
                    DRTypeId = c.DRTypeId
                    ,
                    LastDateDRTest = c.LastDateDRTest ?? DateTime.Now
                    ,
                    ScStateId = c.ScStateId
                    ,
                    SME = c.SME
                    ,
                    OwingBusinessUnit = c.OwingBusinessUnit
                    ,
                    ITContact = c.ITContact
                    ,
                    YearImplement = c.YearImplement??0
                    ,
                    QuantityUserActive = c.QuantityUserActive
                    ,
                    ConCurrentUser = c.ConCurrentUser
                    ,
                    BusinessHour = c.BusinessHour
                    ,
                    BusinessIssue = c.BusinessIssue
                    ,
                    TechIssue = c.TechIssue
                    ,
                    SystemMaintainTime = c.SystemMaintainTime
                    ,
                    IsDevTest = c.IsDevTest
                    ,
                    HostingLocation = c.HostingLocation
                    ,
                    IsReplace = c.IsReplace
                    ,
                    ReplaceBy = c.ReplaceBy??0
                    ,
                    DetailReplaceBy = c.DetailReplaceBy
                    ,
                    IsRequirementSecurity = c.IsRequirementSecurity
                    ,
                    IsRequirementSecurityDesign = c.IsRequirementSecurityDesign
                    ,
                    IsCheckCertification = c.IsCheckCertification
                    ,
                    IsCheckSecurityByGolive = c.IsCheckSecurityByGolive
                    ,
                    IsCheckRisk = c.IsCheckRisk
                    ,
                    SecurityStateId = c.SecurityStateId
                    ,
                    PerformanceId = c.PerformanceId
                    ,
                    PerformanceNote = c.PerformanceNote,
                    PLName= c.PnLList!=null?c.PnLList.PnLListName:string.Empty,
                    CateName=c.CateSystem!=null?c.CateSystem.CateName:string.Empty,
                    SubCateName= c.SubCateSystem!=null?c.SubCateSystem.SubCateName:string.Empty

                }).ToList();
            }
            return result;
        }

        public CUDReturnMessage DeleteSystemCheckList(int systemId, int userId)
        {
            return lazyMasterDataAccess.Value.DeleteSystemCheckList(systemId, userId);
        }

        public CUDReturnMessage InsertUpdateSystemCheckList(SystemCheckListContract info, int userId)
        {
            return lazyMasterDataAccess.Value.InsertUpdateSystemCheckList(info,userId);
        }

        public SearchSubCateSystem GetSubCate(int state, string kw, int subCateId, int p, int ps)
        {
            SearchSubCateSystem result = new SearchSubCateSystem() { Data = new List<SubCateSystemContract>(), TotalRows = 0 };
            var query = lazyMasterDataAccess.Value.GetSubCate(state, kw, subCateId);

            if (query != null && query.Any())
            {
                result.TotalRows = query.Count();
                var data = query.OrderBy(c => c.SubCateId)
                            .ThenBy(c => c.SubCateName)
                            .Skip((p - 1) * ps)
                            .Take(ps).ToList();
                result.Data = data.Select(c => new SubCateSystemContract()
                {
                    SubCateId = c.SubCateId,
                    SubCateName = c.SubCateName,
                    Visible = c.Visible ?? false,
                    CreatedBy = c.CreatedBy ?? 0,
                    CreatedDate = c.CreatedDate ?? DateTime.Now,
                    StateName = EnumExtension.GetDescription((SystemStateEnum)((c.Visible ?? true) ? SystemStateEnum.Active : SystemStateEnum.NoActive)),
                }).ToList();
            }
            return result;
        }

        public SearchCateSystem GetCate(int state, string kw, int CateId, int p, int ps)
        {
            SearchCateSystem result = new SearchCateSystem() { Data = new List<CateSystemContract>(), TotalRows = 0 };
            var query = lazyMasterDataAccess.Value.GetCate(state, kw, CateId);

            if (query != null && query.Any())
            {
                result.TotalRows = query.Count();
                var data = query.OrderBy(c => c.CateId)
                            .ThenBy(c => c.CateName)
                            .Skip((p - 1) * ps)
                            .Take(ps).ToList();
                result.Data = data.Select(c => new CateSystemContract()
                {
                    CateId = c.CateId,
                    CateName = c.CateName,
                    Visible = c.Visible,
                    CreatedBy = c.CreatedBy,
                    CreatedDate = c.CreatedDate,
                    StateName = EnumExtension.GetDescription((SystemStateEnum)((c.Visible) ? SystemStateEnum.Active : SystemStateEnum.NoActive))






                }).ToList();
            }
            return result;
        }

        public CUDReturnMessage AssignUserToSystemCheckList(AssignOwnerSystemContract info, int userId)
        {
            return lazyMasterDataAccess.Value.AssignUserToSystemCheckList(info,userId);
        }

        #endregion
    }
}
