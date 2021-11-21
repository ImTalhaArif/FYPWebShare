using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblAdminTasks
    {
        [Key]
        public int AtaskID { get; set; }
        public string AtaskName { get; set; }
        public string AtaskDetails { get; set; }
        public string Group { get; set; }
        public string AtaskFile { get; set; }
        public string AtaskStatus { get; set; }
        public string AtaskRemarks { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime AtaskCreated { get; set; }




    }
}