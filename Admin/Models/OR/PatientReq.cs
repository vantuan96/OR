using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.OR
{
    public class PatientReq : RequesteBase
    {
        public string pMa { get; set; }
        public string pAge { get; set; }
        public int pSex { get; set; }
        public string pName { get; set; }
        public string pBirthday { get; set; }
        public string pAddress { get; set; }
        public string pNational { get; set; }
        public string pPhone { get; set; }
    }
}