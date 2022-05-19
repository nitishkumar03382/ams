using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AmsDataAccess
    {
        static SqlConnection _dbConnection;

        public AmsDataAccess(String connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }
        public static DataTable GetLoginData(string UserId)
        {
            DataTable dt = null;
            using (SqlCommand sqlComm = new SqlCommand("[dbo].[getLoginData]", _dbConnection))
            {
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlParameter param1 = new SqlParameter("@userId", UserId);
                sqlComm.Parameters.Add(param1);
                using (SqlDataAdapter sqlDA = new SqlDataAdapter(sqlComm))
                {
                    dt = new DataTable();
                    sqlDA.Fill(dt);
                }
            }
            return dt;
        }
    }
}
