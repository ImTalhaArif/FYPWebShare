using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblMeeting
    {
        [Key]
        public int meetingID { get; set; }

        public string meetingGroup { get; set; }
        public string meetingSupervisor { get; set; }

        public virtual tblGroup Group { get; set; }
        public virtual tblSupervisors Supervisor { get; set; }
        public DateTime mDatenTime { get; set; }
        public string meetingLink { get; set; }

    }
}