using AMS.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMS.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if(Session["userId"]!= null)
            {
                string empId = Convert.ToString(Session["userId"]);
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.getTodayAttendance();
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

        public string viewAllEmployeeAttendance()
        {
            return "viewAllEmployeeAttendance";
        }
        public string createUser()
        {
            return "Create new user";
        }
    }
}