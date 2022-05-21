using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.Models;
using DataAccessLayer;
using System.Configuration;

namespace AMS.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult EmployeeHome()
        {
            if (Session["userId"] != null)
                return View();
            else
                return RedirectToAction("Index", "Home");

        }
        public string AttendanceLogin()
        {
            if (Session["userId"] != null)
            {
                string emp_Id = Convert.ToString(Session["userId"]);
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                int AffectedRows = AmsDataAccess.attendanceLogin(emp_Id);
                if (AffectedRows == 1)
                    return "Login Done";
                else
                    return "Login Can`t done today";
            }
            return "ERROR";
        }
        public string AttendanceLogout()
        {
            if (Session["userId"] != null)
            {
                string emp_Id = Convert.ToString(Session["userId"]);
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                int AffectedRows = AmsDataAccess.attendanceLogout(emp_Id);
                if (AffectedRows == 1)
                    return "Logout Done";
                return "Can`t logout";
            }
            return "ERROR!!!";
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
        public string SendLeaveApplication(Leave l)
        {
            string leaveReason = l.leave_reason;
            string leaveStartDate = l.leave_start_date;
            string numLeaveDays = l.num_leave_days;
            return "Leave Application: " + leaveReason + "  " + leaveStartDate + "  " + numLeaveDays;
        }
    }
}