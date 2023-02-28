using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Contract.MasterData;
using Contract.Shared;
using DataAccess.Models;
using Contract.Enum;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Blocktime_view = Contract.OR.Blocktime_view;
namespace DataAccess.DAO
{
    public interface IMasterDataAccess
    {
        #region Blocktime
        List<BlockTime> GetListBlocktime();
        CUDReturnMessage InsertUpdateBlocktime(Blocktime_view data);
        CUDReturnMessage DeleteBlockTime(int Id);
        #endregion
        #region PnL List
        List<PnLList> GetListPnLList();

        CUDReturnMessage InsertUpdatePnLList(PnLListContract data);

        CUDReturnMessage DeletePnLList(int pnLListId, int userId);

        IQueryable<PnLListStatu> GetListPnLListStatus();
        #endregion

        #region Danh mục PnL BU List
        List<PnLBUList> GetListPnLBuList();
        CUDReturnMessage InsertUpdatePnLBuList(PnLBuListContract data);

        CUDReturnMessage DeletePnLBuList(int pnLListId, int userId);
        #endregion

        #region Danh mục phòng ban bộ phận
        List<DepartmentList> GetDepartmentList();

        CUDReturnMessage InsertUpdateDepartmentList(DepartmentListContract data);

        CUDReturnMessage DeleteDepartmentList(int DepartmentListId, int userId);

        List<DepartmentType> GetDepartmentListType();
        #endregion

        #region Danh mục nhân viên
        List<StaffList> GetStaffList();

        CUDReturnMessage InsertUpdateStaffList(StaffListContract data);

        CUDReturnMessage DeleteStaffList(int StaffListId, int userId);

        List<DepartmentTitle> GetDepartmentTitle();
        List<DepartmentStatu> GetDepartmentStatus();
        List<DepartmentGeneral> GetDepartmentGeneral();
        #endregion

        #region Danh mục Nhóm thuộc tính BU
        List<PnLBuAttributeGroup> GetPnLBuAttributeGroup();

        CUDReturnMessage InsertUpdatePnLBuAttributeGroup(PnLBuAttributeGroupContract data);

        CUDReturnMessage DeletePnLBuAttributeGroup(int pnLBuAttributeGroupId, int userId);

        #endregion

        #region Thuộc tính BU
        List<PnLBuAttribute> GetPnLBuAttribute();

        CUDReturnMessage InsertUpdatePnLBuAttribute(PnLBuAttributeContract data);

        CUDReturnMessage DeletePnLBuAttribute(int pnlBuAttributeId, int userId);

        #endregion

        #region Vùng miền
        List<Region> GetRegion();

        CUDReturnMessage InsertUpdateRegion(RegionContract data);

        CUDReturnMessage DeleteRegion(int regionId, int userId);
        #endregion

        #region Đơn vị hành chính
        IQueryable<AdministrativeUnit> GetDvhc();
        AdministrativeUnit Find(int id);
        CUDReturnMessage Delete(int id, int userId);
        CUDReturnMessage InsertUpdateDvhc(DvhcContract data);
        List<AdministrativePrefix> GetPrefix();
        #endregion

        #region Danh mục nhóm cơ sở
        List<BasisGroup> GetBasisGroup();
        CUDReturnMessage InsertUpdateBasisGroup(BasisGroupContract data);
        CUDReturnMessage DeleteBasisGroup(int basisGroupId, int userId);
        #endregion

        #region Danh mục cơ sở
        List<Basis> GetBasis();

        CUDReturnMessage InsertUpdateBasis(BasisContract data);

        CUDReturnMessage DeleteBasis(int BasisId, int userId);
        List<BasisStatu> GetBasisStatus();
        #endregion

        #region Title

        CUDReturnMessage InsertUpdateDepartmentTitle(DepartmentTitleContract data);

        CUDReturnMessage DeleteDepartmentTitle(int departmentTitleId, int userId);
        #endregion

        #region Level
        List<Level> GetLevel();
        CUDReturnMessage InsertUpdateLevel(LevelContract data);
        CUDReturnMessage DeleteLevel(int levelId, int userId);
        #endregion

        #region system checklist
        /// <summary>
        /// Tra cứu danh sách hệ thống checklist
        /// </summary>
        /// <param name="state"></param>
        /// <param name="kw"></param>
        /// <returns></returns>
        IQueryable<SystemCheckList> GetSystemCheckList(int userId, int state = 0, string kw = "", int systemId = 0, Boolean IsCheckRule = false);
        /// <summary>
        /// Xoa he thong chi dinh
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteSystemCheckList(int systemId, int userId);
        /// <summary>
        /// Tạo mới/ Cập nhật thông tin hệ thống
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateSystemCheckList(SystemCheckListContract info,int userId);
        CUDReturnMessage AssignUserToSystemCheckList(AssignOwnerSystemContract info,int userId);
        #endregion

        #region cate,subcate
        IQueryable<SubCateSystem> GetSubCate(int state, string kw, int subCateId);
        IQueryable<CateSystem> GetCate(int state, string kw, int CateId);
        #endregion
    }
    public class MasterDataAccess : BaseDataAccess, IMasterDataAccess
    {
        public MasterDataAccess(string appid, int uid, string connStr = "ConnString.WebPortal") : base(appid, uid, connStr)
        {
        }
        #region Blocktime

        public List<BlockTime> GetListBlocktime()
        {
            var query = DbContext.BlockTimes.ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdateBlocktime(Blocktime_view data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            if (data.id == 0) //Innsert
            {
                var newBlocktime = new BlockTime()
                {
                    MaDV = data.MaDV,
                    TenDv = data.TenDv,
                    CleaningTime = data.CleaningTime,
                    PreparationTime = data.PreparationTime,
                    AnesthesiaTime = data.AnesthesiaTime,
                    OtherTime = data.OtherTime,
                    Ehos_Iddv = data.Ehos_Iddv
                };
                DbContext.BlockTimes.Add(newBlocktime);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.Successed);
            }
            //Update
            var typeBlocktime = DbContext.BlockTimes.SingleOrDefault(r => r.id == data.id);
            if (typeBlocktime == null)
                return new CUDReturnMessage(ResponseCode.Error);

            typeBlocktime.MaDV = data.MaDV;
            typeBlocktime.TenDv = data.TenDv;
            typeBlocktime.CleaningTime = data.CleaningTime;
            typeBlocktime.PreparationTime = data.PreparationTime;
            typeBlocktime.AnesthesiaTime = data.AnesthesiaTime;
            typeBlocktime.OtherTime = data.OtherTime;
            typeBlocktime.Ehos_Iddv = data.Ehos_Iddv;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Successed);
        }

        public CUDReturnMessage DeleteBlockTime(int Id)
        {
            var typeBlocktime = DbContext.BlockTimes.FirstOrDefault(r => r.id == Id);
            if (typeBlocktime == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            //typeBlocktime.Active = false;
            //typeBlocktime.UpdateBy = userId;
            //typeBlocktime.UpdateDate = DateTime.Now;
            //DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Successed);
        }
        #endregion
        #region Danh mục PnL List
        public List<PnLList> GetListPnLList()
        {
            var query = DbContext.PnLLists.Where(x => (bool)x.Visible).ToList();
            return query;
        }
        public CUDReturnMessage InsertUpdatePnLList(PnLListContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.PnLLists.SingleOrDefault(x => x.PnLListId != data.PnLListId && x.PnLListName == data.PnLListName && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.PnLList_DuplicateName);
            }
            var checkRowCode = DbContext.PnLLists.SingleOrDefault(x => x.PnLListId != data.PnLListId && x.PnLListCode == data.PnLListCode && (bool)x.Visible);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.PnLList_DuplicateCode);
            }
            #endregion

            if (data.PnLListId == 0) //Innsert
            {
                var newPnLList = new PnLList()
                {
                    PnLListCode = data.PnLListCode,
                    PnLListName = data.PnLListName,
                    Description = data.Description,
                    FullAddress = data.FullAddress,
                    Status = data.StatusId,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.PnLLists.Add(newPnLList);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.PnLList_SuccessCreate);
            }
            //Update
            var pnlList = DbContext.PnLLists.SingleOrDefault(r => r.PnLListId == data.PnLListId);
            if (pnlList == null)
                return new CUDReturnMessage(ResponseCode.Error);

            pnlList.PnLListCode = data.PnLListCode;
            pnlList.PnLListName = data.PnLListName;
            pnlList.Description = data.Description;
            pnlList.FullAddress = data.FullAddress;
            pnlList.Status = data.StatusId;
            pnlList.Visible = data.Visible;
            pnlList.UpdateBy = data.UpdatedBy;
            pnlList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.PnLList_SuccessUpdate);
        }
        public CUDReturnMessage DeletePnLList(int pnLListId, int userId)
        {
            var pnLList = DbContext.PnLLists.FirstOrDefault(r => r.PnLListId == pnLListId);
            if (pnLList == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            pnLList.Visible = false;
            pnLList.UpdateBy = userId;
            pnLList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.PnLList_SuccessDelete);
        }
        public IQueryable<PnLListStatu> GetListPnLListStatus()
        {
            var query = DbContext.PnLListStatus;
            return query;
        }

        #endregion

        #region Danh mục PnL BU List
        public List<PnLBUList> GetListPnLBuList()
        {
            var query = DbContext.PnLBULists.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdatePnLBuList(PnLBuListContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.PnLBULists.SingleOrDefault(x => x.PnLBUListId != data.PnLListId && x.PnLBUListCode == data.PnLBuListCode && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.PnLBuList_DuplicateCode);
            }

            #endregion

            if (data.PnLBuListId == 0) //Innsert
            {
                var newPnLBuList = new PnLBUList()
                {
                    PnLBUListCode = data.PnLBuListCode,
                    Description = data.Description,
                    PnLListId = data.PnLListId,
                    Sort = data.Sort,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.PnLBULists.Add(newPnLBuList);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.PnLBuList_SuccessCreate);
            }
            //Update
            var pnlBuList = DbContext.PnLBULists.SingleOrDefault(r => r.PnLBUListId == data.PnLBuListId);
            if (pnlBuList == null)
                return new CUDReturnMessage(ResponseCode.Error);

            pnlBuList.PnLBUListCode = data.PnLBuListCode;
            pnlBuList.Description = data.Description;
            pnlBuList.PnLListId = data.PnLListId;
            pnlBuList.Sort = data.Sort;
            pnlBuList.Visible = data.Visible;
            pnlBuList.UpdateBy = data.UpdatedBy;
            pnlBuList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.PnLBuList_SuccessUpdate);
        }

        public CUDReturnMessage DeletePnLBuList(int pnLBuListId, int userId)
        {
            var pnLBuList = DbContext.PnLBULists.FirstOrDefault(r => r.PnLBUListId == pnLBuListId);
            if (pnLBuList == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            pnLBuList.Visible = false;
            pnLBuList.UpdateBy = userId;
            pnLBuList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.PnLBuList_SuccessDelete);
        }

        #endregion

        #region Danh mục phòng ban bộ phận
        public List<DepartmentList> GetDepartmentList()
        {
            var query = DbContext.DepartmentLists.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdateDepartmentList(DepartmentListContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            if (data.DepartmentListId == 0) //Innsert
            {
                var newDepartmentList = new DepartmentList
                {
                    DepartmentListCode = data.DepartmentListCode,
                    Description = data.Description,
                    Type = data.Type,
                    ParentCode = data.ParentCode,
                    Level = data.Level,
                    Sort = data.Sort,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.DepartmentLists.Add(newDepartmentList);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.DepartmentList_SuccessCreate);
            }
            //Update
            var departmentList = DbContext.DepartmentLists.SingleOrDefault(r => r.DepartmentListId == data.DepartmentListId);
            if (departmentList == null)
                return new CUDReturnMessage(ResponseCode.Error);
            departmentList.DepartmentListCode = data.DepartmentListCode;
            departmentList.Description = data.Description;
            departmentList.Type = data.Type;
            departmentList.ParentCode = data.ParentCode;
            departmentList.Level = data.Level;
            departmentList.Sort = data.Sort;
            departmentList.Visible = data.Visible;
            departmentList.UpdateBy = data.UpdatedBy;
            departmentList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.DepartmentList_SuccessUpdate);
        }

        public CUDReturnMessage DeleteDepartmentList(int DepartmentListId, int userId)
        {
            var dList = DbContext.DepartmentLists.FirstOrDefault(r => r.DepartmentListId == DepartmentListId);
            if (dList == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            dList.Visible = false;
            dList.UpdateBy = userId;
            dList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.DepartmentList_SuccessDelete);
        }

        public List<DepartmentType> GetDepartmentListType()
        {
            var query = DbContext.DepartmentTypes.ToList();
            return query;
        }

        #endregion

        #region Danh mục nhân viên
        public List<StaffList> GetStaffList()
        {
            var query = DbContext.StaffLists.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdateStaffList(StaffListContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.StaffLists.SingleOrDefault(x => x.StaffListId != data.StaffListId && x.StaffListCode == data.StaffListCode && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.StaffList_DuplicateCode);
            }

            #endregion

            if (data.StaffListId == 0) //Innsert
            {
                var newStaffList = new StaffList()
                {
                    StaffListCode = data.StaffListCode,
                    FullName = data.FullName,
                    General = data.General,
                    Email = data.Email,
                    PhoneNo = data.PhoneNo,
                    UnitCodeId = data.UnitCodeId,
                    CentreCodeId = data.CentreCodeId,
                    DepartmentCodeId = data.DepartmentCodeId,
                    GroupCodeId = data.GroupCodeId,
                    OfficeLocation = data.OfficeLocation,
                    CityCode = data.CityCode,
                    TitleCodeId = data.TitleCodeId,
                    LevelCode = data.LevelCode,
                    StatusId = data.StatusId,
                    ManagerCode = data.ManagerCode,
                    BirthDate = data.BirthDate,
                    JoinCompanyDate = data.JoinCompanyDate,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.StaffLists.Add(newStaffList);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.StaffList_SuccessCreate);
            }
            //Update
            var staffList = DbContext.StaffLists.SingleOrDefault(r => r.StaffListId == data.StaffListId);
            if (staffList == null)
                return new CUDReturnMessage(ResponseCode.Error);
            staffList.StaffListCode = data.StaffListCode;
            staffList.FullName = data.FullName;
            staffList.General = data.General;
            staffList.Email = data.Email;
            staffList.PhoneNo = data.PhoneNo;
            staffList.UnitCodeId = data.UnitCodeId;
            staffList.CentreCodeId = data.CentreCodeId;
            staffList.DepartmentCodeId = data.DepartmentCodeId;
            staffList.GroupCodeId = data.GroupCodeId;
            staffList.OfficeLocation = data.OfficeLocation;
            staffList.CityCode = data.CityCode;
            staffList.TitleCodeId = data.TitleCodeId;
            staffList.LevelCode = data.LevelCode;
            staffList.StatusId = data.StatusId;
            staffList.ManagerCode = data.ManagerCode;
            staffList.BirthDate = data.BirthDate;
            staffList.JoinCompanyDate = data.JoinCompanyDate;
            staffList.Visible = data.Visible;
            staffList.UpdateBy = data.UpdatedBy;
            staffList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.StaffList_SuccessUpdate);
        }

        public CUDReturnMessage DeleteStaffList(int StaffListId, int userId)
        {
            var staffList = DbContext.StaffLists.FirstOrDefault(r => r.StaffListId == StaffListId);
            if (staffList == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            staffList.Visible = false;
            staffList.UpdateBy = userId;
            staffList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.StaffList_SuccessDelete);
        }

        public List<DepartmentTitle> GetDepartmentTitle()
        {
            var query = DbContext.DepartmentTitles.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public List<DepartmentStatu> GetDepartmentStatus()
        {
            var query = DbContext.DepartmentStatus.ToList();
            return query;
        }

        public List<DepartmentGeneral> GetDepartmentGeneral()
        {
            var query = DbContext.DepartmentGenerals.ToList();
            return query;
        }

        #endregion

        #region Danh mục nhóm thuộc tính BU
        public List<PnLBuAttributeGroup> GetPnLBuAttributeGroup()
        {
            var query = DbContext.PnLBuAttributeGroups.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdatePnLBuAttributeGroup(PnLBuAttributeGroupContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.PnLBuAttributeGroups.SingleOrDefault(x => x.PnLBuAttributeGroupId != data.PnLBuAttributeGroupId && x.PnLBuAttributeGroupName == data.PnLBuAttributeGroupName && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.PBAG_DuplicateName);
            }
            var checkRowCode = DbContext.PnLBuAttributeGroups.SingleOrDefault(x => x.PnLBuAttributeGroupId != data.PnLBuAttributeGroupId && x.PnLBuAttributeGroupCode == data.PnLBuAttributeGroupCode && (bool)x.Visible);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.PBAG_DuplicateCode);
            }
            #endregion

            if (data.PnLBuAttributeGroupId == 0) //Innsert
            {
                var newPnLList = new PnLBuAttributeGroup()
                {
                    PnLBuAttributeGroupCode = data.PnLBuAttributeGroupCode,
                    PnLBuAttributeGroupName = data.PnLBuAttributeGroupName,
                    Description = data.Description,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.PnLBuAttributeGroups.Add(newPnLList);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.PBAG_SuccessCreate);
            }
            //Update
            var pnlList = DbContext.PnLBuAttributeGroups.SingleOrDefault(r => r.PnLBuAttributeGroupId == data.PnLBuAttributeGroupId);
            if (pnlList == null)
                return new CUDReturnMessage(ResponseCode.Error);

            pnlList.PnLBuAttributeGroupCode = data.PnLBuAttributeGroupCode;
            pnlList.PnLBuAttributeGroupName = data.PnLBuAttributeGroupName;
            pnlList.Description = data.Description;
            pnlList.Visible = data.Visible;
            pnlList.UpdateBy = data.UpdatedBy;
            pnlList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.PBAG_SuccessUpdate);
        }

        public CUDReturnMessage DeletePnLBuAttributeGroup(int pnLBuAttributeGroupId, int userId)
        {
            var pnLList = DbContext.PnLBuAttributeGroups.FirstOrDefault(r => r.PnLBuAttributeGroupId == pnLBuAttributeGroupId);
            if (pnLList == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            pnLList.Visible = false;
            pnLList.UpdateBy = userId;
            pnLList.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.PBAG_SuccessDelete);
        }

        #endregion

        #region Thuộc tính BU
        public List<PnLBuAttribute> GetPnLBuAttribute()
        {
            var query = DbContext.PnLBuAttributes.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdatePnLBuAttribute(PnLBuAttributeContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.PnLBuAttributes.SingleOrDefault(x => x.PnLBuAttributeId != data.PnLListId && x.PnLBuAttributeName == data.PnLBuAttributeName && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.PnLBuAttribute_DuplicateName);
            }
            var checkRowCode = DbContext.PnLBuAttributes.SingleOrDefault(x => x.PnLBuAttributeId != data.PnLBuAttributeId && x.PnLBuAttributeCode == data.PnLBuAttributeCode && (bool)x.Visible);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.PnLBuAttribute_DuplicateCode);
            }
            #endregion

            if (data.PnLBuAttributeId == 0) //Innsert
            {
                var newPnLBuAttribute = new PnLBuAttribute()
                {
                    PnLBuAttributeId = data.PnLBuAttributeId,
                    PnLBuAttributeCode = data.PnLBuAttributeCode,
                    PnLBuAttributeName = data.PnLBuAttributeName,
                    PnLListId = data.PnLListId,
                    PnLBUListId = data.PnLBUListId,
                    PnLAttributeGroupId = data.PnLAttributeGroupId,
                    Description = data.Description,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.PnLBuAttributes.Add(newPnLBuAttribute);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.PnLBuAttribute_SuccessCreate);
            }
            //Update
            var pnlBuAttribute = DbContext.PnLBuAttributes.SingleOrDefault(r => r.PnLBuAttributeId == data.PnLBuAttributeId);
            if (pnlBuAttribute == null)
                return new CUDReturnMessage(ResponseCode.Error);

            pnlBuAttribute.PnLBuAttributeCode = data.PnLBuAttributeCode;
            pnlBuAttribute.PnLBuAttributeName = data.PnLBuAttributeName;
            pnlBuAttribute.PnLListId = data.PnLListId;
            pnlBuAttribute.PnLBUListId = data.PnLBUListId;
            pnlBuAttribute.PnLAttributeGroupId = data.PnLAttributeGroupId;
            pnlBuAttribute.Description = data.Description;
            pnlBuAttribute.Visible = data.Visible;
            pnlBuAttribute.UpdateBy = data.UpdatedBy;
            pnlBuAttribute.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.PnLBuAttribute_SuccessUpdate);
        }

        public CUDReturnMessage DeletePnLBuAttribute(int pnlBuAttributeId, int userId)
        {
            var pnlBuAttribute = DbContext.PnLBuAttributes.FirstOrDefault(r => r.PnLBuAttributeId == pnlBuAttributeId);
            if (pnlBuAttribute == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            pnlBuAttribute.Visible = false;
            pnlBuAttribute.UpdateBy = userId;
            pnlBuAttribute.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.PnLBuAttribute_SuccessDelete);
        }

        #endregion

        #region Vùng miền
        public List<Region> GetRegion()
        {
            var query = DbContext.Regions.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdateRegion(RegionContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.Regions.SingleOrDefault(x => x.RegionId != data.RegionId && x.RegionName == data.RegionName && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.Region_DuplicateName);
            }
            var checkRowCode = DbContext.Regions.SingleOrDefault(x => x.RegionId != data.RegionId && x.RegionCode == data.RegionCode && (bool)x.Visible);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.Region_DuplicateCode);
            }
            #endregion

            if (data.RegionId == 0) //Innsert
            {
                var newRegion = new Region()
                {
                    RegionCode = data.RegionCode,
                    RegionName = data.RegionName,
                    Description = data.Description,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.Regions.Add(newRegion);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.Region_SuccessCreate);
            }
            //Update
            var region = DbContext.Regions.SingleOrDefault(r => r.RegionId == data.RegionId);
            if (region == null)
                return new CUDReturnMessage(ResponseCode.Error);

            region.RegionCode = data.RegionCode;
            region.RegionName = data.RegionName;
            region.Description = data.Description;
            region.Visible = data.Visible;
            region.UpdateBy = data.UpdatedBy;
            region.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Region_SuccessUpdate);
        }

        public CUDReturnMessage DeleteRegion(int regionId, int userId)
        {
            var region = DbContext.Regions.FirstOrDefault(r => r.RegionId == regionId);
            if (region == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            region.Visible = false;
            region.UpdateBy = userId;
            region.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Region_SuccessDelete);
        }
        #endregion

        #region Đơn vị hành chính
        public IQueryable<AdministrativeUnit> GetDvhc()
        {
            IQueryable<AdministrativeUnit> query = DbContext.AdministrativeUnits.Where(r => r.Visible == true).OrderBy(x => x.AdministrativeUnitsId);
            return query;
        }

        public AdministrativeUnit Find(int id)
        {
            var query = DbContext.AdministrativeUnits.SingleOrDefault(r => r.AdministrativeUnitsId == id && r.Visible == true);
            return query;
        }

        public CUDReturnMessage Delete(int id, int userId)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    //var allChildLocation = DbContext.AdministrativeUnits.Where(x => ("." + x.LevelPath + ".").Contains("." + id + "."));
                    //if (allChildLocation.Any())
                    //{
                    //    var relRow = DbContext.Devices.Any(x => allChildLocation.Select(r => r.AdministrativeUnitsId).Contains(x.LocationId ?? 0) && x.IsDeleted);
                    //    if (relRow)
                    //    {
                    //        return new CUDReturnMessage(ResponseCode.LocationMngt_IsUsing);
                    //    }
                    //}

                    var query = DbContext.AdministrativeUnits.SingleOrDefault(r => r.AdministrativeUnitsId == id && r.Visible == true);

                    if (query != null)
                    {
                        query.Visible = false;
                        query.UpdatedBy = userId;
                        query.UpdatedDate = DateTime.Now;
                    }

                    DbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return new CUDReturnMessage(ResponseCode.Dvhc_SuccessDelete);
        }

        public CUDReturnMessage InsertUpdateDvhc(DvhcContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            var checkRowName
                = DbContext.AdministrativeUnits
                    .SingleOrDefault(x => x.AdministrativeUnitsVN == data.AdministrativeUnitsVN
                                            && x.AdministrativeUnitsId != data.AdministrativeUnitsId
                                            && x.ParentId == data.ParentId);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.Dvhc_DuplicateName);
            }
            var checkRowCode
                = DbContext.AdministrativeUnits
                    .SingleOrDefault(x => x.AdministrativeUnitsEN == data.AdministrativeUnitsEN
                                          && x.AdministrativeUnitsId != data.AdministrativeUnitsId
                                          && x.ParentId == data.ParentId);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.Dvhc_DuplicateCode);
            }

            if (data.AdministrativeUnitsId == 0)
            {
                var dvhc = new AdministrativeUnit()
                {
                    AdministrativeUnitsVN = data.AdministrativeUnitsVN,
                    AdministrativeUnitsEN = convertToUnSign3(data.AdministrativeUnitsVN),
                    ParentId = data.ParentId,
                    LevelNo = data.LevelNo,
                    LevelPath = data.LevelNo.ToString(),
                    CreatedBy = data.CreatedBy,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = DateTime.Now,
                    Prefix = data.Prefix,
                    Latitude = 0,
                    Longitude = 0,
                    Visible = true
                };
                DbContext.AdministrativeUnits.Add(dvhc);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.Dvhc_SuccessCreate);
            }

            //Update
            var dvhcUpdate = DbContext.AdministrativeUnits.SingleOrDefault(r => r.AdministrativeUnitsId == data.AdministrativeUnitsId);
            if (dvhcUpdate == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            dvhcUpdate.AdministrativeUnitsVN = data.AdministrativeUnitsVN;
            dvhcUpdate.AdministrativeUnitsEN = convertToUnSign3(data.AdministrativeUnitsVN);
            dvhcUpdate.Prefix = data.Prefix;
            dvhcUpdate.Latitude = data.Latitude;
            dvhcUpdate.Longitude = data.Longitude;
            dvhcUpdate.UpdatedBy = data.UpdatedBy;
            dvhcUpdate.UpdatedDate = DateTime.Now;
            dvhcUpdate.Visible = true;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Dvhc_SuccessUpdate);
        }

        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public List<AdministrativePrefix> GetPrefix()
        {
            var query = DbContext.AdministrativePrefixes.ToList();
            return query;
        }

        #endregion

        #region Danh mục nhóm cơ sở
        public List<BasisGroup> GetBasisGroup()
        {
            var query = DbContext.BasisGroups.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdateBasisGroup(BasisGroupContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.BasisGroups.SingleOrDefault(x => x.BasisGroupId != data.BasisGroupId && x.BasisGroupName == data.BasisGroupName && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.BasisGroup_DuplicateName);
            }
            var checkRowCode = DbContext.BasisGroups.SingleOrDefault(x => x.BasisGroupId != data.BasisGroupId && x.BasisGroupCode == data.BasisGroupCode && (bool)x.Visible);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.BasisGroup_DuplicateCode);
            }
            #endregion

            if (data.BasisGroupId == 0) //Innsert
            {
                var newBasisGroup = new BasisGroup()
                {
                    BasisGroupCode = data.BasisGroupCode,
                    BasisGroupName = data.BasisGroupName,
                    Address = data.Address,
                    Longitude = data.Longitude,
                    Latitude = data.Latitude,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.BasisGroups.Add(newBasisGroup);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.BasisGroup_SuccessCreate);
            }
            //Update
            var basisGroup = DbContext.BasisGroups.SingleOrDefault(r => r.BasisGroupId == data.BasisGroupId);
            if (basisGroup == null)
                return new CUDReturnMessage(ResponseCode.Error);

            basisGroup.BasisGroupCode = data.BasisGroupCode;
            basisGroup.BasisGroupName = data.BasisGroupName;
            basisGroup.Address = data.Address;
            basisGroup.Longitude = data.Longitude;
            basisGroup.Latitude = data.Latitude;
            basisGroup.Visible = data.Visible;
            basisGroup.UpdateBy = data.UpdatedBy;
            basisGroup.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.BasisGroup_SuccessUpdate);
        }

        public CUDReturnMessage DeleteBasisGroup(int basisGroupId, int userId)
        {
            var basisGroup = DbContext.BasisGroups.FirstOrDefault(r => r.BasisGroupId == basisGroupId);
            if (basisGroup == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            basisGroup.Visible = false;
            basisGroup.UpdateBy = userId;
            basisGroup.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.BasisGroup_SuccessDelete);
        }

        #endregion

        #region Danh mục cơ sở
        public List<Basis> GetBasis()
        {
            var query = DbContext.Bases.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdateBasis(BasisContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowCode = DbContext.Bases.SingleOrDefault(x => x.BasisId != data.BasisId && x.BasisCode == data.BasisCode && (bool)x.Visible);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.Basis_DuplicateCode);
            }
            var checkRowName = DbContext.Bases.SingleOrDefault(x => x.BasisId != data.BasisId && x.BasisName == data.BasisName && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.Basis_DuplicateName);
            }

            #endregion

            if (data.BasisId == 0) //Innsert
            {
                var newBasis = new Basis()
                {
                    BasisCode = data.BasisCode,
                    BasisName = data.BasisName,
                    BasisGroupId = data.BasisGroupId,
                    PnLListId = data.PnLListId,
                    PnLBUListId = data.PnLBUListId,
                    Description = data.Description,
                    CityId = data.CityId,
                    DistrictId = data.DistrictId,
                    WardId = data.WardId,
                    RefCode = data.RefCode,
                    StatusId = data.StatusId,
                    FullName = data.FullName,
                    StatusDescription = data.StatusDescription,
                    OpeningDate = data.OpeningDate,
                    Latitude = data.Latitude,
                    Longitude = data.Longitude,
                    Address = data.Address,
                    Manager = data.Manager,
                    ManagerPhone = data.ManagerPhone,
                    SitePhone = data.SitePhone,
                    SiteEmail = data.SiteEmail,
                    AreaManager = data.AreaManager,
                    AreaManagerPhone = data.AreaManagerPhone,
                    AreaManagerEmail = data.AreaManagerEmail,
                    DepartmentId = data.DepartmentId,
                    StaffId = data.StaffId,
                    Visible = data.Visible,
                    CreateBy = data.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.Bases.Add(newBasis);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.Basis_SuccessCreate);
            }
            //Update
            var basis = DbContext.Bases.SingleOrDefault(r => r.BasisId == data.BasisId);
            if (basis == null)
                return new CUDReturnMessage(ResponseCode.Error);
            basis.BasisCode = data.BasisCode;
            basis.BasisName = data.BasisName;
            basis.BasisGroupId = data.BasisGroupId;
            basis.PnLListId = data.PnLListId;
            basis.PnLBUListId = data.PnLBUListId;
            basis.Description = data.Description;
            basis.CityId = data.CityId;
            basis.DistrictId = data.DistrictId;
            basis.WardId = data.WardId;
            basis.RefCode = data.RefCode;
            basis.StatusId = data.StatusId;
            basis.FullName = data.FullName;
            basis.StatusDescription = data.StatusDescription;
            basis.OpeningDate = data.OpeningDate;
            basis.Latitude = data.Latitude;
            basis.Longitude = data.Longitude;
            basis.Address = data.Address;
            basis.Manager = data.Manager;
            basis.ManagerPhone = data.ManagerPhone;
            basis.SitePhone = data.SitePhone;
            basis.SiteEmail = data.SiteEmail;
            basis.AreaManager = data.AreaManager;
            basis.AreaManagerPhone = data.AreaManagerPhone;
            basis.AreaManagerEmail = data.AreaManagerEmail;
            basis.DepartmentId = data.DepartmentId;
            basis.StaffId = data.StaffId;
            basis.Visible = data.Visible;
            basis.UpdateBy = data.UpdateBy;
            basis.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Basis_SuccessUpdate);
        }

        public CUDReturnMessage DeleteBasis(int BasisId, int userId)
        {
            var basis = DbContext.Bases.FirstOrDefault(r => r.BasisId == BasisId);
            if (basis == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            basis.Visible = false;
            basis.UpdateBy = userId;
            basis.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Basis_SuccessDelete);
        }

        public List<BasisStatu> GetBasisStatus()
        {
            var query = DbContext.BasisStatus.ToList();
            return query;
        }
        #endregion

        #region Title
        public CUDReturnMessage InsertUpdateDepartmentTitle(DepartmentTitleContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.DepartmentTitles.SingleOrDefault(x => x.DepartmentTitleId != data.DepartmentTitleId && x.DepartmentTitleName == data.DepartmentTitleName && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.DepartmentTitle_DuplicateName);
            }
            var checkRowCode = DbContext.DepartmentTitles.SingleOrDefault(x => x.DepartmentTitleId != data.DepartmentTitleId && x.DepartmentTitleCode == data.DepartmentTitleCode && (bool)x.Visible);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.DepartmentTitle_DuplicateCode);
            }
            #endregion

            if (data.DepartmentTitleId == 0) //Innsert
            {
                var newDeTitle = new DepartmentTitle()
                {
                    DepartmentTitleCode = data.DepartmentTitleCode,
                    DepartmentTitleName = data.DepartmentTitleName,
                    Description = data.Description,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.DepartmentTitles.Add(newDeTitle);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.DepartmentTitle_SuccessCreate);
            }
            //Update
            var departmentTitle = DbContext.DepartmentTitles.SingleOrDefault(r => r.DepartmentTitleId == data.DepartmentTitleId);
            if (departmentTitle == null)
                return new CUDReturnMessage(ResponseCode.Error);

            departmentTitle.DepartmentTitleCode = data.DepartmentTitleCode;
            departmentTitle.DepartmentTitleName = data.DepartmentTitleName;
            departmentTitle.Description = data.Description;
            departmentTitle.Visible = data.Visible;
            departmentTitle.UpdateBy = data.UpdatedBy;
            departmentTitle.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.DepartmentTitle_SuccessUpdate);
        }

        public CUDReturnMessage DeleteDepartmentTitle(int departmentTitleId, int userId)
        {
            var departmantTitle = DbContext.DepartmentTitles.FirstOrDefault(r => r.DepartmentTitleId == departmentTitleId);
            if (departmantTitle == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            departmantTitle.Visible = false;
            departmantTitle.UpdateBy = userId;
            departmantTitle.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.DepartmentTitle_SuccessDelete);
        }

        #endregion

        #region Level
        public List<Level> GetLevel()
        {
            var query = DbContext.Levels.Where(x => (bool)x.Visible).ToList();
            return query;
        }

        public CUDReturnMessage InsertUpdateLevel(LevelContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            #region Validate
            var checkRowName = DbContext.Levels.SingleOrDefault(x => x.LevelId != data.LevelId && x.LevelName == data.LevelName && (bool)x.Visible);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.Level_DuplicateName);
            }
            var checkRowCode = DbContext.Levels.SingleOrDefault(x => x.LevelId != data.LevelId && x.LevelCode == data.LevelCode && (bool)x.Visible);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.Level_DuplicateCode);
            }
            #endregion

            if (data.LevelId == 0) //Innsert
            {
                var newLevel = new Level()
                {
                    LevelCode = data.LevelCode,
                    LevelName = data.LevelName,
                    Description = data.Description,
                    Visible = data.Visible,
                    CreateBy = data.CreatedBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = 0,
                    UpdateDate = DateTime.Now
                };
                DbContext.Levels.Add(newLevel);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.Level_SuccessCreate);
            }
            //Update
            var level = DbContext.Levels.SingleOrDefault(r => r.LevelId == data.LevelId);
            if (level == null)
                return new CUDReturnMessage(ResponseCode.Error);

            level.LevelCode = data.LevelCode;
            level.LevelName = data.LevelName;
            level.Description = data.Description;
            level.Visible = data.Visible;
            level.UpdateBy = data.UpdatedBy;
            level.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Level_SuccessUpdate);
        }

        public CUDReturnMessage DeleteLevel(int levelId, int userId)
        {
            var level = DbContext.Levels.FirstOrDefault(r => r.LevelId == levelId);
            if (level == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }
            level.Visible = false;
            level.UpdateBy = userId;
            level.UpdateDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.Level_SuccessDelete);
        }

        #endregion

        #region system checklisk
        /// <summary>
        /// Tra cứu danh sách hệ thống checklist
        /// </summary>
        /// <param name="state"></param>
        /// <param name="kw"></param>
        /// <param name="p"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public IQueryable<SystemCheckList> GetSystemCheckList(int userId, int state = 0, string kw = "", int systemId = 0, Boolean IsCheckRule = false)
        {
            var query = DbContext.SystemCheckLists.AsQueryable();
            if (systemId > 0)
                query = query.Where(c => c.SystemId == systemId);
            if (state > 0)
                query = query.Where(c => c.Visible == (state == (int)SystemStatusEnum.Active));
            if (!string.IsNullOrEmpty(kw))
                query = query.Where(c => c.SystemName.Contains(kw));
            if (IsCheckRule && userId>0)
                query = query.Where(c => c.AdminUser_System.Any(d => d.UId == userId));

            return query;
        }
        /// <summary>
        /// xóa hệ thống
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteSystemCheckList(int systemId, int userId)
        {
            var result = default(CUDReturnMessage);
            try
            {
                var system = DbContext.SystemCheckLists.Find(systemId);
                if (system == null) return new CUDReturnMessage(ResponseCode.SystemMngt_NoExists);
                system.Visible = false;
                system.UpdatedBy = userId;
                system.UpdatedDate = DateTime.Now;
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.SystemMngt_SuccessDeleted);
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.SystemMngt_Error, ex.Message);
            }
            return result;
        }
        /// <summary>
        /// tạo mới/ cập nhật thông tin hệ thống
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateSystemCheckList(SystemCheckListContract info,int userId)
        {
            try
            {

                var result = default(CUDReturnMessage);
                if (info == null) new CUDReturnMessage(ResponseCode.SystemMngt_NoExists);

                if (DbContext.SystemCheckLists.Where(i => i.SystemName.ToLower().Trim().Equals(info.SystemName.ToLower().Trim()) && i.SystemId != info.SystemId).Any())
                    return result = new CUDReturnMessage(ResponseCode.SystemMngt_DuplicateItemExist);

                var system = DbContext.SystemCheckLists.FirstOrDefault(c => (c.SystemId == info.SystemId));
                if (system != null)
                {
                    #region update template
                    system.UpdatedBy = userId;
                    system.UpdatedDate = DateTime.Now;
                    system.Visible = info.Visible;
                    system.Description = info.Description;
                    //system.SystemName = info.SystemName;
                    system.Priority = info.Priority;

                    //update more info
                    system.Code = info.Code;
                    system.PlId = info.PlId;
                    system.SystemNameEn = info.SystemNameEn;
                    system.CateId = info.CateId;
                    system.SubCateId = info.SubCateId;
                    system.FunctionOverview = info.FunctionOverview;
                    system.ProviderName = info.ProviderName;
                    system.OriginTypeId = info.OriginTypeId;
                    system.Platform = info.Platform;
                    system.AppTypeId = info.AppTypeId;
                    system.IsSAP = info.IsSAP;
                    system.State = info.State;
                    system.UrlSystem = info.UrlSystem;
                    system.AuthenticationMethodId = info.AuthenticationMethodId;
                    system.RankId = info.RankId;
                    system.RTOTypeId = info.RTOTypeId;
                    system.RPOTypeId = info.RPOTypeId;
                    system.RLO = info.RLO ?? 0;
                    system.DRTypeId = info.DRTypeId;
                    system.LastDateDRTest = info.LastDateDRTest ?? DateTime.Now;
                    system.ScStateId = info.ScStateId;
                    system.SME = info.SME;
                    system.OwingBusinessUnit = info.OwingBusinessUnit;
                    system.ITContact = info.ITContact;
                    system.YearImplement = info.YearImplement;
                    system.QuantityUserActive = info.QuantityUserActive;
                    system.ConCurrentUser = info.ConCurrentUser;
                    system.BusinessHour = info.BusinessHour;
                    system.BusinessIssue = info.BusinessIssue;
                    system.TechIssue = info.TechIssue;
                    system.SystemMaintainTime = info.SystemMaintainTime;
                    system.IsDevTest = info.IsDevTest;
                    system.HostingLocation = info.HostingLocation;
                    system.IsReplace = info.IsReplace;
                    system.ReplaceBy = info.ReplaceBy;
                    system.DetailReplaceBy = info.DetailReplaceBy;
                    system.IsRequirementSecurity = info.IsRequirementSecurity;
                    system.IsRequirementSecurityDesign = info.IsRequirementSecurityDesign;
                    system.IsCheckCertification = info.IsCheckCertification;
                    system.IsCheckSecurityByGolive = info.IsCheckSecurityByGolive;
                    system.IsCheckRisk = info.IsCheckRisk;
                    system.SecurityStateId = info.SecurityStateId;
                    system.PerformanceId = info.PerformanceId;
                    system.PerformanceNote = info.PerformanceNote;

                    #endregion
                    result = new CUDReturnMessage(ResponseCode.SystemMngt_SuccessUpdated);
                }
                else
                {
                    system = new SystemCheckList()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = userId,
                        UpdatedDate = DateTime.Now,
                        UpdatedBy = this.uid,
                        SystemName = info.SystemName,
                        Description = info.Description,
                        Visible = info.Visible,
                        Priority = info.Priority
                        // add more info
                        ,
                        Code = info.Code
                        ,
                        PlId = info.PlId
                        ,
                        SystemNameEn = info.SystemNameEn
                        ,
                        CateId = info.CateId
                        ,
                        SubCateId = info.SubCateId
                        ,
                        FunctionOverview = info.FunctionOverview
                        ,
                        ProviderName = info.ProviderName
                        ,
                        OriginTypeId = info.OriginTypeId
                        ,
                        Platform = info.Platform
                        ,
                        AppTypeId = info.AppTypeId
                        ,
                        IsSAP = info.IsSAP
                        ,
                        State = info.State
                        ,
                        UrlSystem = info.UrlSystem
                        ,
                        AuthenticationMethodId = info.AuthenticationMethodId
                        ,
                        RankId = info.RankId
                        ,
                        RTOTypeId = info.RTOTypeId
                        ,
                        RPOTypeId = info.RPOTypeId
                        ,
                        RLO = info.RLO ?? 0
                        ,
                        DRTypeId = info.DRTypeId
                        ,
                        LastDateDRTest = info.LastDateDRTest ?? DateTime.Now
                        ,
                        ScStateId = info.ScStateId
                        ,
                        SME = info.SME
                        ,
                        OwingBusinessUnit = info.OwingBusinessUnit
                        ,
                        ITContact = info.ITContact
                        ,
                        YearImplement = info.YearImplement
                        ,
                        QuantityUserActive = info.QuantityUserActive
                        ,
                        ConCurrentUser = info.ConCurrentUser
                        ,
                        BusinessHour = info.BusinessHour
                        ,
                        BusinessIssue = info.BusinessIssue
                        ,
                        TechIssue = info.TechIssue
                        ,
                        SystemMaintainTime = info.SystemMaintainTime
                        ,
                        IsDevTest = info.IsDevTest
                        ,
                        HostingLocation = info.HostingLocation
                        ,
                        IsReplace = info.IsReplace
                        ,
                        ReplaceBy = info.ReplaceBy
                        ,
                        DetailReplaceBy = info.DetailReplaceBy
                        ,
                        IsRequirementSecurity = info.IsRequirementSecurity
                        ,
                        IsRequirementSecurityDesign = info.IsRequirementSecurityDesign
                        ,
                        IsCheckCertification = info.IsCheckCertification
                        ,
                        IsCheckSecurityByGolive = info.IsCheckSecurityByGolive
                        ,
                        IsCheckRisk = info.IsCheckRisk
                        ,
                        SecurityStateId = info.SecurityStateId
                        ,
                        PerformanceId = info.PerformanceId
                        ,
                        PerformanceNote = info.PerformanceNote



                    };
                    DbContext.SystemCheckLists.Add(system);
                    result = new CUDReturnMessage(ResponseCode.SystemMngt_SuccessCreated);
                }
                DbContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.SystemMngt_Error, ex.Message);
            }
        }

        public CUDReturnMessage AssignUserToSystemCheckList(AssignOwnerSystemContract info,int uId)
        {
            var result = default(CUDReturnMessage);
            try
            {
                if (info == null) return new CUDReturnMessage(ResponseCode.SystemMngt_NoExists);
                var system = DbContext.SystemCheckLists.FirstOrDefault(c => (c.SystemId == info.SystemId));
                if (system == null) return new CUDReturnMessage(ResponseCode.SystemMngt_NoExists);
                if (system != null)
                {
                    system.AdminUser_System.ForEach(c => { c.IsDeleted = true; c.UpdatedBy = uid; c.UpdateDate = DateTime.Now; });
                    foreach (var userId in info.UIds)
                    {
                        var itemMap = system.AdminUser_System.FirstOrDefault(c => c.UId.Equals(userId));
                        if (itemMap != null) itemMap.IsDeleted = false;
                        else
                        {
                            system.AdminUser_System.Add(new AdminUser_System()
                            {
                                UId = userId,
                                CreatedDate = DateTime.Now,
                                CreatedBy = uId,
                                IsDeleted = false
                            });
                        }
                    }
                }
                DbContext.SaveChanges();
                result = new CUDReturnMessage(ResponseCode.SystemMngt_AssignOwnerSuccessed);
                return result;
            }            
            catch (Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.SystemMngt_Error, ex.Message);
            }
        }

        #endregion
        #region cate,subcate

        public IQueryable<SubCateSystem> GetSubCate(int state, string kw, int subCateId)
        {
            var query = DbContext.SubCateSystems.AsQueryable();
            if (subCateId > 0)
                query = query.Where(c => c.SubCateId == subCateId);
            if (state > 0)
                query = query.Where(c => c.Visible == (state == (int)SystemStateEnum.Active));
            if (!string.IsNullOrEmpty(kw))
                query = query.Where(c => c.SubCateName.Contains(kw));
            return query;
        }
        public IQueryable<CateSystem> GetCate(int state, string kw, int CateId)
        {
            var query = DbContext.CateSystems.AsQueryable();
            if (CateId > 0)
                query = query.Where(c => c.CateId == CateId);
            if (state > 0)
                query = query.Where(c => c.Visible == (state == (int)SystemStateEnum.Active));
            if (!string.IsNullOrEmpty(kw))
                query = query.Where(c => c.CateName.Contains(kw));
            return query;
        }
        #endregion
    }
}
