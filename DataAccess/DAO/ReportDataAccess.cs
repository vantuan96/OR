using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DataAccess.Models;

namespace DataAccess.DAO
{
    public interface IReportDataAccess
    {
        
    }
    public class ReportDataAccess : BaseDataAccess, IReportDataAccess
    {  
        public ReportDataAccess(string appid, int uid) : base(appid, uid)
        {

        }
        
        
    }
}
