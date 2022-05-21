using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class Attendance
    {
        public string Employee_id { get; set; }
        public string Date { get; set; }
        public string loginTime { get; set; }
        public string logoutTime { get; set; }
        public string leaveId { get; set; }
        public string AttendanceStatus { get; set; }
    }
}