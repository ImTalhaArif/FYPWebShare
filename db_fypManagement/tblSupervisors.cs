using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblSupervisors
    {
        [Key]
        public int supervisorID { get; set; }
        public string supervisorName { get; set; }
        public string supervisorEmail { get; set; }
        public string supervisorPassword { get; set; }
        public string supervisorDomain { get; set; }

        public virtual ICollection<tblGroup> TblGroups { get; set; }
        public virtual ICollection<tblSupervisorProjects> TblSupervisorProjects { get; set; }
        public virtual ICollection<tblRequestMeeting> TblRequestMeetings { get; set; }

        public virtual ICollection<tblMeeting> TblMeetings { get; set; }
    }
}