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
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login user)
        {
            Session["LOGIN"] = "TRUE";
            
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
                    if (user.userTypeId == "1")
                        return RedirectToAction("loginAsEmployee", "Home");
                    else if(user.userTypeId == "0")
                    return RedirectToAction("loginAsAdmin", "Home");
                }
            }
            return RedirectToAction("errHndle", "Home");
        }

        public string errHndle()
        {
            return "ERROR";
        }

        public ActionResult loginAsEmployee()
        {
            return RedirectToAction("EmployeeHome", "Employee");
        }

        public string loginAsAdmin()
        {

            return "Hello Admin " + Session["uid"];
        }

    }
}