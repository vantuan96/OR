using Admin.Resource;
using Contract.OR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Models.OR
{
    public class ORAnesthProgressBase
    {
        public int Id { get; set; }
        public string PId { get; set; }
        [Display(Name = "Report_Label_Title_Name", ResourceType = typeof(LayoutResource))]
        public string HoTen { get; set; }
        [Display(Name = "Report_Label_Title_Address", ResourceType = typeof(LayoutResource))]
        public string Address { get; set; }
        public string PatientPhone { get; set; }
        public string Email { get; set; }
        public string Ages { get; set; }
        public int Sex { get; set; }
        //ext
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int Priority { get; set; }
        public string VisitCode { get; set; }
        public string NameProject { get; set; }
        public string CreatedByName { get; set; }

        public string NameCreatedBy { get; set; }
        public string EmailCreatedBy { get; set; }


        public int HpServiceId { get; set; }
        public HpServiceContract Service { get; set; }
        public int ORRoomId { get; set; }
        public int SurgeryType { get; set; }

        #region datatime
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? NgaySinh { get; set; }
        public string ShowNgaySinh
        {
            get
            {
                if (NgaySinh != null && NgaySinh != DateTime.MinValue)
                    return NgaySinh.Value.ToVEShortDate();
                return string.Empty;
            }
            set { }
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy }", ApplyFormatInEditMode = false)]
        public DateTime dtOperation { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string ShowdtOperation
        {
            get
            {
                return dtOperation.ToVEShortDate();
            }
            set { }
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy }", ApplyFormatInEditMode = false)]
        public DateTime? dtAdmission { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string ShowdtAdmission
        {
            get
            {
                return dtAdmission != null ? dtAdmission.Value.ToVEShortDate() : string.Empty;
            }
            set { }
        }

        public DateTime dtStart { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public string ShowdtStart
        {
            get; set;
        }


        public DateTime dtEnd { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public string ShowdtEnd
        {
            get; set;

        }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public string ShowTimeAdmission
        {
            get; set;

        }
        #endregion
        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string HospitalPhone { get; set; }
        public int State { get; set; }
        public string OrderID { get; set; }
        public string ChargeDetailId { get; set; }
        public string DepartmentCode { get; set; }
        public string HpServiceCode { get; set; }
        public string HpServiceName { get; set; }
        public string ORRoomName { get; set; }
        //vutv7
        public DateTime? ChargeDate { get; set; }
        public string ChargeDateStr { get; set; }
        public string ChargeBy { get; set; }
    }
}
