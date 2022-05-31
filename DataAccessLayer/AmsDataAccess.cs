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

        public static int attendanceLogin(string empId)
        {
            int noOfRowsAffected = -1;
            try
            {
                _dbConnection.Open();
                using (SqlCommand sqlComm = new SqlCommand("[dbo].[doLogin]", _dbConnection))
                {
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlParameter param1 = new SqlParameter("@empId", empId);
                    sqlComm.Parameters.Add(param1);

                    noOfRowsAffected = sqlComm.ExecuteNonQuery();

                }
                _dbConnection.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return noOfRowsAffected;

        }

        public static int attendanceLogout(string empId)
        {
            int noOfRowsAffected = -1;
            try
            {
                _dbConnection.Open();
                using (SqlCommand sqlComm = new SqlCommand("[dbo].[doLogout]", _dbConnection))
                {
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlParameter param1 = new SqlParameter("@empId", empId);
                    sqlComm.Parameters.Add(param1);

                    noOfRowsAffected = sqlComm.ExecuteNonQuery();

                }
                _dbConnection.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return noOfRowsAffected;

        }
        //Get Attendance data report for a particular employee
        public static DataTable GetAttendanceData(string empId)
        {
            DataTable dt = null;
            using (SqlCommand sqlComm = new SqlCommand("[dbo].[viewAtdReport]", _dbConnection))
            {
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlParameter param1 = new SqlParameter("@empId", empId);
                sqlComm.Parameters.Add(param1);
                using (SqlDataAdapter sqlDA = new SqlDataAdapter(sqlComm))
                {
                    dt = new DataTable();
                    sqlDA.Fill(dt);
                }
            }
            return dt;
        }

        // Get attendance of all employees for today
          public static DataTable getTodayAttendance()
        {
            DataTable dt = null;
            using (SqlCommand sqlComm = new SqlCommand("[dbo].[getTodayAttendance]", _dbConnection))
            {
                sqlComm.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter sqlDA = new SqlDataAdapter(sqlComm))
                {
                    dt = new DataTable();
                    sqlDA.Fill(dt);
                }
            }
            return dt;
        }
        //Apply for leave
        //empId, leave_apply_date, leave_start_date, num_leave_days, leave_reason, leave_status_id

        public static int applyLeave(string empId, string startDate, string numLeaveDays, string reason)
        {
            int noOfRowsAffected = -1;
            try
            {
                _dbConnection.Open();
                using (SqlCommand sqlComm = new SqlCommand("[dbo].[applyLeave]", _dbConnection))
                {
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlParameter param1 = new SqlParameter("@empId", empId);
                    sqlComm.Parameters.Add(param1);

                    SqlParameter param3 = new SqlParameter("@leaveStartDate", startDate);
                    sqlComm.Parameters.Add(param3);

                    SqlParameter param4 = new SqlParameter("@num_leave_days", numLeaveDays);
                    sqlComm.Parameters.Add(param4);

                    SqlParameter param5 = new SqlParameter("@leave_reason", reason);
                    sqlComm.Parameters.Add(param5);

                    noOfRowsAffected = sqlComm.ExecuteNonQuery();

                }
                _dbConnection.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return noOfRowsAffected;

        }
        //Attendance report of all employees
        public static DataTable allEmployeeAttendance(string year, string month)
        {
            DataTable dt = null;
            using (SqlCommand sqlComm = new SqlCommand("[dbo].[getAllEmployeeAttendanceReport]", _dbConnection))
            {
                SqlParameter param1 = new SqlParameter("@year", year);
                sqlComm.Parameters.Add(param1);
                SqlParameter param2 = new SqlParameter("@month", month);
                sqlComm.Parameters.Add(param2);
                sqlComm.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter sqlDA = new SqlDataAdapter(sqlComm))
                {
                    dt = new DataTable();
                    sqlDA.Fill(dt);
                }
            }
            return dt;
        }

        //number of days and holidays
        public static DataTable daysAndHolidaysInMonth(string year, string month)
        {
            DataTable dt = null;
            using (SqlCommand sqlComm = new SqlCommand("[dbo].[dayAndHolidayInMonth]", _dbConnection))
            {
                SqlParameter param1 = new SqlParameter("@year", year);
                sqlComm.Parameters.Add(param1);
                SqlParameter param2 = new SqlParameter("@month", month);
                sqlComm.Parameters.Add(param2);
                sqlComm.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter sqlDA = new SqlDataAdapter(sqlComm))
                {
                    dt = new DataTable();
                    sqlDA.Fill(dt);
                }
            }
            return dt;
        }

        //Get All Login Data
        public static DataTable getAllLoginData()
        {
            DataTable dt = null;
            using (SqlCommand sqlComm = new SqlCommand("[dbo].[getAllLoginData]", _dbConnection))
            {
                sqlComm.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter sqlDA = new SqlDataAdapter(sqlComm))
                {
                    dt = new DataTable();
                    sqlDA.Fill(dt);
                }
            }
            return dt;
        }

        //Get Pending Leave Data of last 30 days
        public static DataTable getLast30DayPendingLeave()
        {
            DataTable dt = null;
            using (SqlCommand sqlComm = new SqlCommand("[dbo].[getLast30DayPendingLeave]", _dbConnection))
            {
                sqlComm.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter sqlDA = new SqlDataAdapter(sqlComm))
                {
                    dt = new DataTable();
                    sqlDA.Fill(dt);
                }
            }
            return dt;
        }
        //Get all leave data
        public static DataTable getAllLeave()
        {
            DataTable dt = null;
            using (SqlCommand sqlComm = new SqlCommand("[dbo].[getAllLeave]", _dbConnection))
            {
                sqlComm.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter sqlDA = new SqlDataAdapter(sqlComm))
                {
                    dt = new DataTable();
                    sqlDA.Fill(dt);
                }
            }
            return dt;
        }

        public static int manageLeave(string leaveId, int leaveStatus, string empId)
        {
            int noOfRowsAffected = -1;
            try
            {
                _dbConnection.Open();
                using (SqlCommand sqlComm = new SqlCommand("[dbo].[manageLeave]", _dbConnection))
                {
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlParameter param1 = new SqlParameter("@leaveId", leaveId);
                    sqlComm.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter("@leaveStatus",leaveStatus);
                    sqlComm.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter("@empId", empId);
                    sqlComm.Parameters.Add(param3);

                    noOfRowsAffected = sqlComm.ExecuteNonQuery();

                }
                _dbConnection.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return noOfRowsAffected;

        }

    }
}
