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
                        d.loginTime = Convert.ToString(dt.Rows[i]["loginTime"]);
                        d.logoutTime = Convert.ToString(dt.Rows[i]["logoutTime"]);
                        obj.Add(d);

                    }
                }
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult viewAllEmployeeAttendance(string s)
        {
            if (Session["userId"] != null)
            {
                string year="2022", month="05";
                if(s == null)
                {
                    DateTime dte = DateTime.Today;
                     year = dte.Year.ToString();
                     month = dte.Month.ToString();
                }
                else
                {
                    s = "DONE";
                    
                }
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.allEmployeeAttendance(year, month);
                DataTable dt2 = AmsDataAccess.daysAndHolidaysInMonth(year, month);
                int daysInMonth = Convert.ToInt32(dt2.Rows[0]["days"]);
                int HolidaysInMonth = Convert.ToInt32(dt2.Rows[0]["weekends"]);
                List<AttendanceReport> obj = new List<AttendanceReport>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        AttendanceReport d = new AttendanceReport();
                        d.empId = Convert.ToString(dt.Rows[i]["empId"]);
                        if (Convert.ToString(dt.Rows[i]["present"]) == "")
                            d.presentDays = "0";
                        else
                            d.presentDays = Convert.ToString(dt.Rows[i]["present"]);
                        if (Convert.ToString(dt.Rows[i]["leave"]) == "")
                            d.leaveDays = "0";
                         else   
                            d.leaveDays = Convert.ToString(dt.Rows[i]["leave"]);
                        d.absentDays = Convert.ToString(daysInMonth - HolidaysInMonth - Convert.ToInt32(d.leaveDays) - Convert.ToInt32(d.presentDays));
                        d.holidays = Convert.ToString(HolidaysInMonth);
                        obj.Add(d);

                    }
                }
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult filterByYearMonth(AttendanceReport arp)
        {
            if(Session["userId"] != null)
            {
                RedirectToAction("viewAllEmployeeAttendance", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }
        //Show All Data
        public ActionResult manageLogin()
        {
            if(Session["userId"] != null)
            {
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.getAllLoginData();

                List<Login> obj = new List<Login>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Login d = new Login();
                        d.userId = Convert.ToString(dt.Rows[i]["userId"]);
                        d.password = Convert.ToString(dt.Rows[i]["password"]);
                        d.userTypeId = Convert.ToString(dt.Rows[i]["userTypeId"]);
                        obj.Add(d);
                    }
                }
                    return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }
        //Create new user
        public ActionResult Create()
        {
            if(Session["userId"] != null)
            {
                Login l = new Login();
                return View(l);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(Login user)
        {
            if(Session["userId"] != null)
            {

            }
            return RedirectToAction("Index", "Home");
        }
        //Edit User Login info
        public ActionResult Edit(string id)
        {
            if(Session["userId"] != null)
            {
                Login l = new Login();
                return View(l);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Edit(Login user)
        {
            if (Session["userId"] != null)
            {
               
            }
            return RedirectToAction("Index", "Home");
        }
        // Details of a user


        //Manage Leave
        public ActionResult Leave()
        {
            if(Session["userId"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

    }
}