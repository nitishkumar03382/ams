using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.Models;
using DataAccessLayer;
using System.Configuration;
using System.Data;

namespace AMS.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult EmployeeHome()
        {
            if (Session["userId"] != null)
            {
                string empId = Convert.ToString(Session["userId"]);
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.getEmpDetail(empId);
                ViewBag.empName = dt.Rows[0]["name"];
                return View();
            }
            else
                return RedirectToAction("Index", "Home");

        }
        public ActionResult AttendanceLogin()
        {
            if (Session["userId"] != null)
            {
                string emp_Id = Convert.ToString(Session["userId"]);
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                int AffectedRows = AmsDataAccess.attendanceLogin(emp_Id);
                if (AffectedRows == 1)
                    return RedirectToAction("messageHandler", "Home", new { msg = "Logged In.", msgType = "success" });
                else
                    return RedirectToAction("messageHandler", "Home", new { msg = "Already Logged in.", msgType = "warning" });
            }
            return RedirectToAction("messageHandler", "Home", new { msg = "ERROR.", msgType = "error" });
        }
        public ActionResult AttendanceLogout()
        {
            if (Session["userId"] != null)
            {
                string emp_Id = Convert.ToString(Session["userId"]);
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                int AffectedRows = AmsDataAccess.attendanceLogout(emp_Id);
                if (AffectedRows == 1)
                    return RedirectToAction("messageHandler", "Home", new { msg = "Logged Out Done.", msgType = "success" });
                return RedirectToAction("messageHandler", "Home", new { msg = "Can`t do Logout.", msgType = "warning" });
            }
            return RedirectToAction("messageHandler", "Home", new { msg = "ERROR.", msgType = "error" });
        }
        public ActionResult LeaveApply(string empId)
        {
            if (Session["userId"] != null)
            {
                ViewBag.empId = Convert.ToString(Session["userId"]);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SendLeaveApplication(Leave l)
        {
            if (Session["userId"] != null)
            {
                string emp_id = Convert.ToString(Session["userId"]);
                string leaveReason = l.leave_reason;
                string leaveStartDate = l.leave_start_date;
                string numLeaveDays = l.num_leave_days;
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                int AffectedRows = AmsDataAccess.applyLeave(emp_id, leaveStartDate, numLeaveDays, leaveReason );
                if (AffectedRows == 1)
                {

                    return RedirectToAction("messageHandler", "Home", new { msg = "SUCCESS.", msgType="success"});
                }
                else
                {
                    return RedirectToAction("messageHandler", "Home", new { msg = "ERROR.", msgType = "error" });
                }
            }
            return RedirectToAction("messageHandler", "Home", new { msg = "UnAuthorized Access.", msgType = "error" });
        }

        public ActionResult viewAtdReport()
        {
            if (Session["userId"] != null)
            {
                string empId = Convert.ToString(Session["userId"]);
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.GetAttendanceData(empId);
                List<Attendance> obj = new List<Attendance>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Attendance d = new Attendance();
                        d.Employee_id = Convert.ToString(dt.Rows[i]["Employee_id"]);
                        d.Date = Convert.ToString(dt.Rows[i]["Date"]);
                        d.loginTime = Convert.ToString(dt.Rows[i]["loginTime"]);
                        d.logoutTime = Convert.ToString(dt.Rows[i]["logoutTime"]);
                        d.AttendanceStatus = Convert.ToString(dt.Rows[i]["AttendanceStatus"]);
                        obj.Add(d);

                    }
                }
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}