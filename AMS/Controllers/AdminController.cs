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
                        string atdStat = Convert.ToString(dt.Rows[i]["AttendanceStatus"]);
                        if(atdStat == "i")
                        {
                            atdStat = "Logged In";
                        }
                        else if(atdStat == "l")
                        {
                            atdStat = "On Leave";
                        }
                        else if(atdStat == "p")
                        {
                            atdStat = "Logged Out";
                        }
                        else
                        {
                            atdStat = "Not Logged in";
                        }
                        Attendance d = new Attendance();

                        d.Employee_id = Convert.ToString(dt.Rows[i]["userId"]);
                        d.loginTime = Convert.ToString(dt.Rows[i]["loginTime"]);
                        d.logoutTime = Convert.ToString(dt.Rows[i]["logoutTime"]);
                        d.AttendanceStatus = atdStat;
                        obj.Add(d);

                    }
                }
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult viewAllEmployeeAttendance(int y, int m)
        {
            if (Session["userId"] != null)
            {
                string year = Convert.ToString(y);
                string month = Convert.ToString(m);
                ViewBag.year = y;
                ViewBag.month = m;
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
        public ActionResult filterByYearMonth(int y, int m)
        {
            if (Session["userId"] != null)
            {
                return RedirectToAction("viewAllEmployeeAttendance", "Admin", new { y = y, m = m });
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult filterByYearMonth()
        {
            if(Session["userId"] != null)
            {
                return View();
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
            if (Session["userId"] != null)
            {
                int dt = AmsDataAccess.CreateUser(user.userId, user.password, user.userTypeId);
                if (dt > 0)
                {
                    return RedirectToAction("messageHandler", "Home", new { msg = "Record Added Succesfully", msgType = "success"});
                }
                else
                {
                    return RedirectToAction("messageHandler", "Home", new { msg = "Can`t add this record!. It might exist already.", msgType = "warning" });

                }
            }

            return RedirectToAction("Index", "Home");
        }
        //Edit User Login info
        public ActionResult Edit(string id)
        {
            if(Session["userId"] != null)
            {
                ViewBag.userId = id;
                Login l = new Login();
                return View(l);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Edit(Login user, string id)
        {

            if (Session["userId"] != null)
            {
                user.userId = id;
                int dt = AmsDataAccess.EditUser(user.userId, user.password, user.userTypeId);
                if (dt > 0)
                {
                    return RedirectToAction("messageHandler", "Home", new { msg = "Record Updated Succesfully", msgType = "success" });
                }
                else
                {
                    return RedirectToAction("messageHandler", "Home", new { msg = "Something wrong happened!!", msgType = "error" });
                }
            }
            
            return RedirectToAction("Index","Home");
        }

        //DELETE
        // Delete user
        public ActionResult DeleteUser(string id)
        {
            if (Session["userId"] != null)
            {
                int dt = AmsDataAccess.DeleteUser(id);
                if (dt > 0)
                {
                    return RedirectToAction("messageHandler", "Home", new { msg = "Record Deleted Successfully", msgType = "success" });
                }
                else
                {
                    return RedirectToAction("messageHandler", "Home", new { msg = "Something wrong happened!!", msgType = "error" });
                }
            }
            return RedirectToAction("Index", "Home");
        }
        // Details of a user
        public ActionResult empDetails(string empId)
        {
            if (Session["userId"] != null)
            {
                string year, month;
                DateTime dte = DateTime.Today;
                year = dte.Year.ToString();
                month = dte.Month.ToString();
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.getEmpDetail(empId);
                DataTable dt2 = AmsDataAccess.daysAndHolidaysFromCurrDate(empId);
                int days = Convert.ToInt32(dt2.Rows[0]["TotalDays"]);
                int weekends = Convert.ToInt32(dt2.Rows[0]["Holidays"]);
                ViewBag.empId = empId;
                ViewBag.workHour = Convert.ToInt32(dt.Rows[0]["workDuration"]) / 60;
                ViewBag.workMin = Convert.ToInt32(dt.Rows[0]["workDuration"]) % 60;
                ViewBag.name = dt.Rows[0]["name"];
                ViewBag.supervisorId = dt.Rows[0]["supervisorId"];
                ViewBag.present = dt.Rows[0]["present"];
                ViewBag.leave = dt.Rows[0]["leave"];
                ViewBag.absent = days - weekends - Convert.ToInt32(dt.Rows[0]["present"]) - Convert.ToInt32(dt.Rows[0]["leave"]) + 1;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
        //Current month details of a user
        public ActionResult currMonthEmpDetails(string empId, int y, int m)
        {

            if (Session["userId"] != null)
            {
                string year, month;
                
                year = y.ToString();
                month = m.ToString();
                ViewBag.year = year;
                ViewBag.month = month;
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.getCurrMonthEmpDetail(empId,year,month);
                DataTable dt2 = AmsDataAccess.daysAndHolidaysInMonth(year, month);
                int days = Convert.ToInt32(dt2.Rows[0]["days"]);
                int weekends = Convert.ToInt32(dt2.Rows[0]["weekends"]);

                int duration = Convert.ToInt32(dt.Rows[0]["workDuration"]);
               
                ViewBag.workHour = duration / 60;
                ViewBag.workMin = duration % 60;
                ViewBag.empId = empId;
                ViewBag.name = dt.Rows[0]["name"];
                ViewBag.supervisorId = dt.Rows[0]["supervisorId"];
                ViewBag.present = dt.Rows[0]["present"];
                ViewBag.leave = dt.Rows[0]["leave"];
                ViewBag.absent = days - weekends - Convert.ToInt32(dt.Rows[0]["present"]) - Convert.ToInt32(dt.Rows[0]["leave"]);
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult filterDashBoard(int y, int m)
        {
            if (Session["userId"] != null)
            {
                string empId = Session["empId"].ToString();
                return RedirectToAction("currMonthEmpDetails", "Admin", new { empId = empId, y = y, m = m });
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult filterDashBoard(string empId)
        {
            if (Session["userId"] != null)
            {
                Session["empId"] = empId;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        //Manage Leave
        public ActionResult Leave()
        {
            if(Session["userId"] != null)
            {
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.getLast30DayPendingLeave();

                List<Leave> obj = new List<Leave>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Leave d = new Leave();
                        d.leaveId = Convert.ToString(dt.Rows[i]["leaveId"]);
                        d.empId = Convert.ToString(dt.Rows[i]["empId"]);
                        d.leave_apply_date = Convert.ToString(dt.Rows[i]["leave_apply_date"]);
                        d.leave_start_date = Convert.ToString(dt.Rows[i]["leave_start_date"]);
                        d.num_leave_days = Convert.ToString(dt.Rows[i]["num_leave_days"]);
                        d.leave_reason = Convert.ToString(dt.Rows[i]["leave_reason"]);
                        obj.Add(d);
                    }
                }
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AllTimeLeave()
        {
            if(Session["userId"] != null)
            {
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                DataTable dt = AmsDataAccess.getAllLeave();

                List<Leave> obj = new List<Leave>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Leave d = new Leave();
                        d.leaveId = Convert.ToString(dt.Rows[i]["leaveId"]);
                        d.empId = Convert.ToString(dt.Rows[i]["empId"]);
                        d.leave_apply_date = Convert.ToString(dt.Rows[i]["leave_apply_date"]);
                        d.leave_start_date = Convert.ToString(dt.Rows[i]["leave_start_date"]);
                        d.num_leave_days = Convert.ToString(dt.Rows[i]["num_leave_days"]);
                        d.leave_reason = Convert.ToString(dt.Rows[i]["leave_reason"]);
                        d.leave_status = Convert.ToString(dt.Rows[i]["leave_status"]);
                        obj.Add(d);
                    }
                }
                return View(obj);

            }
            return RedirectToAction("Index", "Home");
        }

        //Approve Leave
        public ActionResult LeaveApprove(string leaveId, string empId)
        {
            if(Session["userId"] != null)
            {
                int leaveStatus = 1; //1 is for approval
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                int AffectedRows = AmsDataAccess.manageLeave(leaveId, leaveStatus, empId);
                if(AffectedRows >= 1)
                {
                    return RedirectToAction("message", "Admin", new { msg = "Leave Approved for" + empId + "."});
                }
                else
                {
                    return RedirectToAction("message", "Admin", new {msg = "ERROR!"});
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //Reject Leave
        public ActionResult LeaveReject(string leaveId, string empId)
        {
            if (Session["userId"] != null)
            {
                int leaveStatus = 2; //2 is for rejection of leave
                AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                int AffectedRows = AmsDataAccess.manageLeave(leaveId, leaveStatus, empId);
                if (AffectedRows >= 1)
                {
                    return RedirectToAction("message", "Admin", new { msg = "Leave Rejected! for" + empId + "." });
                }
                else
                {
                    return RedirectToAction("message", "Admin", new { msg = "ERROR!" });
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public string message(string msg)
        {
            return msg;
        }
        public ActionResult logout()
        {
            Session["userId"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}