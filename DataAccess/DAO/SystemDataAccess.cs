namespace DataAccess
{
    public interface ISystemDataAccess
    {
        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        /// <returns></returns>
        bool HealthCheckDbConnection();
    }

    public class SystemDataAccess : BaseDataAccess, ISystemDataAccess
    {
        public SystemDataAccess(string appid, int uid) : base(appid, uid)
        {
        }

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        /// <returns></returns>
        public bool HealthCheckDbConnection()
        {
            return DbContext.Database.Exists();
        }
    }
}