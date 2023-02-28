using System;
using System.Collections.Generic;
using Business.Core;
using Contract.MasterData;
using Contract.Microsite;
using Contract.Shared;
using DataAccess.Models;
using static Caching.Core.MasterCaching;
using Contract.OR;

namespace Caching.Core
{
    public interface IMasterCaching
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

        #region System CheckList
        SearchSystemCheckList GetSystemCheckList(int userId, int state , string kw , int p , int ps ,int systemId=0, Boolean IsCheckRule = false);

        CUDReturnMessage DeleteSystemCheckList(int SystemId, int userId);
        CUDReturnMessage InsertUpdateSystemCheckList(SystemCheckListContract info, int userId);
        CUDReturnMessage AssignUserToSystemCheckList(AssignOwnerSystemContract info, int userId);
        #endregion

        #region cate,subcate
        SearchSubCateSystem GetSubCate(int state, string kw, int subCateId, int p, int ps);
        SearchCateSystem GetCate(int state, string kw, int CateId, int p, int ps);
        #endregion
    }

    public class MasterCaching : BaseCaching, IMasterCaching
    {
        private readonly Lazy<IMasterBusiness> lazyMasterBusiness;

        public MasterCaching()
        {
            lazyMasterBusiness = new Lazy<IMasterBusiness>(() => new MasterBusiness(appid, uid));
        }
        #region Blocktime
        public List<Blocktime_view> GetListBlocktime()
        {
            return lazyMasterBusiness.Value.GetListBlocktime();
        }
        public CUDReturnMessage InsertUpdateBlocktime(Blocktime_view data)
        {
            return lazyMasterBusiness.Value.InsertUpdateBlocktime(data);
        }
        public CUDReturnMessage DeleteBlocktime(int Id)
        {
            return lazyMasterBusiness.Value.DeleteBlocktime(Id);
        }

        #endregion
        #region PnL List
        public List<PnLListContract> GetListPnLList()
        {
            return lazyMasterBusiness.Value.GetListPnLList();
        }

        public CUDReturnMessage InsertUpdatePnLList(PnLListContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdatePnLList(data);
        }

        public CUDReturnMessage DeletePnLList(int PnLListId, int userId)
        {
            return lazyMasterBusiness.Value.DeletePnLList(PnLListId, userId);
        }

        public List<PnLListStatusContract> GetListPnLListStatus()
        {
            return lazyMasterBusiness.Value.GetListPnLListStatus();
        }

        #endregion

        #region PnL BU List
        public List<PnLBuListContract> GetListPnLBuList()
        {
            return lazyMasterBusiness.Value.GetListPnLBuList();
        }

        public CUDReturnMessage InsertUpdatePnLBuList(PnLBuListContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdatePnLBuList(data);
        }

        public CUDReturnMessage DeletePnLBuList(int PnLBuListId, int userId)
        {
            return lazyMasterBusiness.Value.DeletePnLBuList(PnLBuListId, userId);
        }

        #endregion

        #region Danh mục phòng ban bộ phận
        public List<DepartmentListContract> GetDepartmentList()
        {
            return lazyMasterBusiness.Value.GetDepartmentList();
        }

        public CUDReturnMessage InsertUpdateDepartmentList(DepartmentListContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdateDepartmentList(data);
        }

        public CUDReturnMessage DeleteDepartmentList(int DepartmentListId, int userId)
        {
            return lazyMasterBusiness.Value.DeleteDepartmentList(DepartmentListId, userId);
        }

        public List<DepartmentListTypeContract> GetDepartmentListType()
        {
            return lazyMasterBusiness.Value.GetDepartmentListType();
        }

        #endregion

        #region Danh mục nhân viên
        public List<StaffListContract> GetStaffList()
        {
            return lazyMasterBusiness.Value.GetStaffList();
        }

        public CUDReturnMessage InsertUpdateStaffList(StaffListContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdateStaffList(data);
        }

        public CUDReturnMessage DeleteStaffList(int StaffListId, int userId)
        {
            return lazyMasterBusiness.Value.DeleteStaffList(StaffListId, userId);
        }

        public List<DepartmentTitleContract> GetDepartmentTitle()
        {
            return lazyMasterBusiness.Value.GetDepartmentTitle();
        }

        public List<DepartmentStatusContract> GetDepartmentStatus()
        {
            return lazyMasterBusiness.Value.GetDepartmentStatus();
        }

        public List<DepartmentGeneralContract> GetDepartmentGeneral()
        {
            return lazyMasterBusiness.Value.GetDepartmentGeneral();
        }

        #endregion

        #region Danh mục nhóm thuộc tính BU
        public List<PnLBuAttributeGroupContract> GetPnLBuAttributeGroup()
        {
            return lazyMasterBusiness.Value.GetPnLBuAttributeGroup();
        }

        public CUDReturnMessage InsertUpdatePnLBuAttributeGroup(PnLBuAttributeGroupContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdatePnLBuAttributeGroup(data);
        }

        public CUDReturnMessage DeletePnLBuAttributeGroup(int PnLBuAttributeGroupId, int userId)
        {
            return lazyMasterBusiness.Value.DeletePnLBuAttributeGroup(PnLBuAttributeGroupId, userId);
        }

        #endregion

        #region Thuộc tính BU
        public List<PnLBuAttributeContract> GetPnLBuAttribute()
        {
            return lazyMasterBusiness.Value.GetPnLBuAttribute();
        }

        public CUDReturnMessage InsertUpdatePnLBuAttribute(PnLBuAttributeContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdatePnLBuAttribute(data);
        }

        public CUDReturnMessage DeletePnLBuAttribute(int pnlBuAttributeId, int userId)
        {
            return lazyMasterBusiness.Value.DeletePnLBuAttribute(pnlBuAttributeId, userId);
        }

        #endregion

        #region Vùng miền
        public List<RegionContract> GetRegion()
        {
            return lazyMasterBusiness.Value.GetRegion();
        }

        public CUDReturnMessage InsertUpdateRegion(RegionContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdateRegion(data);
        }

        public CUDReturnMessage DeleteRegion(int RegionId, int userId)
        {
            return lazyMasterBusiness.Value.DeleteRegion(RegionId, userId);
        }
        #endregion

        #region Đơn vị hành chính
        public List<DvhcContract> GetDvhc()
        {
            return lazyMasterBusiness.Value.GetDvhc();
        }

        public DvhcContract Find(int id)
        {
            return lazyMasterBusiness.Value.Find(id);
        }

        public CUDReturnMessage Delete(int id, int userId)
        {
            return lazyMasterBusiness.Value.Delete(id, userId);
        }

        public CUDReturnMessage InsertUpdateDvhc(DvhcContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdateDvhc(data);
        }

        public List<PrefixContract> GetPrefix()
        {
            return lazyMasterBusiness.Value.GetPrefix();
        }
        #endregion

        #region Danh mục nhóm cơ sở
        public List<BasisGroupContract> GetBasisGroup()
        {
            return lazyMasterBusiness.Value.GetBasisGroup();
        }

        public CUDReturnMessage InsertUpdateBasisGroup(BasisGroupContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdateBasisGroup(data);
        }

        public CUDReturnMessage DeleteBasisGroup(int LevelId, int userId)
        {
            return lazyMasterBusiness.Value.DeleteBasisGroup(LevelId, userId);
        }

        #endregion

        #region Danh mục cơ sở
        public List<BasisContract> GetBasis()
        {
            return lazyMasterBusiness.Value.GetBasis();
        }

        public CUDReturnMessage InsertUpdateBasis(BasisContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdateBasis(data);
        }

        public CUDReturnMessage DeleteBasis(int BasisId, int userId)
        {
            return lazyMasterBusiness.Value.DeleteBasis(BasisId, userId);
        }

        public List<BasisStatusContract> GetBasisStatus()
        {
            return lazyMasterBusiness.Value.GetBasisStatus();
        }
        #endregion

        #region Title
        public CUDReturnMessage InsertUpdateDepartmentTitle(DepartmentTitleContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdateDepartmentTitle(data);
        }

        public CUDReturnMessage DeleteDepartmentTitle(int departmentTitleId, int userId)
        {
            return lazyMasterBusiness.Value.DeleteDepartmentTitle(departmentTitleId, userId);
        }

        #endregion

        #region Level
        public List<LevelContract> GetLevel()
        {
            return lazyMasterBusiness.Value.GetLevel();
        }

        public CUDReturnMessage InsertUpdateLevel(LevelContract data)
        {
            return lazyMasterBusiness.Value.InsertUpdateLevel(data);
        }

        public CUDReturnMessage DeleteLevel(int LevelId, int userId)
        {
            return lazyMasterBusiness.Value.DeleteLevel(LevelId, userId);
        }

        public SearchSystemCheckList GetSystemCheckList(int userId,int state , string kw , int p , int ps, int systemId = 0, Boolean IsCheckRule = false)
        {
            return lazyMasterBusiness.Value.GetSystemCheckList(userId, state, kw, p, ps,systemId,IsCheckRule);
        }

        public CUDReturnMessage DeleteSystemCheckList(int SystemId, int userId)
        {
            return lazyMasterBusiness.Value.DeleteSystemCheckList(SystemId,userId);
        }

        public CUDReturnMessage InsertUpdateSystemCheckList(SystemCheckListContract info, int userId)
        {
            return lazyMasterBusiness.Value.InsertUpdateSystemCheckList(info,userId);
        }
        public CUDReturnMessage AssignUserToSystemCheckList(AssignOwnerSystemContract info, int userId)
        {
            return lazyMasterBusiness.Value.AssignUserToSystemCheckList(info,userId);
        }

        public SearchSubCateSystem GetSubCate(int state, string kw, int subCateId, int p, int ps)
        {
            return lazyMasterBusiness.Value.GetSubCate(state, kw, subCateId, p, ps);
        }

        public SearchCateSystem GetCate(int state, string kw, int CateId, int p, int ps)
        {
            return lazyMasterBusiness.Value.GetCate(state, kw, CateId, p, ps);
        }

        #endregion


    }
}
