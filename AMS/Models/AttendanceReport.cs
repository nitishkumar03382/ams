using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class AttendanceReport
    {
        public string year { get; set; }
        public string month { get; set; }
        public string empId { get; set; }
        public string presentDays { get; set; }
        public string absentDays { get; set; }
        public string leaveDays { get; set; }
        public string holidays { get; set; }
    }
}