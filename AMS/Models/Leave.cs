using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class Leave
    {
        //empId, leave_apply_date, leave_start_date, num_leave_days, leave_reason, leave_status_id
        public string leaveId { get; set; }
        public string empId { get; set; }
        public string leave_apply_date { get; set; }
        public string leave_start_date { get; set; }
        public string num_leave_days { get; set; }
        public string leave_reason { get; set; }
        public string leave_status_id { get; set; }
        public string leave_status { get; set; }

    }
}