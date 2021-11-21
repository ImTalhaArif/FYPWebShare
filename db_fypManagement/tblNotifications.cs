using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblNotifications
    {
        [Key]
        public int NotificationID { get; set; }
        [Display (Name = "Invitation Title")]
        public string NotificationTitle { get; set; }
        [Display (Name = "Invitation Message")]
        public string NotificationDetails { get; set; }
        [Display (Name = "Group")]
        public string GroupName { get; set; }
        [Display (Name = "Invitation to")]
        public virtual tblStudents SendTo { get; set; }
    }
}