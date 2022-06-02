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
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if(Session["userId"] == null)
                return View();
            else
            {
                if(Convert.ToString(Session["userTypeId"]) == "1")
                    return RedirectToAction("loginAsEmployee", "Home", new { userId = Convert.ToString(Session["userId"]) });
                else
                    return RedirectToAction("loginAsAdmin", "Home", new { userId = Convert.ToString(Session["userId"]) });

            }

        }

        [HttpPost]
        public ActionResult Login(Login user)
        {
            
            AmsDataAccess objDA = new AmsDataAccess(ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
            DataTable dt = AmsDataAccess.GetLoginData(Convert.ToString(user.userId));

            if (dt.Rows.Count > 0)
            {
                Login tmpUser = new Login();
                tmpUser.userId = Convert.ToString(dt.Rows[0]["userId"]);
                tmpUser.password = Convert.ToString(dt.Rows[0]["password"]);
                tmpUser.userTypeId = Convert.ToString(dt.Rows[0]["userTypeId"]);
                if (user.password == tmpUser.password && user.userTypeId == tmpUser.userTypeId)
                {
                    TempData["uid"] = user.userId;
                    Session["userId"] = user.userId;
                    Session["userTypeId"] = user.userTypeId;
                    if (user.userTypeId == "1")
                        return RedirectToAction("loginAsEmployee", "Home", new { userId = Convert.ToString(Session["userId"]) });
                    else if(user.userTypeId == "0")
                        return RedirectToAction("loginAsAdmin", "Home", new {userId = Convert.ToString(Session["userId"]) });
                }
            }
            return RedirectToAction("messageHandler", "Home", new { msg = "Invalid Credentials", msgType="error"});
        }

        public ActionResult messageHandler(string msg, string msgType)
        {
            ViewBag.message = msg;
            ViewBag.messageType = msgType;
            return View();
        }

        public ActionResult loginAsEmployee(string username)
        {
            if(Session["userId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("EmployeeHome", "Employee");
        }

        public ActionResult loginAsAdmin()
        {
            if(Session["userId"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }

    }
}