using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblGroup
    {
        [Key]
        public int groupID { get; set; }
        [Display(Name = "Group Name")]
        [Required(ErrorMessage = "What should be the Group name?"), MaxLength(50)]
        public string groupName { get; set; }
        [Display(Name = "Group Leader")]
        [Required(ErrorMessage = "Group leader is neccesary"), MaxLength(50)]
        public string leader { get; set; }
   
        [Display(Name = "Number of Members")]
        [Required(ErrorMessage = "How many Members?")]

     public List<tblStudents> groupMembers { get; set; }
        public int MembersCount { get; set; }
        public string groupProject { get; set; }
        public DateTime createdOn { get; set; }
        
        public string groupSupervisor { get; set; }

        public virtual ICollection<tblStudents> TblStudents { get; set; }

        public virtual ICollection<tblGroupTasks> TblGroupTasks { get; set; }
        public virtual ICollection<tblProjects> TblProjects { get; set; }
        public virtual ICollection<tblAdminTasks> TblAdminTasks { get; set; }
        public virtual ICollection<tblRequestMeeting> TblRequestMeetings { get; set; }
        public virtual ICollection<tblMeeting> TblMeetings { get; set; }
        public virtual ICollection<tblNotifications> TblNotifications { get; set; }
        
        public tblGroup()
        {
            this.createdOn = DateTime.UtcNow;
        }

    }
}