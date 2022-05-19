using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public String EmployeeHome()
        {
            return "Hello " + TempData["uid"];

            //return View();
        }
    }
}