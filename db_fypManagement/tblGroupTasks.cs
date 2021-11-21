using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblGroupTasks
    {
        [Key]
        public int groupTaskID { get; set; }
        public string AssignedTo { get; set; }
        public string gtDetails { get; set; }
        public string gtStatus { get; set; }
        public string gtTaskfile { get; set; }
        public int gtprogress { get; set; }
        public string submissionFile { get; set; }
        public DateTime gtCreatedOn { get; set; }
        public DateTime? gtDeadline { get; set; }
        public string TaskRemarks { get; set; }
       
        public DateTime? submissionDate { get; set; }
        public string Taskgroup { get; set; }

       


    }
}